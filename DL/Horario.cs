using System;
using System.Collections.Generic;

namespace DL;

public partial class Horario
{
    public int IdHorario { get; set; }

    public string? Turno { get; set; }

    public int? IdAlumno { get; set; }

    public virtual Alumno? IdAlumnoNavigation { get; set; }




    
}
