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
    
    public class CajasMovimientosController : Controller
    {
        private readonly ICajasMovimientosRepository repository; 
        private readonly IUserHelper userHelper;
        private readonly ICajasRepository cajasRepository;
        private readonly ICajasTiposMovimientosRepository cajasTiposMovimientosRepository;

        public CajasMovimientosController(ICajasMovimientosRepository repository, 
                                        IUserHelper userHelper,
                                        ICajasRepository cajasRepository,
                                        ICajasTiposMovimientosRepository cajasTiposMovimientosRepository
            )
        {
            this.repository = repository;
            this.userHelper = userHelper;
            this.cajasRepository = cajasRepository;
            this.cajasTiposMovimientosRepository = cajasTiposMovimientosRepository;
        }

        [Authorize(Roles = "Admin,CajasMovimientos,CajasMovimientosAdministra")]
        public IActionResult Index()
        {
            var model = repository.GetAll()
                    .Include(x => x.Caja)
                    .Include(x => x.Caja.Sucursal)
                    .Include(x => x.TipoMovimiento);
            return View(model);
        }

        [Authorize(Roles = "Admin,CajasMovimientos,CajasMovimientosAdministra")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var CajasMovimientos = await this.repository.GetByIdAsync(id);            
            if (CajasMovimientos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }
                                            
            return this.View(CajasMovimientos);
        }

        [Authorize(Roles = "Admin,CajasMovimientos")]
        public async Task<IActionResult> Create()
        {
            var user = await userHelper.GetUserByEmailAsync(User.Identity.Name);
            ViewBag.Cajas = this.cajasRepository.GetCombo(user.SucursalId);
            ViewBag.TiposMovimientos = this.cajasTiposMovimientosRepository.GetCombo();
            
            var CajasMovimientos = new CajasMovimientos();
            CajasMovimientos.Fecha = DateTime.Today;
            return View(CajasMovimientos);
        }

        [Authorize(Roles = "Admin,CajasMovimientos")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CajasMovimientos CajasMovimientos)
        {
            CajasMovimientos.Fecha = DateTime.Today;
            
            if (ModelState.IsValid)
            {
                var tipoMov = await cajasTiposMovimientosRepository.GetByIdAsync(CajasMovimientos.TipoMovimientoId);

                CajasMovimientos.FechaAlta = DateTime.Now;
                CajasMovimientos.UsuarioAlta = User.Identity.Name;
                if (tipoMov.EsDebe)
                {
                    CajasMovimientos.Importe = -1 * Math.Abs(CajasMovimientos.Importe);
                }
                else 
                {
                    CajasMovimientos.Importe = Math.Abs(CajasMovimientos.Importe);
                }
                
                await repository.CreateAsync(CajasMovimientos);
                return RedirectToAction(nameof(Index));
            }

            var user = await userHelper.GetUserByEmailAsync(User.Identity.Name);
            ViewBag.Cajas = this.cajasRepository.GetCombo(user.SucursalId);
            ViewBag.TiposMovimientos = this.cajasTiposMovimientosRepository.GetCombo();

            return View(CajasMovimientos);
        }

        [Authorize(Roles = "Admin,CajasMovimientos")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var CajasMovimientos = await this.repository.GetByIdAsync(id);
            if (CajasMovimientos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var user = await userHelper.GetUserByEmailAsync(User.Identity.Name);
            ViewBag.Cajas = this.cajasRepository.GetCombo(user.SucursalId);
            ViewBag.TiposMovimientos = this.cajasTiposMovimientosRepository.GetCombo();

            return this.View(CajasMovimientos);
        }

        [Authorize(Roles = "Admin,CajasMovimientos")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, CajasMovimientos CajasMovimientos)
        {
            if (id != CajasMovimientos.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                   // await repository.UpdateAsync(CajasMovimientos);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(CajasMovimientos.Id))
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

            var user = await userHelper.GetUserByEmailAsync(User.Identity.Name);
            ViewBag.Cajas = this.cajasRepository.GetCombo(user.SucursalId);
            ViewBag.TiposMovimientos = this.cajasTiposMovimientosRepository.GetCombo();

            return View(CajasMovimientos);
        }

        [Authorize(Roles = "Admin,CajasMovimientosAdministra")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var CajasMovimientos = await this.repository.GetByIdAsync(id);
            if (CajasMovimientos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(CajasMovimientos);
        }

        [Authorize(Roles = "Admin,CajasMovimientos,CajasMovimientosAdministra")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var CajasMovimientos = await repository.GetByIdAsync(id);
            await repository.DeleteAsync(CajasMovimientos);
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Admin,CajasMovimientosAdministra")]
        public async Task<IActionResult> CreateAdministra()
        {
            var user = await userHelper.GetUserByEmailAsync(User.Identity.Name);
            ViewBag.Cajas = this.cajasRepository.GetCombo(user.SucursalId);
            ViewBag.TiposMovimientos = this.cajasTiposMovimientosRepository.GetCombo();

            var cajasMovimientos = new CajasMovimientosAdministra();
            cajasMovimientos.Fecha = DateTime.Today;
            return View(cajasMovimientos);
        }

        [Authorize(Roles = "Admin,CajasMovimientosAdministra")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdministra(CajasMovimientosAdministra CajasMovimientos)
        {
            if (ModelState.IsValid)
            {
                var tipoMov = await cajasTiposMovimientosRepository.GetByIdAsync(CajasMovimientos.TipoMovimientoId);
                if (tipoMov.EsDebe)
                {
                    CajasMovimientos.Importe = -1 * Math.Abs(CajasMovimientos.Importe);
                }
                else
                {
                    CajasMovimientos.Importe = Math.Abs(CajasMovimientos.Importe);
                }

                var CajaAdmin = new CajasMovimientos();

                CajaAdmin.CajaId = CajasMovimientos.CajaId;
                CajaAdmin.Fecha = CajasMovimientos.Fecha;
                CajaAdmin.TipoMovimientoId = CajasMovimientos.TipoMovimientoId;
                CajaAdmin.Importe = CajasMovimientos.Importe;
                CajaAdmin.Observaciones = CajasMovimientos.Observaciones;
                CajaAdmin.FechaAlta = DateTime.Now;
                CajaAdmin.UsuarioAlta = User.Identity.Name;

                await repository.CreateAsync(CajaAdmin);
                return RedirectToAction(nameof(Index));
            }

            var user = await userHelper.GetUserByEmailAsync(User.Identity.Name);
            ViewBag.Cajas = this.cajasRepository.GetCombo(user.SucursalId);
            ViewBag.TiposMovimientos = this.cajasTiposMovimientosRepository.GetCombo();

            return View(CajasMovimientos);
        }

        [Authorize(Roles = "Admin,CajasMovimientosAdministra")]
        public async Task<IActionResult> EditAdministra(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var CajasMovimientos = await this.repository.GetByIdAsync(id);
            if (CajasMovimientos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var user = await userHelper.GetUserByEmailAsync(User.Identity.Name);
            ViewBag.Cajas = this.cajasRepository.GetCombo(user.SucursalId);
            ViewBag.TiposMovimientos = this.cajasTiposMovimientosRepository.GetCombo();


            var CajaAdmin = new CajasMovimientosAdministra();
            
            CajaAdmin.CajaId = CajasMovimientos.CajaId;
            CajaAdmin.Fecha = CajasMovimientos.Fecha;
            CajaAdmin.TipoMovimientoId = CajasMovimientos.TipoMovimientoId;
            CajaAdmin.Importe = CajasMovimientos.Importe;
            CajaAdmin.Observaciones = CajasMovimientos.Observaciones;
            CajaAdmin.FechaAlta = CajasMovimientos.FechaAlta;
            CajaAdmin.UsuarioAlta = CajasMovimientos.UsuarioAlta;

            return this.View(CajaAdmin);
        }

        [Authorize(Roles = "Admin,CajasMovimientosAdministra")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdministra(string id, CajasMovimientosAdministra CajasMovimientos)
        {
            if (id != CajasMovimientos.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var tipoMov = await cajasTiposMovimientosRepository.GetByIdAsync(CajasMovimientos.TipoMovimientoId);
                    if (tipoMov.EsDebe)
                    {
                        CajasMovimientos.Importe = -1 * Math.Abs(CajasMovimientos.Importe);
                    }
                    else
                    {
                        CajasMovimientos.Importe = Math.Abs(CajasMovimientos.Importe);
                    }

                    var CajaAdmin = new CajasMovimientos();

                    CajaAdmin.CajaId = CajasMovimientos.CajaId;
                    CajaAdmin.Fecha = CajasMovimientos.Fecha;
                    CajaAdmin.TipoMovimientoId = CajasMovimientos.TipoMovimientoId;
                    CajaAdmin.Importe = CajasMovimientos.Importe;
                    CajaAdmin.Observaciones = CajasMovimientos.Observaciones;
                    CajaAdmin.FechaAlta = CajasMovimientos.FechaAlta;
                    CajaAdmin.UsuarioAlta = CajasMovimientos.UsuarioAlta;

                    await repository.UpdateAsync(CajaAdmin);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(CajasMovimientos.Id))
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

            var user = await userHelper.GetUserByEmailAsync(User.Identity.Name);
            ViewBag.Cajas = this.cajasRepository.GetCombo(user.SucursalId);
            ViewBag.TiposMovimientos = this.cajasTiposMovimientosRepository.GetCombo();

            return View(CajasMovimientos);
        }

    }
}
