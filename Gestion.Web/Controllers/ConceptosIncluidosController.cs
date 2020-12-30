using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize(Roles = "Admin,ConceptosIncluidos")]
    
    public class ConceptosIncluidosController : Controller
    {
        private readonly IConceptosIncluidosRepository repository; 
        private readonly IUserHelper userHelper;

        public ConceptosIncluidosController(IConceptosIncluidosRepository repository, IUserHelper userHelper)
        {
            this.repository = repository;
            this.userHelper = userHelper;
        }

        public IActionResult Index()
        {
            return View(repository.GetAll());
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var ConceptosIncluidos = await this.repository.GetByIdAsync(id);
            if (ConceptosIncluidos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(ConceptosIncluidos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParamConceptosIncluidos ConceptosIncluidos)
        {
            if (ModelState.IsValid)
            {
                ConceptosIncluidos.Estado = true;
                await repository.CreateAsync(ConceptosIncluidos);
                return RedirectToAction(nameof(Index));
            }
            return View(ConceptosIncluidos);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var ConceptosIncluidos = await this.repository.GetByIdAsync(id);
            if (ConceptosIncluidos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(ConceptosIncluidos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ParamConceptosIncluidos ConceptosIncluidos)
        {
            if (id != ConceptosIncluidos.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await repository.UpdateAsync(ConceptosIncluidos);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(ConceptosIncluidos.Id))
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
            return View(ConceptosIncluidos);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var ConceptosIncluidos = await this.repository.GetByIdAsync(id);
            if (ConceptosIncluidos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            ConceptosIncluidos.Estado = !ConceptosIncluidos.Estado;
            await repository.DeleteAsync(ConceptosIncluidos);
            return RedirectToAction(nameof(Index));
        }        

    }
}
