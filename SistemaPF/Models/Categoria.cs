using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaPF.Models
{
    public class Categoria
    {
        
        //proppiedades
        public int CategoriaID { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public Boolean Estado { get; set; } = true;
        
        //objeto interface IColletion
        public ICollection<Cursos> Cursos { get; set; }
    }
}
