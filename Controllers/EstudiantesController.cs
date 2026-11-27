using GestionEstudiantes.Data;
using Microsoft.AspNetCore.Mvc;

namespace GestionEstudiantes.Controllers;

public class EstudiantesController : Controller
{
    private readonly IEstudianteRepository _repositorio;

    public EstudiantesController(IEstudianteRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public IActionResult Index(string? buscar)
    {
        var estudiantes = _repositorio.ObtenerTodos();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            var texto = buscar.Trim();
            estudiantes = estudiantes
                .Where(e => e.Matricula.Contains(texto, StringComparison.OrdinalIgnoreCase)
                         || e.NombreCompleto.Contains(texto, StringComparison.OrdinalIgnoreCase)
                         || e.Carrera.Contains(texto, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        ViewData["Buscar"] = buscar;
        return View(estudiantes);
    }

    public IActionResult Details(int id)
    {
        var estudiante = _repositorio.ObtenerPorId(id);
        if (estudiante is null)
        {
            return NotFound();
        }

        return View(estudiante);
    }
}
