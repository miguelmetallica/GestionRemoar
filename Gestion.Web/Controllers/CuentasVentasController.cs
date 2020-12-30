using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize(Roles = "Admin,CuentasVentas")]
    
    public class CuentasVentasController : Controller
    {
        private readonly ICuentasVentasRepository repository; 
        private readonly IUserHelper userHelper;

        public CuentasVentasController(ICuentasVentasRepository repository, IUserHelper userHelper)
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

            var CuentasVentas = await this.repository.GetByIdAsync(id);
            if (CuentasVentas == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(CuentasVentas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParamCuentasVentas CuentasVentas)
        {
            if (ModelState.IsValid)
            {
                CuentasVentas.Estado = true;
                await repository.CreateAsync(CuentasVentas);
                return RedirectToAction(nameof(Index));
            }
            return View(CuentasVentas);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var CuentasVentas = await this.repository.GetByIdAsync(id);
            if (CuentasVentas == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(CuentasVentas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ParamCuentasVentas CuentasVentas)
        {
            if (id != CuentasVentas.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await repository.UpdateAsync(CuentasVentas);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(CuentasVentas.Id))
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
            return View(CuentasVentas);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var CuentasVentas = await this.repository.GetByIdAsync(id);
            if (CuentasVentas == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            CuentasVentas.Estado = !CuentasVentas.Estado;
            await repository.DeleteAsync(CuentasVentas);
            return RedirectToAction(nameof(Index));
        }        

    }
}
