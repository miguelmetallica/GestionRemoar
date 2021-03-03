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
    [Authorize(Roles = "Admin,FormasPagos")]
    public class FormasPagosController : Controller
    {
        private readonly IFormasPagosRepository repository; 
        private readonly IUserHelper userHelper;
        private readonly IFormasPagosCuotasRepository cuotasRepository;
        private readonly IEntidadesRepository entidadesRepository;

        public FormasPagosController(IFormasPagosRepository repository, 
            IUserHelper userHelper,
            IFormasPagosCuotasRepository cuotasRepository,
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

            var FormasPagos = await this.repository.GetByIdAsync(id);
            if (FormasPagos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(FormasPagos);
        }

        public IActionResult Create()
        {
            ViewBag.Tipos = repository.GetTipos();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FormasPagos FormasPagos)
        {
            if (ModelState.IsValid)
            {
                FormasPagos.Estado = true;
                FormasPagos.Codigo = FormasPagos.Codigo.ToUpper();
                FormasPagos.Descripcion = FormasPagos.Descripcion.ToUpper();
                await repository.CreateAsync(FormasPagos);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Tipos = repository.GetTipos();

            return View(FormasPagos);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var FormasPagos = await this.repository.GetByIdAsync(id);
            if (FormasPagos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            ViewBag.Tipos = repository.GetTipos();
            return this.View(FormasPagos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, FormasPagos FormasPagos)
        {
            if (id != FormasPagos.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    FormasPagos.Codigo = FormasPagos.Codigo.ToUpper();
                    FormasPagos.Descripcion = FormasPagos.Descripcion.ToUpper();
                    
                    await repository.UpdateAsync(FormasPagos);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(FormasPagos.Id))
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

            ViewBag.Tipos = repository.GetTipos();
            return View(FormasPagos);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var FormasPagos = await this.repository.GetByIdAsync(id);
            if (FormasPagos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            FormasPagos.Estado = !FormasPagos.Estado;
            await repository.DeleteAsync(FormasPagos);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Cuotas(string id)
        {
            ViewData["Entidades"] = entidadesRepository.GetAll();
            ViewBag.FormaPagoId = id;
            return View(cuotasRepository.GetAll(id));
        }

        public IActionResult CreateCuota(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            ViewBag.Entidades = entidadesRepository.GetCombo();

            var formasPagosCuotas = new FormasPagosCuotas();
            formasPagosCuotas.FormaPagoId = id;
            return View(formasPagosCuotas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCuota(FormasPagosCuotas formasPagosCuotas)
        {
            if (formasPagosCuotas.FechaDesde == null)             
            {
                formasPagosCuotas.FechaDesde = Convert.ToDateTime("01/01/2020");
            }

            if (formasPagosCuotas.FechaHasta == null)
            {
                formasPagosCuotas.FechaHasta = Convert.ToDateTime("01/01/2099");
            }

            if (ModelState.IsValid)
            {

                formasPagosCuotas.Id = Guid.NewGuid().ToString();
                formasPagosCuotas.Descripcion = formasPagosCuotas.Descripcion.ToUpper(); 
                formasPagosCuotas.Estado = true;
                await cuotasRepository.CreateAsync(formasPagosCuotas);
                return RedirectToAction(nameof(Cuotas) , new { id = formasPagosCuotas.FormaPagoId });
            }

            ViewBag.Entidades = entidadesRepository.GetCombo();

            return View(formasPagosCuotas);
        }

        public async Task<IActionResult> EditCuota(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var cuotas  = await this.cuotasRepository.GetByIdAsync(id);
            if (cuotas == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            ViewBag.Entidades = entidadesRepository.GetCombo();
            return this.View(cuotas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCuota(string id, FormasPagosCuotas formasPagosCuotas)
        {
            if (id != formasPagosCuotas.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }
            
            if (formasPagosCuotas.FechaDesde == null)
            {
                formasPagosCuotas.FechaDesde = Convert.ToDateTime("01/01/2020");
            }

            if (formasPagosCuotas.FechaHasta == null)
            {
                formasPagosCuotas.FechaHasta = Convert.ToDateTime("01/01/2099");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    formasPagosCuotas.Descripcion = formasPagosCuotas.Descripcion.ToUpper();
                    await cuotasRepository.UpdateAsync(formasPagosCuotas);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await cuotasRepository.ExistAsync(formasPagosCuotas.Id))
                    {
                        return new NotFoundViewResult("NoExiste");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Cuotas), new { id = formasPagosCuotas.FormaPagoId });
            }

            ViewBag.Entidades = entidadesRepository.GetCombo();
            return View(formasPagosCuotas);
        }

        public async Task<IActionResult> DeleteCuota(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var cuotas = await this.cuotasRepository.GetByIdAsync(id);
            if (cuotas == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            cuotas.Estado = !cuotas.Estado;
            await cuotasRepository.DeleteAsync(cuotas);
            return RedirectToAction(nameof(Cuotas), new { id = cuotas.FormaPagoId });
        }

        public async Task<IActionResult> DetailsCuota(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var cuotas = await this.cuotasRepository.GetByIdAsync(id);
            if (cuotas == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            ViewData["Entidades"] = entidadesRepository.GetAll();
            return this.View(cuotas);
        }
        [AllowAnonymous]
        public JsonResult GetCuotas(FormasPagosCuotas formasPagosCuotas)
        {
            var cuotas = cuotasRepository.GetCuotas(formasPagosCuotas.FormaPagoId,formasPagosCuotas.EntidadId);
            return Json(cuotas);
        }
        [AllowAnonymous]
        public JsonResult GetCuotasInteres(FormasPagosCuotas formasPagosCuotas)
        {
            var cuotas = cuotasRepository.GetCuotasInteres(formasPagosCuotas.FormaPagoId, formasPagosCuotas.EntidadId, formasPagosCuotas.Cuota);
            return Json(cuotas);
        }

    }
}
