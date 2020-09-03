using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize]
    public class CategoriasController : Controller
    {
        private readonly ICategoriasRepository repository; 
        private readonly IUserHelper userHelper;

        public CategoriasController(ICategoriasRepository repository, IUserHelper userHelper)
        {
            this.repository = repository;
            this.userHelper = userHelper;
        }

        public IActionResult Index()
        {
            return View(repository.GetCategorias());
        }

        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Categorias = this.repository.GetCategorias().Where( x => x.Id == id).FirstOrDefault();
            if (Categorias == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(Categorias);
        }

        public IActionResult Create()
        {
            ViewBag.Categorias = repository.GetCombo();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParamCategorias Categorias)
        {
            if (ModelState.IsValid)
            {
                Categorias.Estado = true;                
                await repository.CreateAsync(Categorias);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categorias = repository.GetCombo();
            return View(Categorias);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Categorias = await this.repository.GetByIdAsync(id);
            if (Categorias == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            ViewBag.Categorias = repository.GetCombo().Where(x => x.Value != id);
            return this.View(Categorias);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ParamCategorias Categorias)
        {
            if (id != Categorias.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await repository.UpdateAsync(Categorias);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(Categorias.Id))
                    {
                        return new NotFoundViewResult("NoExiste");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categorias = repository.GetCombo().Where(x => x.Value != Categorias.PadreId);
            return View(Categorias);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Categorias = await this.repository.GetByIdAsync(id);
            if (Categorias == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            Categorias.Estado = !Categorias.Estado;
            await repository.DeleteAsync(Categorias);
            return RedirectToAction(nameof(Index));
        }        

    }
}
