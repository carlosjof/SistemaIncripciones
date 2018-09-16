using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SistemaPF.Data;
using SistemaPF.Models;

namespace SistemaPF.ModelsClass {
    public class CursosModels {

        private ApplicationDbContext context;
        private List<IdentityError> errorList = new List<IdentityError>();
        private string code = "", des = "";
        private Boolean estados;
        public CursosModels(ApplicationDbContext context) {
            this.context = context;
        }

        internal List<Categoria> getCategorias() {
            return context.Categoria.Where(c => c.Estado == true).ToList();
        }

        public List<Categoria> getCategoria(int id) {
            return context.Categoria.Where(c => c.CategoriaID == id).ToList();
        }

        public List<Cursos> getCursos(int id) {
            return context.Cursos.Where(c => c.CursoID == id).ToList();
        }

        public List<IdentityError> agregarCurso(int id, string nombre, string descripcion, byte creditos, decimal costo, Boolean estado, int categoria, string funcion) {
            var curso = new Cursos
            {

                Nombre = nombre,
                Descripcion = descripcion,
                Creditos = creditos,
                Costo = costo,
                Estado = estado,
                CategoriaID = categoria
            };
            try
            {
                context.Add(curso);
                context.SaveChanges();
                code = "Save";
                des = "Save";
            }
            catch (Exception ex)
            {
                code = "error";
                des = ex.Message;
            }
            errorList.Add(new IdentityError
            {
                Code = code,
                Description = des
            });
            return errorList;
        }

        public List<object[]> filtrarCurso(int numPag, string valor, string order) {

            int cant, numRegistros = 0, inicio = 0, reg_por_pagina = 10;
            int can_paginas, paginas;
            string dataFilter = "", paginador = "", Estado = null;
            List<object[]> data = new List<object[]>();
            IEnumerable<Cursos> query;
            List<Cursos> cursos = null;
            switch (order)
            {
                case "nombre":
                    cursos = context.Cursos.OrderBy(c => c.Nombre).ToList();
                    break;
                case "des":
                    cursos = context.Cursos.OrderBy(c => c.Descripcion).ToList();
                    break;
                case "creditos":
                    cursos = context.Cursos.OrderBy(c => c.Creditos).ToList();
                    break;
                case "costo":
                    cursos = context.Cursos.OrderBy(c => c.Costo).ToList();
                    break;
                case "estado":
                    cursos = context.Cursos.OrderBy(c => c.Estado).ToList();
                    break;
                case "categoria":
                    cursos = context.Cursos.OrderBy(c => c.Categoria).ToList();
                    break;  
            }

            numRegistros = cursos.Count;
            inicio = (numPag - 1) * reg_por_pagina;
            can_paginas = (numRegistros / reg_por_pagina);
            if (valor == "null")
            {
                query = cursos.Skip(inicio).Take(reg_por_pagina);
            }
            else
            {
                query = cursos.Where(c => c.Nombre.StartsWith(valor) || c.Descripcion.StartsWith(valor));
            }
            cant = query.Count();
            foreach (var item in query)
            {
                var categoria = getCategoria(item.CategoriaID);
                if (item.Estado == true)
                {
                    Estado = "<a data-toggle='modal' data-target='#modalEstadoCurso' onclick='editarEstadoCurso(" + item.CursoID + ',' + 0 + ")' class='btn btn-success'>Activo</a>";
                }
                else
                {
                    Estado = "<a data-toggle='modal' data-target='#modalEstadoCurso' onclick='editarEstadoCurso(" + item.CursoID + ',' + 0 + ")' class='btn btn-danger'>Desactivado</a>";
                }
                dataFilter += "<tr>" +
                    "<td>" + item.Nombre + "</td>" +
                    "<td>" + item.Descripcion + "</td>" +
                    "<td>" + item.Creditos + "</td>" +
                    "<td>" + item.Costo + "</td>" +
                    "<td>" + Estado + "</td>" +
                    "<td>" + categoria[0].Nombre + "</td>" +
                    "<td>" +
                    "<a data-toggle='modal' data-target='#modalNC' onclick='editarEstadoCurso(" + item.CursoID + ',' + 1 + ")' class='btn btn-success'>Editar</a>" +
                    "</td>" +
                    "<td>"+
                    getProfesoresCurso(item.CursoID)+
                    "</td>"+
                    "</tr>";
            }
            if (valor == "null")
            {
                if (numPag > 1)
                {
                    paginas = numPag - 1;
                    paginador += "<a class='btn btn-default' onclick='filtrarCurso(" + 1 + ',' + '"' + order + '"' + ")'> << </a>" +
                         "<a class='btn btn-default' onclick='filtrarCurso(" + paginas + ',' + '"' + order + '"' + ")'> < </a>";
                }
                if (1 < can_paginas)
                {
                    paginador += "<strong class'btn btn-success'>" + numPag + "/" + can_paginas + "</strong>";
                }
                if (numPag < can_paginas)
                {
                    paginas = numPag + 1;
                    paginador += "<a class='btn btn-default' onclick='filtrarCurso(" + paginas + ',' + '"' + order + '"' + ")'> > </a>" +
                         "<a class='btn btn-default' onclick='filtrarCurso(" + can_paginas + ',' + '"' + order + '"' + ")'> >> </a>";
                }
            }
            object[] dataObj = { dataFilter, paginador };
            data.Add(dataObj);
            return data;
        }

        public List<IdentityError> editarCurso(int id, string nombre, string descripcion, byte creditos, decimal costo, Boolean estado, int categoriaID, int funcion) {
            switch (funcion)
            {
                case 0:
                    if (estado)
                    {
                        estados = false;
                    }
                    else
                    {
                        estados = true;
                    }
                    break;
                case 1:
                    estados = estado;
                    break;
            }
            var curso = new Cursos
            {
                CursoID = id, 
                Nombre = nombre,
                Descripcion = descripcion,
                Creditos = creditos,
                Costo = costo,
                Estado = estado,
                CategoriaID = categoriaID
            };
            try
            {
                context.Update(curso);
                context.SaveChanges();
                code = "Save";
                des = "Save";
            }
            catch (Exception ex)
            {
                code = "error";
               des = ex.Message;
            }
            errorList.Add(new IdentityError
            {
                Code = code,
                Description = des
            });
            return errorList;
        }
        private String getProfesoresCurso(int cursoID) {

            String boton;
            var data = context.Asignacion.Where(a => a.CursoID == cursoID).ToList();
            if (0 < data.Count)
            {
                boton = "<a data-toggle='modal' data-target='.bs-example-modal-sm' onclick='getProfesorCurso(" + data[0].AsignacionID + ',' + cursoID + ',' + data[0].ProfesorID + ',' + 2 + ")' class='btn btn-info'>Actualizar</a>";
            }
            else
            {
                boton = "<a data-toggle='modal' data-target='.bs-example-modal-sm' onclick='getProfesorCurso(" + 0 + ',' + cursoID + ',' + 0 + ',' + 3 + ")' class='btn btn-info'>Asignar</a>";
            }
            return boton;
        }
        internal List<Profesor> getProfesores() {

            return context.Profesor.Where(p => p.Estado == true).ToList();
        }

        internal List<IdentityError> profesorCurso(List<Asignacion> asig)
        {

            var asignacion = new Asignacion {

                AsignacionID = asig[0].AsignacionID,
                CursoID = asig[0].CursoID,
                ProfesorID = asig[0].ProfesorID,
                Fecha = asig[0].Fecha
            };
            try
            {
                context.Update(asignacion);
                context.SaveChanges();
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

    }
}
