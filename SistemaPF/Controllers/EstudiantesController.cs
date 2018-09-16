using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaPF.Data;
using SistemaPF.Models;
using SistemaPF.ModelsClass;

namespace SistemaPF.Controllers
{
    public class EstudiantesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private EstudianteModels estudiante;

        public EstudiantesController(ApplicationDbContext context)
        {
            _context = context;
            estudiante = new EstudianteModels(context);
        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public List<IdentityError> guardarEstudiantes(List<Estudiante> response, int funcion) {
            return estudiante.guardarEstudiantes(response, funcion);
        }

        public List<object[]> filtrarEstudiantes(int numPag, string valor, string order) {
            return estudiante.filtrarEstudiantes(numPag, valor, order);
        }

        public List<Estudiante> getEstudiantes(int id) {
            return estudiante.getEstudiantes(id);
        }

        public List<IdentityError> eliminarEstudiante(int id) {
            return estudiante.eliminarEstudiante(id);
        }
    }
}
