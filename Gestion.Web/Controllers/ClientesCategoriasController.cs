using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize(Roles = "Admin,ClientesCategorias")]
    
    public class ClientesCategoriasController : Controller
    {
        private readonly IClientesCategoriasRepository repository; 
        private readonly IUserHelper userHelper;

        public ClientesCategoriasController(IClientesCategoriasRepository repository, IUserHelper userHelper)
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

            var ClientesCategorias = await this.repository.GetByIdAsync(id);
            if (ClientesCategorias == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(ClientesCategorias);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParamClientesCategorias ClientesCategorias)
        {
            if (ModelState.IsValid)
            {
                ClientesCategorias.Estado = true;
                await repository.CreateAsync(ClientesCategorias);
                return RedirectToAction(nameof(Index));
            }
            return View(ClientesCategorias);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var ClientesCategorias = await this.repository.GetByIdAsync(id);
            if (ClientesCategorias == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(ClientesCategorias);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ParamClientesCategorias ClientesCategorias)
        {
            if (id != ClientesCategorias.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await repository.UpdateAsync(ClientesCategorias);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(ClientesCategorias.Id))
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
            return View(ClientesCategorias);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var ClientesCategorias = await this.repository.GetByIdAsync(id);
            if (ClientesCategorias == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            ClientesCategorias.Estado = !ClientesCategorias.Estado;
            await repository.DeleteAsync(ClientesCategorias);
            return RedirectToAction(nameof(Index));
        }        

    }
}
