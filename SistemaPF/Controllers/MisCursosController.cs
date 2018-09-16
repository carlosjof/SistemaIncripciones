using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SistemaPF.Data;
using SistemaPF.Models;
using SistemaPF.ModelsClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaPF.Controllers
{
    public class MisCursosController : Controller
    {
        private MisCursosModels misCursos;
        public MisCursosController(ApplicationDbContext context) {
            misCursos = new MisCursosModels(context);
        }
        [Authorize(Roles = "Administrador, Asistente")]
        public IActionResult Index() {
            return View();
        }

        public List<object[]> filtrarMisCurso(int numPag, string valor, int funcion) {
            return misCursos.filtrarMisCurso(numPag, valor, funcion);
        }

        public List<IdentityError> actualizarMisCursos(String data)
        {
            var array = JArray.Parse(data);
            //la coleccion la vamos a guardar en datosCurso
            var datosCurso = array[0];

            DatosCurso modelo = JsonConvert.DeserializeObject<DatosCurso>(datosCurso.ToString());

            return misCursos.actualizarMisCursos(modelo);
        }

        public List<object[]> reportesCursos(String valor, int numPag, int funcion) {

            String thead = "<tr>" +
                "<th>Curso</th>" +
                "<th>Estudiante</th>" +
                "<th>Profesor</th>" +
                "<th>Grado</th>" +
                "<th>Pago</th>" +
                "<th>Fecha</th>" +
                "</tr>";

            object[] dataObj = { thead };
            var reportes = misCursos.filtrarMisCurso(numPag, valor, funcion);
            reportes.Add(dataObj);
            return reportes;
        }
    }
}
