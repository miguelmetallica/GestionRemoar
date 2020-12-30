using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize(Roles = "Admin,ComprobantesNumeraciones")]
    
    public class ComprobantesNumeracionesController : Controller
    {
        private readonly IComprobantesNumeracionesRepository repository; 
        private readonly IUserHelper userHelper;
        private readonly ISucursalesRepository sucursalesRepository;
        private readonly ITiposComprobantesRepository tiposComprobantesRepository;

        public ComprobantesNumeracionesController(IComprobantesNumeracionesRepository repository, IUserHelper userHelper, ISucursalesRepository sucursalesRepository,ITiposComprobantesRepository tiposComprobantesRepository)
        {
            this.repository = repository;
            this.userHelper = userHelper;
            this.sucursalesRepository = sucursalesRepository;
            this.tiposComprobantesRepository = tiposComprobantesRepository;
        }

        public async Task<IActionResult> Index()
        {
            var comprobantesNumeraciones = await repository.GetTodos();
            return View(comprobantesNumeraciones);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var ComprobantesNumeraciones = await this.repository.GetByIdAsync(id);
            if (ComprobantesNumeraciones == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(ComprobantesNumeraciones);
        }

        public IActionResult Create()
        {
            ViewBag.Sucursales = sucursalesRepository.GetCombo();
            ViewBag.TiposComprobantes = tiposComprobantesRepository.GetCombo();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ComprobantesNumeraciones ComprobantesNumeraciones)
        {
            if (ModelState.IsValid)
            {
                ComprobantesNumeraciones.Estado = true;
                await repository.CreateAsync(ComprobantesNumeraciones);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Sucursales = sucursalesRepository.GetCombo();
            ViewBag.TiposComprobantes = tiposComprobantesRepository.GetCombo();
            return View(ComprobantesNumeraciones);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var ComprobantesNumeraciones = await this.repository.GetByIdAsync(id);
            if (ComprobantesNumeraciones == null)
            {
                return new NotFoundViewResult("NoExiste");
            }
            ViewBag.Sucursales = sucursalesRepository.GetCombo();
            ViewBag.TiposComprobantes = tiposComprobantesRepository.GetCombo();

            return this.View(ComprobantesNumeraciones);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ComprobantesNumeraciones ComprobantesNumeraciones)
        {
            if (id != ComprobantesNumeraciones.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await repository.UpdateAsync(ComprobantesNumeraciones);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(ComprobantesNumeraciones.Id))
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

            ViewBag.Sucursales = sucursalesRepository.GetCombo();
            ViewBag.TiposComprobantes = tiposComprobantesRepository.GetCombo();

            return View(ComprobantesNumeraciones);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var ComprobantesNumeraciones = await this.repository.GetByIdAsync(id);
            if (ComprobantesNumeraciones == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            ComprobantesNumeraciones.Estado = !ComprobantesNumeraciones.Estado;
            await repository.DeleteAsync(ComprobantesNumeraciones);
            return RedirectToAction(nameof(Index));
        }        

    }
}
