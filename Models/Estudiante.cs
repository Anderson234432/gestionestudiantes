using System.ComponentModel.DataAnnotations;

namespace GestionEstudiantes.Models;

public class Estudiante
{
    public int Id { get; set; }

    [Required(ErrorMessage = "La matrícula es obligatoria.")]
    [RegularExpression(@"^\d{4}-\d{4}$", ErrorMessage = "La matrícula debe tener el formato 0000-0000.")]
    [Display(Name = "Matrícula")]
    public string Matricula { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(60, ErrorMessage = "El nombre no puede superar 60 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(60, ErrorMessage = "El apellido no puede superar 60 caracteres.")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no es válido.")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La carrera es obligatoria.")]
    [StringLength(80, ErrorMessage = "La carrera no puede superar 80 caracteres.")]
    public string Carrera { get; set; } = string.Empty;

    [Range(0, 4, ErrorMessage = "El índice debe estar entre 0 y 4.")]
    [Display(Name = "Índice académico")]
    public decimal Indice { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Fecha de ingreso")]
    public DateTime FechaIngreso { get; set; } = DateTime.Today;

    [Display(Name = "Nombre completo")]
    public string NombreCompleto => $"{Nombre} {Apellido}";
}
