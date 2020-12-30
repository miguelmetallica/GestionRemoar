using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize(Roles = "Admin,Localidades")]
    
    public class LocalidadesController : Controller
    {
        private readonly ILocalidadesRepository repository; 
        private readonly IUserHelper userHelper;

        public LocalidadesController(ILocalidadesRepository repository, IUserHelper userHelper)
        {
            this.repository = repository;
            this.userHelper = userHelper;
        }

        public IActionResult Index()
        {

            return View(repository.GetAll());
            
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Localidades = await this.repository.GetByIdAsync(id);
            if (Localidades == null)
            {
                return new NotFoundViewResult("NoExiste");
            }
            
            return this.View(Localidades);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Localidades localidades)
        {
            if (ModelState.IsValid)
            {
                await repository.CreateAsync(localidades);
                return RedirectToAction(nameof(Index));
            }
            return View(localidades);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Localidades = await this.repository.GetByIdAsync(id);
            if (Localidades == null)
            {
                return new NotFoundViewResult("NoExiste");
            }
            
            return this.View(Localidades);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Localidades localidades)
        {
            if (id != localidades.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await repository.UpdateAsync(localidades);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(localidades.Id))
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
            return View(localidades);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Localidades = await this.repository.GetByIdAsync(id);
            if (Localidades == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(Localidades);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var Localidades = await repository.GetByIdAsync(id);
            await repository.DeleteAsync(Localidades);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult NoExiste()
        {
            return this.View();
        }

    }
    
}
