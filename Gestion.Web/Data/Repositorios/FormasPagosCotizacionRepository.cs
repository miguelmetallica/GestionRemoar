using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public class FormasPagosCotizacionRepository : GenericRepository<FormasPagosCotizacion>, IFormasPagosCotizacionRepository
    {
        private readonly DataContext context;
        private readonly IFactoryConnection factoryConnection;

        public FormasPagosCotizacionRepository(DataContext context,
                                IFactoryConnection factoryConnection) : base(context)
        {
            this.context = context;
            this.factoryConnection = factoryConnection;
        }

        public async Task<decimal> spCotizacion()
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
                    oCmd.CommandText = "FormasPagosContizacionGet";
                    
                    //No retornamos DataSets, siempre utilizamos objetos para hacernos 
                    //independientes de la estructura de las tablas en el resto
                    //de las capas. Para ellos leemos con el DataReader y creamos
                    //los objetos asociados que se esperan

                    //Ejecutamos el comando y retornamos los valores
                    using (SqlDataReader oReader = await oCmd.ExecuteReaderAsync())
                    {
                        if (oReader.Read())
                        {
                            var obj = new FormasPagosCotizacion();
                            //si existe algun valor, creamos el objeto y lo almacenamos
                            //en la colección
                            if (!DBNull.Value.Equals(oReader["Cotizacion"]))
                                obj.Cotizacion = (decimal)oReader["Cotizacion"];
                            else
                                obj.Cotizacion = 0;

                            return obj.Cotizacion;
                        }
                    }
                    
                    return 0;                    
                }
            }
        }


    }
}
