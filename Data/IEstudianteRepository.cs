using GestionEstudiantes.Models;

namespace GestionEstudiantes.Data;

public interface IEstudianteRepository
{
    IReadOnlyList<Estudiante> ObtenerTodos();
    Estudiante? ObtenerPorId(int id);
    void Agregar(Estudiante estudiante);
    bool Actualizar(Estudiante estudiante);
    bool Eliminar(int id);
    bool ExisteMatricula(string matricula, int? excluirId = null);
}
