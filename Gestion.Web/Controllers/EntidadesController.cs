using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize(Roles = "Admin,Entidades")]
    public class EntidadesController : Controller
    {
        private readonly IEntidadesRepository repository; 
        private readonly IUserHelper userHelper;

        public EntidadesController(IEntidadesRepository repository, IUserHelper userHelper)
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

            var Entidades = await this.repository.GetByIdAsync(id);
            if (Entidades == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(Entidades);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParamEntidades Entidades)
        {
            if (ModelState.IsValid)
            {
                Entidades.Estado = true;
                Entidades.Codigo = Entidades.Codigo.ToUpper();
                Entidades.Descripcion = Entidades.Descripcion.ToUpper();
                await repository.CreateAsync(Entidades);
                return RedirectToAction(nameof(Index));
            }
            return View(Entidades);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Entidades = await this.repository.GetByIdAsync(id);
            if (Entidades == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(Entidades);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ParamEntidades Entidades)
        {
            if (id != Entidades.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Entidades.Codigo = Entidades.Codigo.ToUpper();
                    Entidades.Descripcion = Entidades.Descripcion.ToUpper();
                    
                    await repository.UpdateAsync(Entidades);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(Entidades.Id))
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
            return View(Entidades);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Entidades = await this.repository.GetByIdAsync(id);
            if (Entidades == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            Entidades.Estado = !Entidades.Estado;
            await repository.DeleteAsync(Entidades);
            return RedirectToAction(nameof(Index));
        }        

    }
}
