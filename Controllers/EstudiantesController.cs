using GestionEstudiantes.Data;
using GestionEstudiantes.Models;
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

    public IActionResult Create()
    {
        return View(new Estudiante());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Estudiante estudiante)
    {
        if (_repositorio.ExisteMatricula(estudiante.Matricula))
        {
            ModelState.AddModelError(nameof(Estudiante.Matricula), "Ya existe un estudiante con esa matrícula.");
        }

        if (!ModelState.IsValid)
        {
            return View(estudiante);
        }

        _repositorio.Agregar(estudiante);
        TempData["Mensaje"] = $"Estudiante {estudiante.NombreCompleto} registrado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        var estudiante = _repositorio.ObtenerPorId(id);
        if (estudiante is null)
        {
            return NotFound();
        }

        return View(estudiante);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Estudiante estudiante)
    {
        if (id != estudiante.Id)
        {
            return BadRequest();
        }

        if (_repositorio.ExisteMatricula(estudiante.Matricula, estudiante.Id))
        {
            ModelState.AddModelError(nameof(Estudiante.Matricula), "Ya existe otro estudiante con esa matrícula.");
        }

        if (!ModelState.IsValid)
        {
            return View(estudiante);
        }

        if (!_repositorio.Actualizar(estudiante))
        {
            return NotFound();
        }

        TempData["Mensaje"] = $"Estudiante {estudiante.NombreCompleto} actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
