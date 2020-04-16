using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WooCommerceNET.WooCommerce.v3;

namespace Gestion.Web.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly IProductosRepository repository; 
        private readonly IUserHelper userHelper;

        public ProductosController(IProductosRepository repository, IUserHelper userHelper)
        {
            this.repository = repository;
            this.userHelper = userHelper;
        }

        public async Task<IActionResult> Index()
        {
            MyRestAPI rest = new MyRestAPI("http://remoar.site/catalogo/wp-json/wc/v3/", "ck_48a4ce75e1343bfa3195aeb98a484e0bfd9d311d", "cs_d7170330f840cca7da468cff92ad22c07f940682");
            WCObject wc = new WCObject(rest);

            //Get all products
            var wproducts = await wc.Product.GetAll();

            //return View(repository.GetAll());
            var productos = ToProductos(wproducts);

            return View(productos);
        }

        private List<Productos> ToProductos(List<Product> wproducts)
        {
            List<Productos> list = new List<Productos>();
            try
            {                
                foreach (var item in wproducts)
                {
                    list.Add(new Productos()
                    {
                        Id = (int)item.id,
                        Nombre = item.name,
                        Codigo = item.sku,
                        Tipo = item.type,
                        Precio = item.price,
                        Descripcion = item.description,
                        DescripcionCorta = item.short_description
                    });
                };

                return list;

            }
            catch (Exception)
            {

                return list;
            }
            

            
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Productos = await this.repository.GetByIdAsync(id.Value);
            if (Productos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }
            
            return this.View(Productos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Productos productos)
        {
            if (ModelState.IsValid)
            {
                await repository.CreateAsync(productos);
                return RedirectToAction(nameof(Index));
            }
            return View(productos);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var productos = await this.repository.GetByIdAsync(id.Value);
            if (productos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }
            return this.View(productos);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Productos productos)
        {
            if (id != productos.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await repository.UpdateAsync(productos);
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
            return View(productos);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Productos = await this.repository.GetByIdAsync(id.Value);
            if (Productos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(Productos);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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
