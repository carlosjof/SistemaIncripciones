using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemaPF.Data;
using SistemaPF.Models;
using SistemaPF.ModelsClass;

namespace SistemaPF.Controllers
{
    public class Inscripciones : Controller
    {
        private InscripcionesModels inscripcion;

        public Inscripciones(ApplicationDbContext context) {
            inscripcion = new InscripcionesModels(context);
        }
        [Authorize(Roles = "Administrador, Asistente")]
        public IActionResult Index()
        {
            return View();
        }

        public String filtrarEstudiantes(string valor) {
           return inscripcion.filtrarEstudiantes(valor);
        }

        public List<Estudiante> getEstudiante(int id) {
            return inscripcion.getEstudiante(id);
        }

        public String filtrarCursos(string valor) {
            return inscripcion.filtrarCursos(valor);
        }

        public List<Cursos> getCurso(int id) {
            return inscripcion.getCurso(id);
        }

        public List<IdentityError> guardarCursos(List<Inscripcion> listCursos)
        {
            return inscripcion.guardarCursos(listCursos);
        }

    }
}