using System.Text.Json;
using GestionEstudiantes.Models;

namespace GestionEstudiantes.Data;

public class JsonEstudianteRepository : IEstudianteRepository
{
    private static readonly JsonSerializerOptions Opciones = new() { WriteIndented = true };
    private readonly string _ruta;
    private readonly object _bloqueo = new();
    private List<Estudiante> _estudiantes;

    public JsonEstudianteRepository(IWebHostEnvironment entorno)
    {
        var carpeta = Path.Combine(entorno.ContentRootPath, "App_Data");
        Directory.CreateDirectory(carpeta);
        _ruta = Path.Combine(carpeta, "estudiantes.json");

        if (File.Exists(_ruta))
        {
            var json = File.ReadAllText(_ruta);
            _estudiantes = JsonSerializer.Deserialize<List<Estudiante>>(json) ?? new List<Estudiante>();
        }
        else
        {
            _estudiantes = DatosIniciales();
            Guardar();
        }
    }

    public IReadOnlyList<Estudiante> ObtenerTodos()
    {
        lock (_bloqueo)
        {
            return _estudiantes.OrderBy(e => e.Apellido).ThenBy(e => e.Nombre).ToList();
        }
    }

    public Estudiante? ObtenerPorId(int id)
    {
        lock (_bloqueo)
        {
            return _estudiantes.FirstOrDefault(e => e.Id == id);
        }
    }

    public void Agregar(Estudiante estudiante)
    {
        lock (_bloqueo)
        {
            estudiante.Id = _estudiantes.Count == 0 ? 1 : _estudiantes.Max(e => e.Id) + 1;
            _estudiantes.Add(estudiante);
            Guardar();
        }
    }

    public bool Actualizar(Estudiante estudiante)
    {
        lock (_bloqueo)
        {
            var indice = _estudiantes.FindIndex(e => e.Id == estudiante.Id);
            if (indice < 0)
            {
                return false;
            }

            _estudiantes[indice] = estudiante;
            Guardar();
            return true;
        }
    }

    public bool Eliminar(int id)
    {
        lock (_bloqueo)
        {
            var eliminados = _estudiantes.RemoveAll(e => e.Id == id);
            if (eliminados == 0)
            {
                return false;
            }

            Guardar();
            return true;
        }
    }

    public bool ExisteMatricula(string matricula, int? excluirId = null)
    {
        lock (_bloqueo)
        {
            return _estudiantes.Any(e =>
                string.Equals(e.Matricula, matricula, StringComparison.OrdinalIgnoreCase) &&
                e.Id != excluirId);
        }
    }

    private void Guardar()
    {
        var json = JsonSerializer.Serialize(_estudiantes, Opciones);
        File.WriteAllText(_ruta, json);
    }

    private static List<Estudiante> DatosIniciales() => new()
    {
        new Estudiante { Id = 1, Matricula = "2024-0101", Nombre = "María", Apellido = "Rodríguez", Correo = "maria.rodriguez@correo.com", Carrera = "Desarrollo de Software", Indice = 3.75m, FechaIngreso = new DateTime(2024, 1, 15) },
        new Estudiante { Id = 2, Matricula = "2024-0102", Nombre = "Luis", Apellido = "Martínez", Correo = "luis.martinez@correo.com", Carrera = "Redes de Información", Indice = 3.20m, FechaIngreso = new DateTime(2024, 1, 15) },
        new Estudiante { Id = 3, Matricula = "2025-0201", Nombre = "Carla", Apellido = "Peña", Correo = "carla.pena@correo.com", Carrera = "Seguridad Informática", Indice = 3.90m, FechaIngreso = new DateTime(2025, 5, 5) }
    };
}
