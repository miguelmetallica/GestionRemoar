using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public class CajasRepository : GenericRepository<Cajas>, ICajasRepository
    {
        private readonly DataContext context;
        private readonly IFactoryConnection factoryConnection;

        public CajasRepository(DataContext context,
                            IFactoryConnection factoryConnection) : base(context)
        {
            this.context = context;
            this.factoryConnection = factoryConnection;
        }

        public IEnumerable<SelectListItem> GetCombo(string sucursalId)
        {
            var list = this.context.Cajas
                .Where(x => x.SucursalId == sucursalId)
                .Select(c => new SelectListItem
            {
                Text = c.Codigo,
                Value = c.Id.ToString()
            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona una Caja...)",
                Value = ""
            });

            return list;
        }

        public async Task<List<CajasEstadoDTO>> spCajasEstadoImportesGet()
        {
            //Creamos la conexión a utilizar.
            //Utilizamos la sentencia Using para asegurarnos de cerrar la conexión
            //y liberar el objeto al salir de esta sección de manera automática            
            using (var oCnn = factoryConnection.GetConnection())
            {
                using (SqlCommand oCmd = new SqlCommand())
                {
                    //asignamos la conexion de trabajo
                    oCmd.Connection = oCnn;

                    //utilizamos stored procedures
                    oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                    //el indicamos cual stored procedure utilizar
                    oCmd.CommandText = "CajasEstadoImportesGet";

                    //le asignamos el parámetro para el stored procedure
                    //oCmd.Parameters.AddWithValue("@Id", Id);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<CajasEstadoDTO>();

                    //No retornamos DataSets, siempre utilizamos objetos para hacernos 
                    //independientes de la estructura de las tablas en el resto
                    //de las capas. Para ellos leemos con el DataReader y creamos
                    //los objetos asociados que se esperan
                    try
                    {
                        //Ejecutamos el comando y retornamos los valores
                        using (SqlDataReader oReader = await oCmd.ExecuteReaderAsync())
                        {
                            while (oReader.Read())
                            {
                                //si existe algun valor, creamos el objeto y lo almacenamos
                                //en la colección
                                var obj = new CajasEstadoDTO();
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.FormaPagoTipo = oReader["FormaPagoTipo"] as string;
                                obj.Total = (decimal)(oReader["Total"] ?? 0);
                                obj.Sucursal = oReader["Sucursal"] as string;
                                //Agregamos el objeto a la coleccion de resultados
                                objs.Add(obj);
                                obj = null;
                            }
                        }
                        //retornamos los valores encontrados


                        return objs;
                    }

                    finally
                    {
                        //el Finally nos da siempre la oportunidad de liberar
                        //la memoria utilizada por los objetos 
                        objs = null;
                    }
                }
            }
        }
    }
}
