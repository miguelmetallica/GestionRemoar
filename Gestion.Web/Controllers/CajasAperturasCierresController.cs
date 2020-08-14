using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize]
    public class CajasAperturasCierresController : Controller
    {
        private readonly ICajasAperturasCierresRepository repository; 
        private readonly IUserHelper userHelper;
        private readonly ICajasRepository cajas;
        private readonly UserManager<Usuarios> userManager;

        public CajasAperturasCierresController(ICajasAperturasCierresRepository repository, IUserHelper userHelper, ICajasRepository cajas, UserManager<Usuarios> userManager)
        {
            this.repository = repository;
            this.userHelper = userHelper;
            this.cajas = cajas;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(repository.GetCajasAll());
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var CajasAperturasCierres = await this.repository.GetByIdAsync(id);
            if (CajasAperturasCierres == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(CajasAperturasCierres);
        }

        public async Task<IActionResult> Create()
        {
            var user = await userHelper.GetUserByEmailAsync(User.Identity.Name);            
            ViewBag.Cajas = this.cajas.GetCombo(user.SucursalId);
            var cajasAperturasCierres = new CajasAperturasCierres();
            cajasAperturasCierres.FechaApertura = DateTime.Today.Date;
            cajasAperturasCierres.UsuarioAperturaId = user.Id;
            cajasAperturasCierres.EfectivoApertura = 0;
            cajasAperturasCierres.DolaresApertura = 0;
            cajasAperturasCierres.CuponesApertura = 0;
            cajasAperturasCierres.ChequesApertura = 0;
            cajasAperturasCierres.OtrosApertura = 0;
            cajasAperturasCierres.ObservacionesApertura = "";
            
            cajasAperturasCierres.EfectivoCierre = 0;
            cajasAperturasCierres.DolaresCierre = 0;
            cajasAperturasCierres.CuponesCierre = 0;
            cajasAperturasCierres.ChequesCierre = 0;
            cajasAperturasCierres.OtrosCierre = 0;
            cajasAperturasCierres.ObservacionesCierre = "";
            
            cajasAperturasCierres.Estado = true;
            return View(cajasAperturasCierres);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CajasAperturasCierres CajasAperturasCierres)
        {
            if (ModelState.IsValid)
            {
                CajasAperturasCierres.Estado = true;
                CajasAperturasCierres.FechaAlta = DateTime.Now;                
                await repository.CreateAsync(CajasAperturasCierres);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Cajas = this.cajas.GetCombo("0b23156a-2e01-4c4c-a76e-a0bd4381e5ea");
            return View(CajasAperturasCierres);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var CajasAperturasCierres = await this.repository.GetByIdAsync(id);
            if (CajasAperturasCierres == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var user = await userHelper.GetUserByEmailAsync(User.Identity.Name);
            ViewBag.Cajas = this.cajas.GetCombo(user.SucursalId);
            CajasAperturasCierres.UsuarioCierreId = user.Id;

            return this.View(CajasAperturasCierres);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, CajasAperturasCierres CajasAperturasCierres)
        {
            if (id != CajasAperturasCierres.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    CajasAperturasCierres.Estado = true;
                    await repository.UpdateAsync(CajasAperturasCierres);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(CajasAperturasCierres.Id))
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
            
            ViewBag.Cajas = this.cajas.GetCombo("0b23156a-2e01-4c4c-a76e-a0bd4381e5ea");
            return View(CajasAperturasCierres);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var CajasAperturasCierres = await this.repository.GetByIdAsync(id);
            if (CajasAperturasCierres == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(CajasAperturasCierres);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var CajasAperturasCierres = await repository.GetByIdAsync(id);
            await repository.DeleteAsync(CajasAperturasCierres);
            return RedirectToAction(nameof(Index));
        }

    }
}
