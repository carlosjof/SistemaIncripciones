using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SistemaPF.Data;
using SistemaPF.Models;

namespace SistemaPF.ModelsClass
{
    public class InscripcionesModels
    {
        private ApplicationDbContext context;
        private List<IdentityError> errorList = new List<IdentityError>();
        private string code = "", des = "";
        public InscripcionesModels(ApplicationDbContext context)
        {
            this.context = context;
        }

        public String filtrarEstudiantes(string valor) {
            String dataFilter = "";
            if (valor != "null")
            {
                var estudiantes = context.Estudiante.OrderBy(e => e.Nombres).ToList();
                var query = estudiantes.Where(e => e.Cedula.StartsWith(valor) || e.Nombres.StartsWith(valor) || e.Apellidos.StartsWith(valor));
                foreach (var item in query)
                {
                    dataFilter += "<tr>" +
                        "<td>" + "<input type='checkbox' name='chkboxEstudiante[]' id='chkboxEstudiante' value='" + item.ID + "'>" + "</td>" +
                        "<td>" + item.Nombres + " " + item.Apellidos + "</td>" +
                        "<td>" + item.Cedula + "</td>" +
                        "<td>" + item.Email + "</td>" +
                        "<td>" + item.Telefono + "</td>" +
                        "</tr>";
                }
            }
            return dataFilter;
        }

        internal List<IdentityError> guardarCursos(List<Inscripcion> listCursos)
        {
            try
            {
                for (int i = 0; i < listCursos.Count; i++)
                {
                    context.Add(listCursos[i]);
                    context.SaveChanges();
                }
                code = "Save";
                des = "Save";
            }
            catch (Exception ex)
            {

                code = "Error";
                des = ex.Message;
            }
            errorList.Add(new IdentityError
            {
                Code = code,
                Description = des
            });
            return errorList;
        }

        internal List<Cursos> getCurso(int id)
        {
            return context.Cursos.Where(c => c.CursoID == id).ToList();
        }

        internal string filtrarCursos(string valor)
        {
            String dataFilter = "";
            if (valor != "null")
            {

                var listCursos = from c in context.Cursos
                                 join a in context.Asignacion on c.CursoID equals a.CursoID
                                 select new
                                 {
                                     c.CursoID,
                                     c.Nombre,
                                     c.CategoriaID,
                                     c.Creditos,
                                     c.Costo,
                                 };


                var curso = listCursos.OrderBy(c => c.Nombre).ToList();
                var query = curso.Where(c => c.Nombre.StartsWith(valor));
                foreach (var item in query)
                {
                    dataFilter += "<tr>" +
                        "<td>" + "<input type='checkbox' name='chkboxCurso[]' id='chkboxCurso' value='" + item.CursoID + "'>" + "</td>" +
                        "<td>" + item.Nombre +
                        "<td>" + getCategorias(item.CategoriaID) + "</td>" +
                        "<td>" + item.Creditos + "</td>" +
                        "<td>" + item.Costo + "</td>" +
                        "</tr>";
                }
            }
            return dataFilter;
        }

        public List<Estudiante> getEstudiante(int id)
        {
            return context.Estudiante.Where(e => e.ID == id).ToList();
        }

        public String getCategorias(int id) {
            var data = context.Categoria.Where(c => c.CategoriaID == id).ToList();
            return data[0].Nombre;
        }
    }
}
