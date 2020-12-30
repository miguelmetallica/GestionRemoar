using Gestion.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Gestion.Web.Data
{
    public class DataContext : IdentityDbContext<Usuarios>
    {
        public virtual DbSet<Clientes> Clientes { get; set; }
        public virtual DbSet<ParamAlicuotas> ParamAlicuotas { get; set; }
        public virtual DbSet<ParamCategorias> ParamCategorias { get; set; }
        public virtual DbSet<ParamColores> ParamColores { get; set; }
        public virtual DbSet<ParamCondicionesIva> ParamCondicionesIva { get; set; }
        public virtual DbSet<ParamCuentasCompras> ParamCuentasCompras { get; set; }
        public virtual DbSet<ParamCuentasVentas> ParamCuentasVentas { get; set; }
        public virtual DbSet<ParamEtiquetas> ParamEtiquetas { get; set; }
        public virtual DbSet<ParamMarcas> ParamMarcas { get; set; }
        public virtual DbSet<ParamProvincias> ParamProvincias { get; set; }
        public virtual DbSet<ParamTiposDocumentos> ParamTiposDocumentos { get; set; }
        public virtual DbSet<ParamTiposComprobantes> ParamTiposComprobantes { get; set; }
        public virtual DbSet<ParamConceptosIncluidos> ParamConceptosIncluidos { get; set; }
        public virtual DbSet<ParamTiposProductos> ParamTiposProductos { get; set; }
        public virtual DbSet<ParamTiposResponsables> ParamTiposResponsables { get; set; }
        public virtual DbSet<ParamUnidadesMedidas> ParamUnidadesMedidas { get; set; }
        public virtual DbSet<ComprobantesNumeraciones> ComprobantesNumeraciones { get; set; }

        public virtual DbSet<Productos> Productos { get; set; }
        public virtual DbSet<ProductosCategorias> ProductosCategorias { get; set; }
        public virtual DbSet<ProductosColores> ProductosColores { get; set; }
        public virtual DbSet<ProductosEtiquetas> ProductosEtiquetas { get; set; }
        public virtual DbSet<ProductosImagenes> ProductosImagenes { get; set; }

        public virtual DbSet<Sucursales> Sucursales { get; set; }

        public DbSet<Presupuestos> Presupuestos { get; set; }
        public DbSet<PresupuestosDetalle> PresupuestosDetalle { get; set; }
        public DbSet<PresupuestosDetalleTemp> PresupuestosDetalleTemp { get; set; }
        public DbSet<ParamPresupuestosEstados> ParamPresupuestosEstados { get; set; }
        public DbSet<ParamPresupuestosDescuentos> ParamPresupuestosDescuentos { get; set; }

        public virtual DbSet<FormasPagos> FormasPagos { get; set; }

        public DbSet<ParamCajas> ParamCajas{ get; set; }
        //public DbSet<CajasAperturasCierres> CajasAperturasCierres { get; set; }
        public DbSet<CajasMovimientos> CajasMovimientos { get; set; }
        public DbSet<ParamCajasMovimientosTipos> ParamCajasMovimientosTipos { get; set; }

        public DbSet<SistemaConfiguraciones> SistemaConfiguraciones { get; set; }

        public virtual DbSet<ParamClientesCategorias> ParamClientesCategorias { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }

}
