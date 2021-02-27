using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize(Roles = "Admin,FormasPagosCotizacion")]
    public class FormasPagosCotizacionController : Controller
    {
        private readonly IFormasPagosCotizacionRepository repository; 
        private readonly IUserHelper userHelper;
        private readonly IFormasPagosCotizacionRepository cuotasRepository;
        private readonly IEntidadesRepository entidadesRepository;

        public FormasPagosCotizacionController(IFormasPagosCotizacionRepository repository, 
            IUserHelper userHelper,
            IFormasPagosCotizacionRepository cuotasRepository,
            IEntidadesRepository entidadesRepository
            )
        {
            this.repository = repository;
            this.userHelper = userHelper;
            this.cuotasRepository = cuotasRepository;
            this.entidadesRepository = entidadesRepository;
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

            var FormasPagosCotizacion = await this.repository.GetByIdAsync(id);
            if (FormasPagosCotizacion == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(FormasPagosCotizacion);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FormasPagosCotizacion FormasPagosCotizacion)
        {
            if (ModelState.IsValid)
            {
                FormasPagosCotizacion.Estado = true;
                await repository.CreateAsync(FormasPagosCotizacion);
                return RedirectToAction(nameof(Index));
            }

            return View(FormasPagosCotizacion);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var FormasPagosCotizacion = await this.repository.GetByIdAsync(id);
            if (FormasPagosCotizacion == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(FormasPagosCotizacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, FormasPagosCotizacion FormasPagosCotizacion)
        {
            if (id != FormasPagosCotizacion.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await repository.UpdateAsync(FormasPagosCotizacion);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(FormasPagosCotizacion.Id))
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

            return View(FormasPagosCotizacion);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var FormasPagosCotizacion = await this.repository.GetByIdAsync(id);
            if (FormasPagosCotizacion == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            FormasPagosCotizacion.Estado = !FormasPagosCotizacion.Estado;
            await repository.DeleteAsync(FormasPagosCotizacion);
            return RedirectToAction(nameof(Index));
        }

    }
}
