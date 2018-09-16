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
    public class CursosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private CursosModels cursosModels;

        public CursosController(ApplicationDbContext context)
        {
            _context = context;
            cursosModels = new CursosModels(context);
        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public List<Categoria> getCategorias() {
            return cursosModels.getCategorias();
        }

        public List<IdentityError> agregarCurso(int id, string nombre, string descripcion, byte creditos, decimal costo, Boolean estado, int categoria, string funcion) {
            return cursosModels.agregarCurso(id, nombre, descripcion, creditos, costo, estado, categoria, funcion);
        }

        public List<object[]> filtrarCurso(int numPag, string valor, string order) {
            return cursosModels.filtrarCurso(numPag, valor, order);
        }

        public List<Cursos> getCursos(int id) {
            return cursosModels.getCursos(id);
        }

        public List<IdentityError> editarCurso(int id, string nombre, string descripcion, byte creditos, decimal costo, Boolean estado, int categoria, int funcion) {
            return cursosModels.editarCurso(id, nombre, descripcion, creditos, costo, estado, categoria, funcion);
        }

        public List<Profesor> getProfesores() {
            return cursosModels.getProfesores();
        }

        public List<IdentityError> profesorCurso(List<Asignacion> asignacion) {
            return cursosModels.profesorCurso(asignacion);
        }

    }
}
