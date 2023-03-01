﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Alumno
    {
        public int IdAlumno { get; set; }

        [Required]
        [RegularExpression("[a-zA-Z]")]
        public string Nombre { get; set; }

        [Required]
        public string ApellidoPaterno { get; set; }

        public string ApellidoMaterno { get; set; }
        public string FechaNacimiento { get; set; }
        public ML.Horarios Horario { get; set; }
        public ML.Semestre Semestre { get; set; }
        public string NombreCompleto { get; set; }
        public string Imagen { get; set; }
        public List<object> Alumnos { get; set; }
    }
}
