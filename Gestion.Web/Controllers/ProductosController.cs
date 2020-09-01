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
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly IProductosRepository repository; 
        private readonly IUserHelper userHelper;
        private readonly ITiposProductosRepository tiposProductos;
        private readonly ICuentasComprasRepository cuentasCompras;
        private readonly ICuentasVentasRepository cuentasVentas;
        private readonly IUnidadesMedidasRepository unidadesMedidas;
        private readonly IAlicuotasRepository alicuotas;
        

        public ProductosController(IProductosRepository repository, 
                                IUserHelper userHelper, 
                                ITiposProductosRepository tiposProductos,
                                ICuentasComprasRepository cuentasCompras,
                                ICuentasVentasRepository cuentasVentas,
                                IUnidadesMedidasRepository unidadesMedidas,
                                IAlicuotasRepository alicuotas 
                                )
        {
            this.repository = repository;
            this.userHelper = userHelper;
            this.tiposProductos = tiposProductos;
            this.cuentasCompras = cuentasCompras;
            this.cuentasVentas = cuentasVentas;
            this.unidadesMedidas = unidadesMedidas;
            this.alicuotas = alicuotas;            
        }

        public async Task<IActionResult> Index()
        {
            return View(await repository.spProductosGet());
        }
        
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Productos = await this.repository.GetProducto(id);
            if (Productos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }
            
            return this.View(Productos);
        }

        public IActionResult Create()
        {
            ViewBag.TiposProductos = this.tiposProductos.GetCombo();
            ViewBag.CuentasCompras = this.cuentasCompras.GetCombo();
            ViewBag.CuentasVentas = this.cuentasVentas.GetCombo();
            ViewBag.UnidadesMedidas = this.unidadesMedidas.GetCombo();
            ViewBag.Alicuotas = this.alicuotas.GetCombo();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Productos productos)
        {
            if (ModelState.IsValid)
            {
                //var codigo = await repository.ExistCodigoAsync("", productos.Codigo);
                //if (codigo) 
                //{
                //    ModelState.AddModelError("Codigo", "El Codigo ya existe en la base de datos");                
                //}

                if (ModelState.IsValid)
                {
                    productos.Id = Guid.NewGuid().ToString();
                    productos.Estado = true;
                    productos.FechaAlta = DateTime.Now;
                    productos.UsuarioAlta = User.Identity.Name;
                    await repository.spInsertar(productos);
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.TiposProductos = this.tiposProductos.GetCombo();
            ViewBag.CuentasCompras = this.cuentasCompras.GetCombo();
            ViewBag.CuentasVentas = this.cuentasVentas.GetCombo();
            ViewBag.UnidadesMedidas = this.unidadesMedidas.GetCombo();
            ViewBag.Alicuotas = this.alicuotas.GetCombo();


            return View(productos);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var productos = await this.repository.GetByIdAsync(id);
            if (productos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            ViewBag.TiposProductos = this.tiposProductos.GetCombo();
            ViewBag.CuentasCompras = this.cuentasCompras.GetCombo();
            ViewBag.CuentasVentas = this.cuentasVentas.GetCombo();
            ViewBag.UnidadesMedidas = this.unidadesMedidas.GetCombo();
            ViewBag.Alicuotas = this.alicuotas.GetCombo();

            return this.View(productos);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Productos productos)
        {
            if (id != productos.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //var codigo = await repository.ExistCodigoAsync(id, productos.Codigo);
                    //if (codigo)
                    //{
                    //    ModelState.AddModelError("Codigo", "El Codigo ya existe en la base de datos");
                    //}

                    if (ModelState.IsValid)
                    {
                        productos.Estado = true;
                        productos.FechaAlta = DateTime.Now;
                        productos.UsuarioAlta = User.Identity.Name;

                        await repository.spEditar(productos);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(productos.Id))
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

            ViewBag.TiposProductos = this.tiposProductos.GetCombo();
            ViewBag.CuentasCompras = this.cuentasCompras.GetCombo();
            ViewBag.CuentasVentas = this.cuentasVentas.GetCombo();
            ViewBag.UnidadesMedidas = this.unidadesMedidas.GetCombo();
            ViewBag.Alicuotas = this.alicuotas.GetCombo();

            return View(productos);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Productos = await this.repository.GetByIdAsync(id);
            if (Productos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(Productos);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var Productos = await repository.GetByIdAsync(id);
            await repository.DeleteAsync(Productos);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult NoExiste()
        {
            return this.View();
        }

    }
}

