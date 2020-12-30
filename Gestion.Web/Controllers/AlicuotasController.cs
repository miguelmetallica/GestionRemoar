using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize(Roles = "Admin,Alicuotas")]    
    public class AlicuotasController : Controller
    {
        private readonly IAlicuotasRepository repository; 
        private readonly IUserHelper userHelper;

        public AlicuotasController(IAlicuotasRepository repository, IUserHelper userHelper)
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

            var Alicuotas = await this.repository.GetByIdAsync(id);
            if (Alicuotas == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(Alicuotas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParamAlicuotas Alicuotas)
        {
            if (ModelState.IsValid)
            {
                Alicuotas.Estado = true;
                await repository.CreateAsync(Alicuotas);
                return RedirectToAction(nameof(Index));
            }
            return View(Alicuotas);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Alicuotas = await this.repository.GetByIdAsync(id);
            if (Alicuotas == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(Alicuotas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ParamAlicuotas Alicuotas)
        {
            if (id != Alicuotas.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await repository.UpdateAsync(Alicuotas);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(Alicuotas.Id))
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
            return View(Alicuotas);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Alicuotas = await this.repository.GetByIdAsync(id);
            if (Alicuotas == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            Alicuotas.Estado = !Alicuotas.Estado;
            await repository.DeleteAsync(Alicuotas);
            return RedirectToAction(nameof(Index));
        }        

    }
}
