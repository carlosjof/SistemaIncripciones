using SistemaPF.Data;
using SistemaPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaPF.ModelsClass
{
    public class ListaObjetos
    {
        public List<object[]> data = new List<object[]>();
        public ApplicationDbContext context;
        public List<Inscripcion> dataInscripcion = new List<Inscripcion>();
        public List<Cursos> cursos = new List<Cursos>();
    }
}
