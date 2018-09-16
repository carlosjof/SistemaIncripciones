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
    public class ProfesorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public readonly ProfesorModels profesor;

        public ProfesorsController(ApplicationDbContext context)
        {
            _context = context;
            profesor = new ProfesorModels(context);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Profesor.ToListAsync());
        }



        public List<IdentityError> guardarProfesor(List<Profesor> response, int funcion)
        {
            return profesor.guardarProfesor(response, funcion);
        }

        public List<object[]> filtrarProfesores(int numPag, string valor, string order)
        {
            return profesor.filtrarProfesores(numPag, valor, order);
        }

        public List<Profesor> getProfesores(int id)
        {
            return profesor.getProfesores(id);
        }

        public List<IdentityError> eliminarProfesor(int id)
        {
            return profesor.eliminarProfesor(id);
        }
    }
}
