using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize(Roles = "Admin,TiposDocumentos")]
    public class TiposDocumentosController : Controller
    {
        private readonly ITiposDocumentosRepository repository; 
        private readonly IUserHelper userHelper;

        public TiposDocumentosController(ITiposDocumentosRepository repository, IUserHelper userHelper)
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

            var TiposDocumentos = await this.repository.GetByIdAsync(id);
            if (TiposDocumentos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(TiposDocumentos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParamTiposDocumentos TiposDocumentos)
        {
            if (ModelState.IsValid)
            {
                TiposDocumentos.Estado = true;
                await repository.CreateAsync(TiposDocumentos);
                return RedirectToAction(nameof(Index));
            }
            return View(TiposDocumentos);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var TiposDocumentos = await this.repository.GetByIdAsync(id);
            if (TiposDocumentos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(TiposDocumentos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ParamTiposDocumentos TiposDocumentos)
        {
            if (id != TiposDocumentos.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await repository.UpdateAsync(TiposDocumentos);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(TiposDocumentos.Id))
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
            return View(TiposDocumentos);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var TiposDocumentos = await this.repository.GetByIdAsync(id);
            if (TiposDocumentos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            TiposDocumentos.Estado = !TiposDocumentos.Estado;
            await repository.DeleteAsync(TiposDocumentos);
            return RedirectToAction(nameof(Index));
        }        

    }
}
