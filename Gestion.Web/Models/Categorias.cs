namespace Gestion.Web.Models
{
    public partial class Categorias : IEntidades
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Slug { get; set; }
        public int? ParentId { get; set; }
        public string Descripcion { get; set; }
        public string Display { get; set; }
        public string Imagen { get; set; }
        public int? MenuOrden { get; set; }

        public string ImagenFull {
            get
            {
                if (string.IsNullOrEmpty(this.Imagen))
                {
                    return null;
                }

                return $"https://localhost:44376{this.Imagen.Substring(1)}";
            }
        }
    }
}
