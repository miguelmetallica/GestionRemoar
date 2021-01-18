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
    [Authorize(Roles = "Admin,Cajas")]
    
    public class CajasController : Controller
    {
        private readonly ICajasRepository repository;
        private readonly IUserHelper userHelper;
        private readonly ISucursalesRepository sucursales;

        public CajasController(ICajasRepository repository, IUserHelper userHelper, ISucursalesRepository sucursales)
        {
            this.repository = repository;
            this.userHelper = userHelper;
            this.sucursales = sucursales;
        }

        public IActionResult Index()
        {
            var model = repository.GetAll().Include(x => x.Sucursal);
            return View(model);
        }
        public async Task<IActionResult> Estado()
        {
            try
            {
                var model = await repository.spCajasEstadoFechaGet();
                return View(model);
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

        public async Task<IActionResult> InformeCaja()
        {
            try
            {
                var model = await repository.spCajasEstadoFechaGet();
                return View(model);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public async Task<IActionResult> DetailsImportes(string fecha,string sucid)
        {
            var model = await repository.spCajasEstadoImportesGet(fecha,sucid);
            return View(model);
        }

        public async Task<IActionResult> DetailsUsuarios(string fecha, string sucid)
        {
            var model = await repository.spCajasEstadoUsuariosGet(fecha, sucid);
            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Cajas = await this.repository.GetByIdAsync(id);
            if (Cajas == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(Cajas);
        }

        public IActionResult Create()
        {
            ViewBag.Sucursales = this.sucursales.GetCombo();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParamCajas Cajas)
        {
            if (ModelState.IsValid)
            {
                Cajas.Estado = true;
                Cajas.FechaAlta = DateTime.Now;
                Cajas.UsuarioAlta = User.Identity.Name;

                await repository.CreateAsync(Cajas);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Sucursales = this.sucursales.GetCombo();

            return View(Cajas);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Cajas = await this.repository.GetByIdAsync(id);
            if (Cajas == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            ViewBag.Sucursales = this.sucursales.GetCombo();

            return this.View(Cajas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ParamCajas Cajas)
        {
            if (id != Cajas.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Cajas.Estado = true;
                    await repository.UpdateAsync(Cajas);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(Cajas.Id))
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

            ViewBag.Sucursales = this.sucursales.GetCombo();
            return View(Cajas);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Cajas = await repository.GetByIdAsync(id);
            Cajas.Estado = !Cajas.Estado;
            await repository.UpdateAsync(Cajas);
            return RedirectToAction(nameof(Index));
        }        

    }
}
