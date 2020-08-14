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
    public class ProvinciasController : Controller
    {
        private readonly IProvinciasRepository repository;
        private readonly IUserHelper userHelper;

        public ProvinciasController(IProvinciasRepository repository, IUserHelper userHelper)
        {
            this.repository = repository;
            this.userHelper = userHelper;
        }

        //public async Task<IActionResult> DeleteLocalidad(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    //var localidad = await this.repository.GetLocalidadesAsync(id.Value);
        //    //if (localidad == null)
        //    //{
        //    //    return NotFound();
        //    //}

        //    //var provinciaId = await this.repository.DeleteLocalidadAsync(localidad);

        //    //return this.RedirectToAction($"Details/{provinciaId}");
        //    return this.RedirectToAction($"Index");
        //}

        //public async Task<IActionResult> EditLocalidad(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var localidades = await this.repository.GetLocalidadesAsync(id.Value);
        //    if (localidades == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(localidades);
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> EditLocalidad(Localidades localidad)
        //{
        //    if (this.ModelState.IsValid)
        //    {
        //        var ProvinciaId = await this.repository.UpdateLocalidadAsync(localidad);
        //        if (ProvinciaId != 0)
        //        {
        //            return this.RedirectToAction($"Details/{ProvinciaId}");
        //        }
        //    }

        //    return this.View(localidad);
        //}

        //public async Task<IActionResult> AddLocalidad(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var country = await this.repository.GetByIdAsync(id);
        //    if (country == null)
        //    {
        //        return NotFound();
        //    }

        //    var model = new LocalidadesViewModel { ProvinciaId = country.Id };
        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> AddLocalidad(LocalidadesViewModel model)
        //{
        //    if (this.ModelState.IsValid)
        //    {
        //        await this.repository.AddLocalidadesAsync(model);
        //        return this.RedirectToAction($"Details/{model.ProvinciaId}");
        //    }

        //    return this.View(model);
        //}

        public IActionResult Index()
        {
            return View();// this.repository.GetProvinciasWithLocalidades());
        }

        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    //var provincias = await this.repository.GetProvinciasWithLocalidadesAsync(id.Value);
        //    //if (provincias == null)
        //    //{
        //    //    return NotFound();
        //    //}

        //    //return View(provincias);
        //    return View();
        //}

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParamProvincias provincias)
        {
            if (ModelState.IsValid)
            {
                await this.repository.CreateAsync(provincias);
                return RedirectToAction(nameof(Index));
            }

            return View(provincias);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var provincias = await this.repository.GetByIdAsync(id);
            if (provincias == null)
            {
                return NotFound();
            }
            return View(provincias);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ParamProvincias provincias)
        {
            if (ModelState.IsValid)
            {
                await this.repository.UpdateAsync(provincias);
                return RedirectToAction(nameof(Index));
            }

            return View(provincias);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var provincias = await this.repository.GetByIdAsync(id);
            if (provincias == null)
            {
                return NotFound();
            }

            await this.repository.DeleteAsync(provincias);
            return RedirectToAction(nameof(Index));
        }


    }

}
