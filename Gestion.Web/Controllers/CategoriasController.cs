using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using WooCommerceNET;
using WooCommerceNET.WooCommerce.v3;

namespace Gestion.Web.Controllers
{
    [Authorize]
    public class CategoriasController : Controller
    {
        private readonly ICategoriasRepository repository; 
        private readonly IUserHelper userHelper;

        public CategoriasController(ICategoriasRepository repository, IUserHelper userHelper)
        {
            this.repository = repository;
            this.userHelper = userHelper;
        }

        public IActionResult Index()
        {

            return View(repository.GetAll());
            
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Categorias = await this.repository.GetByIdAsync(id.Value);
            if (Categorias == null)
            {
                return new NotFoundViewResult("NoExiste");
            }
            MyRestAPI rest = new MyRestAPI("http://remoar.site/catalogo/wp-json/wc/v3/", "ck_48a4ce75e1343bfa3195aeb98a484e0bfd9d311d", "cs_d7170330f840cca7da468cff92ad22c07f940682");
            WCObject wc = new WCObject(rest);

            //Get all products
            var products = await wc.Product.Get("2143");

            return this.View(Categorias);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoriasViewModel view)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (view.ImagenFile != null && view.ImagenFile.Length > 0)
                {

                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(), 
                        "wwwroot\\images\\categorias", 
                        file);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await view.ImagenFile.CopyToAsync(stream);
                    }

                    path = $"~/images/categorias/{file}";
                }
                
                var categorias = this.ToCategoria(view, path);
                await repository.CreateAsync(categorias);
                return RedirectToAction(nameof(Index));
            }
            return View(view);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Categorias = await this.repository.GetByIdAsync(id.Value);
            if (Categorias == null)
            {
                return new NotFoundViewResult("NoExiste");
            }
            var view = this.ToCategoria(Categorias);
            return this.View(view);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoriasViewModel view)
        {
            if (id != view.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (view.ImagenFile != null && view.ImagenFile.Length > 0)
                {
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\categorias",
                        file);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await view.ImagenFile.CopyToAsync(stream);
                    }

                    path = $"~/images/categorias/{file}";
                }
                else
                {
                    path = view.Imagen;
                }

                var categorias = this.ToCategoria(view, path);

                try
                {
                    await repository.UpdateAsync(categorias);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(view.Id))
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
            return View(view);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Categorias = await this.repository.GetByIdAsync(id.Value);
            if (Categorias == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(Categorias);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Categorias = await repository.GetByIdAsync(id);
            await repository.DeleteAsync(Categorias);
            return RedirectToAction(nameof(Index));
        }

        private Categorias ToCategoria(CategoriasViewModel view, string path)
        {
            return new Categorias
            {
                Id = view.Id,
                Nombre = view.Nombre,
                Slug = view.Slug,
                ParentId = view.ParentId,
                Descripcion = view.Descripcion,
                Imagen = path,
                MenuOrden = view.MenuOrden,
            };
        }

        private CategoriasViewModel ToCategoria(Categorias categorias)
        {
            return new CategoriasViewModel
            {
                Id = categorias.Id,
                Nombre = categorias.Nombre,
                Slug = categorias.Slug,
                ParentId = categorias.ParentId,
                Descripcion = categorias.Descripcion,
                Imagen = categorias.Imagen,
                MenuOrden = categorias.MenuOrden,
            };
        }

        public IActionResult NoExiste()
        {
            return this.View();
        }

    }

    public class MyRestAPI : RestAPI
    {
        public MyRestAPI(string url, string key, string secret, bool authorizedHeader = true,
            Func<string, string> jsonSerializeFilter = null,
            Func<string, string> jsonDeserializeFilter = null,
            Action<HttpWebRequest> requestFilter = null) : base(url, key, secret, authorizedHeader, jsonSerializeFilter, jsonDeserializeFilter, requestFilter)
        {
        }

        public override T DeserializeJSon<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public override string SerializeJSon<T>(T t)
        {
            return JsonConvert.SerializeObject(t);
        }
    }
}
