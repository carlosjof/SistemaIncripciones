using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using SistemaPF.Data;
using SistemaPF.Models;

namespace SistemaPF.ModelsClass
{
    public class MisCursosModels : ListaObjetos
    {
        private string dataFilter = "", paginador = "", curso;
        private int count = 0, cant, numReg = 0, inicio = 0, reg_por_pagina = 20;
        private int can_paginas, pagina;
        private string code = "", des = "";
        private List<IdentityError> errorList = new List<IdentityError>();
        public MisCursosModels(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<object[]> filtrarMisCurso(int numPag, string valor, int funcion ) {

            count = 0;
            var inscripcion = context.Inscripcion.OrderBy(i => i.Fecha).ToList();
            numReg = inscripcion.Count;
            if ((numReg % reg_por_pagina) > 0)
            {
                numReg += 1;
            }
            inicio = (numPag - 1) * reg_por_pagina;
            can_paginas = (numReg / reg_por_pagina);
            if (valor == "null")
            {
                dataInscripcion = inscripcion.Skip(inicio).Take(reg_por_pagina).ToList();
            }
            else
            {
                cursos = getCursos(valor);
                cursos.ForEach(item =>
                {
                    var data = inscripcion.Where(i => i.CursoID == item.CursoID).Skip(inicio).Take(reg_por_pagina).ToList();
                    if (data.Count > 0)
                    {
                        var verinscripciones = new Inscripcion
                        {
                            Grado = data[0].Grado,
                            CursoID = data[0].CursoID,
                            EstudianteID = data[0].EstudianteID,
                            Fecha = data[0].Fecha,
                            Pago = data[0].Pago
                        };
                        dataInscripcion.Add(verinscripciones);

                    }
                });
            }
            foreach (var item in dataInscripcion)
            {
                if (cursos.Count > 0)
                {
                    curso = cursos[count].Nombre;

                }
                else
                {
                    curso = getCurso(item.CursoID);
                    object[] dataCurso = {
                        curso, 
                        getEstudiante(item.EstudianteID),
                        getProfesor(getAsignacion(item.CursoID)),
                        item.Grado,
                        item.Pago,
                        item.Fecha
                    };

                        dataFilter += "<tr>" +
                         "<td>" + curso + "</td>" +
                         "<td>" + getEstudiante(item.EstudianteID) + "</td>" +
                         "<td>" + getProfesor(getAsignacion(item.CursoID)) + "</td>" +
                         "<td>" + item.Grado + "</td>" +
                         "<td>" + '$' +item.Pago + "</td>" +
                         "<td>" + item.Fecha + "</td>" +
                            dataBoton(item, funcion)
                         +
                         "</tr>";   
                    count++;

                }
            }
            object[] dataObj = { dataFilter, paginador };
            data.Add(dataObj);
            return data;
        }

        internal List<IdentityError> actualizarMisCursos(DatosCurso model)
        {
            var curso = context.Cursos.Where(c => c.Nombre.Equals(model.Curso)).ToList();
            //separar los datos de los estudiantes
            var estudiantes = model.Estudiante.Split();
            var estudiante = context.Estudiante.Where(e => e.Nombres.Equals(estudiantes[0]) || e.Apellidos.Equals(estudiantes[1])).ToList();

            var inscripcion = new Inscripcion
            {
                InscripcionID = model.InscripcionID,
                Grado = model.Grado,
                CursoID = curso[0].CursoID,
                EstudianteID = estudiante[0].ID,
                Fecha = model.Fecha,
                Pago = model.Pago

            };
            try
            {
                context.Update(inscripcion);
                context.SaveChanges();
                code = "Save";
                des = "Save";
            }
            catch (Exception ex)
            {
                code = "Error";
                des = ex.Message;
            }
            errorList.Add(new IdentityError {

                Code = code,
                Description = des
            });
            return errorList;
        }

        public List<Cursos> getCursos(String curso) {
            return context.Cursos.Where(c => c.Nombre.StartsWith(curso)).ToList();

        }


        //para obtener los nombres del curso por id
        public String getCurso(int id)
        {
            var curso = context.Cursos.Where(c => c.CursoID == id).ToList();
            return curso[0].Nombre;
        }

        private string getEstudiante(int estudianteId) {
            var estudiante = context.Estudiante.Where(e => e.ID == estudianteId).ToList();
            return estudiante[0].Nombres + " "+ estudiante[0].Apellidos;
        }

        private int getAsignacion(int id) {
            var asignacion = context.Asignacion.Where(a => a.CursoID == id).ToList();
            return asignacion[0].ProfesorID;
        }

        private string getProfesor(int id) {
            var profesor = context.Profesor.Where(p => p.ID == id).ToList();
            return profesor[0].Nombres + " " + profesor[0].Apellidos;
        }

        private String dataBoton(Inscripcion item, int funcion) {
            String data = "";
            if (funcion == 1)
            {
                object[] dataCurso = {
                        curso,
                        getEstudiante(item.EstudianteID),
                        getProfesor(getAsignacion(item.CursoID)),
                        item.Grado,
                        item.Pago,
                        item.Fecha
                    };
                data += "<td>" +
                         "<a data-toggle='modal' data-target='#modalMisCurso' onclick='getMisCursos(" + JsonConvert.SerializeObject(dataCurso) + ',' + item.InscripcionID + ")' class='btn btn-success'>Editar</a>" +
                         "</td>";
            }
            return data;
        }
    }
}
