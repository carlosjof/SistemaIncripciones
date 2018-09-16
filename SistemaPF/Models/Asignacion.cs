using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaPF.Models
{
    public class Asignacion
    {
        public int AsignacionID { get; set; }
        public int CursoID { get; set; }
        public int ProfesorID { get; set; }
        public DateTime Fecha { get; set; }

    }
}
