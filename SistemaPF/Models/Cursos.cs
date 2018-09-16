using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaPF.Models
{
    public class Cursos
    {
        [Key]
        public int CursoID { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public byte Creditos { get; set; }

        public decimal Costo { get; set; }

        public Boolean Estado { get; set; } = true;

        public int CategoriaID { get; set; }

        public Categoria Categoria {get; set;}
    }
}
