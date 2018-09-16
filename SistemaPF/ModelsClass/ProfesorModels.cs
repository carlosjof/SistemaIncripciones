using Microsoft.AspNetCore.Identity;
using SistemaPF.Data;
using SistemaPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaPF.ModelsClass
{
    public class ProfesorModels
    {
        private ApplicationDbContext context;
        private List<IdentityError> identityError;
        private string code = "", des = "";
        private Boolean estados;


        public ProfesorModels(ApplicationDbContext context) {
            this.context = context;
            identityError = new List<IdentityError>();
        }

        public List<Profesor> getProfesores(int id) {
            return context.Profesor.Where(p => p.ID == id).ToList();
        }

        public List<IdentityError> guardarProfesor(List<Profesor> response, int funcion ) {
            switch (funcion)
            {
                case 0:
                    if (response[0].Estado)
                    {
                        estados = false;
                    }
                    else
                    {
                        estados = true;
                    }
                    break;
                case 1:
                    estados = response[0].Estado;
                    break;
            }

            var profesor = new Profesor
            {

                ID = response[0].ID,
                Especialidad = response[0].Especialidad,
                Apellidos = response[0].Apellidos,
                Nombres = response[0].Nombres,
                FechaNacimiento = response[0].FechaNacimiento,
                Cedula = response[0].Cedula,
                Email = response[0].Email,
                Telefono = response[0].Telefono,
                Direccion = response[0].Direccion,
                Estado = estados

            };
            try
            {
                context.Update(profesor);
                context.SaveChanges();
                code = "1";
                des = "Save";
            }
            catch (Exception ex)
            {
                code = "0";
                des = ex.Message;
            }
            identityError.Add(new IdentityError {
                Code = code,
                Description = des
            });
            return identityError;
        }

        public List<object[]> filtrarProfesores(int numPag, string valor, string order)
        {
            int cant, numRegistros = 0, inicio = 0, reg_por_pagina = 10;
            int can_paginas, paginas, count = 1;
            string dataFilter = "", paginador = "", Estado = null;
            List<object[]> data = new List<object[]>();
            IEnumerable<Profesor> query;
            List<Profesor> profesor = null;

            profesor = context.Profesor.OrderBy(p => p.Nombres).ToList();
            numRegistros = profesor.Count();
            inicio = (numPag - 1) * reg_por_pagina;
            can_paginas = (numRegistros / reg_por_pagina);

            if (valor == "null")
            {
                query = profesor.Skip(inicio).Take(reg_por_pagina);
            }
            else
            {
                query = profesor.Where(p => p.Cedula.StartsWith(valor) || p.Nombres.StartsWith(valor) || p.Apellidos.StartsWith(valor)).Skip(inicio).Take(reg_por_pagina);

            }
            cant = query.Count();

            foreach (var item in query)
            {
                if (item.Estado == true)
                {
                    Estado = "<a onclick='editarProfesor(" + item.ID + ',' + 0 + ")' class='btn btn-success'>Activo</a>";
                }
                else
                {
                    Estado = "<a onclick='editarProfesor(" + item.ID + ',' + 0 + ")' class='btn btn-danger'>Desactivado</a>";
                }
                dataFilter += "<tr>" +
                "<td>" + item.Especialidad + "</td>" +
                "<td>" + item.Cedula + "</td>" +
                "<td>" + item.Nombres + "</td>" +
                "<td>" + item.Apellidos + "</td>" +
                "<td>" + item.FechaNacimiento + "</td>" +
                "<td>" + item.Telefono + "</td>" +
                "<td>" + item.Email + "</td>" +
                "<td>" + item.Direccion + "</td>" +
                "<td>" + Estado + "</td>" +
                "<td>" +
                "<a data-toggle='modal' data-target='#modalNP' onclick='editarProfesor(" + item.ID + ',' + 1 + ")' class='btn btn-success'>Editar</a>" +
                "</td>" +

                "<td>" +
                "<a data-toggle='modal' data-target='#modalEP' onclick='eliminarProfesor(" + item.ID + ")' class='btn btn-danger'>Eliminar</a>" +
                "</td>" +

                "</tr>";
            }

            if (valor == "null")
            {
                if (numPag > 1)
                {
                    paginas = numPag - 1;
                    paginador += "<a class='btn btn-default' onclick='filtrarProfesores(" + 1 + ',' + '"' + order + '"' + ")'> << </a>" +
                     "<a class='btn btn-default' onclick='filtrarProfesores(" + paginas + ',' + '"' + order + '"' + ")'> < </a>";
                }
                if (1 < can_paginas)
                {

                    for (int i = numPag; i < can_paginas; i++)
                    {
                        paginador += "<strong class='btn btn-success' onclick='filtrarProfesores(" + i + ',' + '"' + order + '"' + ")'>" + i + "</strong>";
                        if (count == 5)
                        {
                            break;
                        }
                        count++;
                    }
                }
                if (numPag < can_paginas)
                {
                    paginas = numPag + 1;
                    paginador += "<a class='btn btn-default' onclick='filtrarProfesores(" + paginas + ',' + '"' + order + '"' + ")'> > </a>" +
                         "<a class='btn btn-default' onclick='filtrarProfesores(" + can_paginas + ',' + '"' + order + '"' + ")'> >> </a>";
                }
            }

            object[] dataObj = { dataFilter, paginador };
            data.Add(dataObj);
            return data;

        }

        internal List<IdentityError> eliminarProfesor(int id)
        {
            var profesor = context.Profesor.SingleOrDefault(e => e.ID == id);
            if (profesor == null)
            {
                code = "0";
                des = "Not";
            }
            else
            {
                context.Profesor.Remove(profesor);
                context.SaveChanges();
                code = "1";
                des = "Save";
            }
            identityError.Add(new IdentityError
            {

                Code = code,
                Description = des

            });
            return identityError;
        }

    }
}
