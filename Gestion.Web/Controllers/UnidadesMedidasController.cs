using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize]
    public class UnidadesMedidasController : Controller
    {
        private readonly IUnidadesMedidasRepository repository; 
        private readonly IUserHelper userHelper;

        public UnidadesMedidasController(IUnidadesMedidasRepository repository, IUserHelper userHelper)
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

            var UnidadesMedidas = await this.repository.GetByIdAsync(id);
            if (UnidadesMedidas == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(UnidadesMedidas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParamUnidadesMedidas UnidadesMedidas)
        {
            if (ModelState.IsValid)
            {
                UnidadesMedidas.Estado = true;
                await repository.CreateAsync(UnidadesMedidas);
                return RedirectToAction(nameof(Index));
            }
            return View(UnidadesMedidas);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var UnidadesMedidas = await this.repository.GetByIdAsync(id);
            if (UnidadesMedidas == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(UnidadesMedidas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ParamUnidadesMedidas UnidadesMedidas)
        {
            if (id != UnidadesMedidas.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await repository.UpdateAsync(UnidadesMedidas);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(UnidadesMedidas.Id))
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
            return View(UnidadesMedidas);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var UnidadesMedidas = await this.repository.GetByIdAsync(id);
            if (UnidadesMedidas == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            UnidadesMedidas.Estado = !UnidadesMedidas.Estado;
            await repository.DeleteAsync(UnidadesMedidas);
            return RedirectToAction(nameof(Index));
        }        

    }
}
