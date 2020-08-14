using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public class ProvinciasRepository : GenericRepository<ParamProvincias>, IProvinciasRepository
    {
        private readonly DataContext context;

        public ProvinciasRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        //public async Task AddLocalidadesAsync(LocalidadesViewModel model)
        //{
        //    var provincias = await this.GetProvinciasWithLocalidadesAsync(model.ProvinciaId);
        //    if (provincias == null)
        //    {
        //        return;
        //    }

        //    provincias.Localidades.Add(new Localidades { Nombre = model.Nombre});
        //    this.context.Provincias.Update(provincias);
        //    await this.context.SaveChangesAsync();
        //}

        //public async Task<int> DeleteLocalidadAsync(Localidades localidades)
        //{
        //    var country = await this.context.Provincias.Where(c => c.Localidades.Any(ci => ci.Id == localidades.Id)).FirstOrDefaultAsync();
        //    if (country == null)
        //    {
        //        return 0;
        //    }

        //    this.context.Localidades.Remove(localidades);
        //    await this.context.SaveChangesAsync();
        //    return country.Id;
        //}

        //public IQueryable GetProvinciasWithLocalidades()
        //{
        //    return this.context.Provincias
        //        .Include(c => c.Localidades)
        //        .OrderBy(c => c.Nombre);
        //}

        //public async Task<Provincia> GetProvinciasWithLocalidadesAsync(int id)
        //{
        //    return await this.context.Provincias
        //        .Include(c => c.Localidades)
        //        .Where(c => c.Id == id)
        //        .FirstOrDefaultAsync();
        //}

        //public async Task<int> UpdateLocalidadAsync(Localidades localidades)
        //{
        //    var country = await this.context.Provincias.Where(c => c.Localidades.Any(ci => ci.Id == localidades.Id)).FirstOrDefaultAsync();
        //    if (country == null)
        //    {
        //        return 0;
        //    }

        //    this.context.Localidades.Update(localidades);
        //    await this.context.SaveChangesAsync();
        //    return country.Id;
        //}

        //public async Task<Localidades> GetLocalidadesAsync(int id)
        //{
        //    return await this.context.Localidades.FindAsync(id);
        //}

        public IEnumerable<SelectListItem> GetCombo()
        {
            var list = this.context.ParamProvincias.Where(x => x.Estado == true).Select(c => new SelectListItem
            {
                Text = c.Descripcion,
                Value = c.Id.ToString()
            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona una Provincia...)",
                Value = ""
            });

            return list;
        }

        //public IEnumerable<SelectListItem> GetComboLocalidades(int provinciaId)
        //{
        //    var country = this.context.Provincias.Find(provinciaId);
        //    var list = new List<SelectListItem>();
        //    if (country != null)
        //    {
        //        list = country.Localidades.Select(c => new SelectListItem
        //        {
        //            Text = c.Nombre,
        //            Value = c.Id.ToString()
        //        }).OrderBy(l => l.Text).ToList();
        //    }

        //    list.Insert(0, new SelectListItem
        //    {
        //        Text = "(Selecciona una Localidad...)",
        //        Value = "0"
        //    });

        //    return list;
        //}

        //public async Task<Provincia> GetProvinciasAsync(Localidades localidades)
        //{
        //    return await this.context.Provincias.Where(c => c.Localidades.Any(ci => ci.Id == localidades.Id)).FirstOrDefaultAsync();
        //}

    }
}
