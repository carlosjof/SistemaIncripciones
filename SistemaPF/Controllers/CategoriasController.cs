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
    public class CategoriasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private CategoriaModels categoriaModels;

        public CategoriasController(ApplicationDbContext context)
        {
            _context = context;
            categoriaModels = new CategoriaModels(_context);
        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        //metodo que va recibir los datos por ajax
        public List<object[]> filtrarDatos(int numPag, string valor, string order)
        {
            return categoriaModels.filtrarDatos(numPag, valor, order);
        }

        public List<Categoria> getCategoria(int id)
        {

            return categoriaModels.getCategorias(id);

        }

        public List<IdentityError> editarCategoria(int id, string nombre, string descripcion, Boolean estado, int funcion)
        {

            return categoriaModels.editarCategoria(id, nombre, descripcion, estado, funcion);

        }

        public List<IdentityError> guardarCategoria(string nombre, string descripcion, string estado)
        {

            return categoriaModels.guardarCategoria(nombre, descripcion, estado);

        }
    }
}