using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize(Roles = "Admin,Productos")]
    
    public class ProductosController : Controller
    {
        private readonly IProductosRepository repository; 
        private readonly IUserHelper userHelper;
        private readonly ITiposProductosRepository tiposProductos;
        private readonly ICuentasComprasRepository cuentasCompras;
        private readonly ICuentasVentasRepository cuentasVentas;
        private readonly IUnidadesMedidasRepository unidadesMedidas;
        private readonly IAlicuotasRepository alicuotas;
        private readonly ICategoriasRepository categorias;
        private readonly IProductosCategoriasRepository productosCategorias;
        private readonly IProductosImagenesRepository productosImagenes;
        private readonly IProveedoresRepository proveedores;

        public ProductosController(IProductosRepository repository, 
                                IUserHelper userHelper, 
                                ITiposProductosRepository tiposProductos,
                                ICuentasComprasRepository cuentasCompras,
                                ICuentasVentasRepository cuentasVentas,
                                IUnidadesMedidasRepository unidadesMedidas,
                                IAlicuotasRepository alicuotas,
                                ICategoriasRepository categorias,
                                IProductosCategoriasRepository productosCategorias,
                                IProductosImagenesRepository productosImagenes,
                                IProveedoresRepository proveedores
                                )
        {
            this.repository = repository;
            this.userHelper = userHelper;
            this.tiposProductos = tiposProductos;
            this.cuentasCompras = cuentasCompras;
            this.cuentasVentas = cuentasVentas;
            this.unidadesMedidas = unidadesMedidas;
            this.alicuotas = alicuotas;
            this.categorias = categorias;
            this.productosCategorias = productosCategorias;
            this.productosImagenes = productosImagenes;
            this.proveedores = proveedores;
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

            var Productos = await this.repository.spProductosOne(id);
            if (Productos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            ViewBag.ProductosCategorias = await productosCategorias.spProductosCategoriasGet(id);

            ViewBag.ProductosImagenes = await productosImagenes.GetImagenes(id);

            return this.View(Productos);
        }

        public async Task<IActionResult> Copiar(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Productos = await this.repository.spProductosOne(id);
            if (Productos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            ViewBag.TiposProductos = this.tiposProductos.GetCombo();
            ViewBag.CuentasCompras = this.cuentasCompras.GetCombo();
            ViewBag.CuentasVentas = this.cuentasVentas.GetCombo();
            ViewBag.UnidadesMedidas = this.unidadesMedidas.GetCombo();
            ViewBag.Alicuotas = this.alicuotas.GetCombo();
            ViewBag.Proveedores = this.proveedores.GetCombo();
            ViewBag.Categorias = this.categorias.GetCombo();
            return this.View(Productos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Copiar(ProductosDTO productos)
        {
            if (ModelState.IsValid)
            {
                productos.Id = Guid.NewGuid().ToString();
                productos.Estado = true;
                productos.FechaAlta = DateTime.Now;
                productos.UsuarioAlta = User.Identity.Name;
                await repository.spInsertar(productos);
                return RedirectToAction(nameof(Details), new { id = productos.Id });
            }
            
            ViewBag.TiposProductos = this.tiposProductos.GetCombo();
            ViewBag.CuentasCompras = this.cuentasCompras.GetCombo();
            ViewBag.CuentasVentas = this.cuentasVentas.GetCombo();
            ViewBag.UnidadesMedidas = this.unidadesMedidas.GetCombo();
            ViewBag.Alicuotas = this.alicuotas.GetCombo();
            ViewBag.Proveedores = this.proveedores.GetCombo();
            ViewBag.Categorias = this.categorias.GetCombo();
            
            return View(productos);
        }



        public IActionResult Create()
        {
            ViewBag.TiposProductos = this.tiposProductos.GetCombo();
            ViewBag.CuentasCompras = this.cuentasCompras.GetCombo();
            ViewBag.CuentasVentas = this.cuentasVentas.GetCombo();
            ViewBag.UnidadesMedidas = this.unidadesMedidas.GetCombo();
            ViewBag.Alicuotas = this.alicuotas.GetCombo();
            ViewBag.Proveedores = this.proveedores.GetCombo();
            ViewBag.Categorias = this.categorias.GetCombo();

            var producto = new ProductosDTO();
            producto.RebajaDesde = DateTime.Parse("01/01/2000");
            producto.RebajaHasta = DateTime.Parse("01/01/2100");
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductosDTO productos)
        {
            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    productos.Id = Guid.NewGuid().ToString();
                    productos.Estado = true;
                    productos.FechaAlta = DateTime.Now;
                    productos.UsuarioAlta = User.Identity.Name;
                    await repository.spInsertar(productos);
                    return RedirectToAction(nameof(Details), new { id = productos.Id } );
                }
            }

            ViewBag.TiposProductos = this.tiposProductos.GetCombo();
            ViewBag.CuentasCompras = this.cuentasCompras.GetCombo();
            ViewBag.CuentasVentas = this.cuentasVentas.GetCombo();
            ViewBag.UnidadesMedidas = this.unidadesMedidas.GetCombo();
            ViewBag.Alicuotas = this.alicuotas.GetCombo();
            ViewBag.Proveedores = this.proveedores.GetCombo();
            ViewBag.Categorias = this.categorias.GetCombo();

            return View(productos);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var productos = await this.repository.spProductosOne(id);
            if (productos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            ViewBag.TiposProductos = this.tiposProductos.GetCombo();
            ViewBag.CuentasCompras = this.cuentasCompras.GetCombo();
            ViewBag.CuentasVentas = this.cuentasVentas.GetCombo();
            ViewBag.UnidadesMedidas = this.unidadesMedidas.GetCombo();
            ViewBag.Alicuotas = this.alicuotas.GetCombo();
            ViewBag.Proveedores = this.proveedores.GetCombo();
            ViewBag.Categorias = this.categorias.GetCombo();

            return this.View(productos);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ProductosDTO productos)
        {
            if (id != productos.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                return RedirectToAction(nameof(Details), new { id = productos.Id });
            }

            ViewBag.TiposProductos = this.tiposProductos.GetCombo();
            ViewBag.CuentasCompras = this.cuentasCompras.GetCombo();
            ViewBag.CuentasVentas = this.cuentasVentas.GetCombo();
            ViewBag.UnidadesMedidas = this.unidadesMedidas.GetCombo();
            ViewBag.Alicuotas = this.alicuotas.GetCombo();
            ViewBag.Proveedores = this.proveedores.GetCombo();
            ViewBag.Categorias = this.categorias.GetCombo();

            return View(productos);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Productos = await repository.GetByIdAsync(id);
            Productos.Estado = !Productos.Estado;
            await repository.UpdateAsync(Productos);
            return RedirectToAction(nameof(Index));
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

        public async Task<IActionResult> CategoriaOn(string id, string id2)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id2 == null)
            {
                return NotFound();
            }


            var categoria = await this.productosCategorias.spProductosCategoriasOn(Guid.NewGuid().ToString(), id, id2);
            return RedirectToAction("Details", new { id = id });
        }

        public async Task<IActionResult> CategoriaOff(string id, string id2)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var categoria = await this.productosCategorias.spProductosCategoriasOff(id, id2);
            return RedirectToAction("Details", new { id = id });
        }

        public async Task<IActionResult> AddImage(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Productos = await this.repository.spProductosOne(id);
            if (Productos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Imagen = new ProductosImagenesViewModel();
            Imagen.ProductoId = id;

            return this.View(Imagen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(ProductosImagenesViewModel imagenes)
        {
            if (ModelState.IsValid)
                {
                    var path = string.Empty;

                    if (imagenes.ImageFile != null && imagenes.ImageFile.Length > 0)
                    {
                        var guid = Guid.NewGuid().ToString();
                        var file = $"{guid}.jpg";

                        path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\images\\Products",file);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await imagenes.ImageFile.CopyToAsync(stream);
                        }

                        path = $"~/images/Products/{file}";
                    }

                    var productImg = new ProductosImagenes();
                    productImg.Id = Guid.NewGuid().ToString();
                    productImg.ProductoId = imagenes.ProductoId;
                    productImg.ImagenUrl = path;
                    
                    await productosImagenes.CreateAsync(productImg);
                    return RedirectToAction("Details", new { id = imagenes.ProductoId });
                }
            
            return View(imagenes);
        }

        public async Task<IActionResult> DeleteImage(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var imagenes = await this.productosImagenes.GetByIdAsync(id);
            if (imagenes == null)
            {
                return new NotFoundViewResult("NoExiste");
            }
            await productosImagenes.DeleteImage(imagenes);
            
            return RedirectToAction("Details", new { id = imagenes.ProductoId });
        }

    }
}

