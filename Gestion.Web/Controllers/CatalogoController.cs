using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    
    public class CatalogoController : Controller
    {
        //private readonly ICatalogoRepository repository; 
        //private readonly IUserHelper userHelper;

        public CatalogoController()//ICatalogoRepository repository, IUserHelper userHelper)
        {
            //this.repository = repository;
            //this.userHelper = userHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        //public async Task<IActionResult> Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new NotFoundViewResult("NoExiste");
        //    }

        //    var Catalogo = await this.repository.GetByIdAsync(id);
        //    if (Catalogo == null)
        //    {
        //        return new NotFoundViewResult("NoExiste");
        //    }

        //    return this.View(Catalogo);
        //}

        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(ParamCatalogo Catalogo)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Catalogo.Estado = true;
        //        await repository.CreateAsync(Catalogo);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(Catalogo);
        //}

        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new NotFoundViewResult("NoExiste");
        //    }

        //    var Catalogo = await this.repository.GetByIdAsync(id);
        //    if (Catalogo == null)
        //    {
        //        return new NotFoundViewResult("NoExiste");
        //    }

        //    return this.View(Catalogo);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, ParamCatalogo Catalogo)
        //{
        //    if (id != Catalogo.Id)
        //    {
        //        return new NotFoundViewResult("NoExiste");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            await repository.UpdateAsync(Catalogo);
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!await repository.ExistAsync(Catalogo.Id))
        //            {
        //                return new NotFoundViewResult("NoExiste");
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(Catalogo);
        //}

        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new NotFoundViewResult("NoExiste");
        //    }

        //    var Catalogo = await this.repository.GetByIdAsync(id);
        //    if (Catalogo == null)
        //    {
        //        return new NotFoundViewResult("NoExiste");
        //    }

        //    Catalogo.Estado = !Catalogo.Estado;
        //    await repository.DeleteAsync(Catalogo);
        //    return RedirectToAction(nameof(Index));
        //}        

    }
}
