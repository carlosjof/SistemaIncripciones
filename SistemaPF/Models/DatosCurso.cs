using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaPF.Models
{
    public class DatosCurso: Inscripcion
    {
        public string Curso { get; set; }
        public string Estudiante { get; set; }
        public string Profesor { get; set; }
    }
}
