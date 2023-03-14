using System;
using System.Collections.Generic;

namespace DL;

public partial class Alumno
{
    public int IdAlumno { get; set; }

    public string? Nombre { get; set; }

    public string? ApellidoPaterno { get; set; }

    public string? ApellidoMaterno { get; set; }

    public byte? IdSemestre { get; set; }

    public DateTime? FechaNacimiento { get; set; }

    public string? Imagen { get; set; }

    public virtual ICollection<Horario> Horarios { get; } = new List<Horario>();

    public virtual Semestre? IdSemestreNavigation { get; set; }
    public string? SemestreNombre { get; set; }
}
