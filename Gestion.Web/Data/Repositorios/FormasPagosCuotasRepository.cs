using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Gestion.Web.Data
{
    public class FormasPagosCuotasRepository : GenericRepository<FormasPagosCuotas>, IFormasPagosCuotasRepository
    {
        private readonly DataContext context;
        private readonly IFactoryConnection factoryConnection;

        public FormasPagosCuotasRepository(DataContext context, IFactoryConnection factoryConnection) : base(context)
        {
            this.context = context;
            this.factoryConnection = factoryConnection;
        }

        public IEnumerable<SelectListItem> GetCombo()
        {
            var list = this.context.FormasPagosCuotas
                .Where(x => x.Estado == true).Select(c => new SelectListItem
                {
                    Text = c.Cuota.ToString(),
                    Value = c.Id.ToString()
                }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona una Cuota...)",
                Value = ""
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetBancos()
        {
            List<SelectListItem> lst = new List<SelectListItem>();

            lst.Add(new SelectListItem() { Text = "(Selecciona un Banco...)", Value = "" });

            using (var oCnn = factoryConnection.GetConnection())
            {
                using (SqlCommand oCmd = new SqlCommand())
                {
                    //asignamos la conexion de trabajo
                    oCmd.Connection = oCnn;

                    //utilizamos stored procedures
                    oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                    //el indicamos cual stored procedure utilizar
                    oCmd.CommandText = "FormasPagosBancosGet";

                    //No retornamos DataSets, siempre utilizamos objetos para hacernos 
                    //independientes de la estructura de las tablas en el resto
                    //de las capas. Para ellos leemos con el DataReader y creamos
                    //los objetos asociados que se esperan

                    //Ejecutamos el comando y retornamos los valores
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            lst.Add(new SelectListItem() { Text = (oReader["Descripcion"] as string), Value = (oReader["Id"] as string) });
                        }
                    }
                }
            }

            return lst;
        }
        public IEnumerable<SelectListItem> GetEntidades(string formaPagoId)
        {
            List<SelectListItem> lst = new List<SelectListItem>();

            lst.Add(new SelectListItem() { Text = "(Selecciona una Entidad...)", Value = "" });
            
            using (var oCnn = factoryConnection.GetConnection())
            {
                using (SqlCommand oCmd = new SqlCommand())
                {
                    //asignamos la conexion de trabajo
                    oCmd.Connection = oCnn;

                    //utilizamos stored procedures
                    oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                    //el indicamos cual stored procedure utilizar
                    oCmd.CommandText = "FormasPagosCuotasEntidadesGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@FormaPagoId", formaPagoId);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 


                    //No retornamos DataSets, siempre utilizamos objetos para hacernos 
                    //independientes de la estructura de las tablas en el resto
                    //de las capas. Para ellos leemos con el DataReader y creamos
                    //los objetos asociados que se esperan

                    //Ejecutamos el comando y retornamos los valores
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while(oReader.Read())
                        {
                            lst.Add(new SelectListItem() { Text = (oReader["Descripcion"] as string), Value = (oReader["Id"] as string) });
                        }
                    }
                }
            }

            return lst;
        }

        public IEnumerable<SelectListItem> GetCuotas(string formaPagoId, string entidadId)
        {
            List<SelectListItem> lst = new List<SelectListItem>();

            lst.Add(new SelectListItem() { Text = "(Selecciona una Cuota...)", Value = "" });

            using (var oCnn = factoryConnection.GetConnection())
            {
                using (SqlCommand oCmd = new SqlCommand())
                {
                    //asignamos la conexion de trabajo
                    oCmd.Connection = oCnn;

                    //utilizamos stored procedures
                    oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                    //el indicamos cual stored procedure utilizar
                    oCmd.CommandText = "FormasPagosEntidadesCuotasGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@FormaPagoId", formaPagoId);
                    oCmd.Parameters.AddWithValue("@EntidadId", entidadId);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 


                    //No retornamos DataSets, siempre utilizamos objetos para hacernos 
                    //independientes de la estructura de las tablas en el resto
                    //de las capas. Para ellos leemos con el DataReader y creamos
                    //los objetos asociados que se esperan

                    //Ejecutamos el comando y retornamos los valores
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {                            
                            lst.Add(new SelectListItem() { Text = (oReader["Descripcion"] as string), Value = (((int)oReader["Cuota"]).ToString()) });
                        }
                    }
                }
            }

            return lst;
        }

        public IEnumerable<SelectListItem> GetCuotasInteres(string formaPagoId, string entidadId,int cuota)
        {
            List<SelectListItem> lst = new List<SelectListItem>();

            using (var oCnn = factoryConnection.GetConnection())
            {
                using (SqlCommand oCmd = new SqlCommand())
                {
                    //asignamos la conexion de trabajo
                    oCmd.Connection = oCnn;

                    //utilizamos stored procedures
                    oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                    //el indicamos cual stored procedure utilizar
                    oCmd.CommandText = "FormasPagosEntidadesCuotasInteresGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@FormaPagoId", formaPagoId);
                    oCmd.Parameters.AddWithValue("@EntidadId", entidadId);
                    oCmd.Parameters.AddWithValue("@Cuota", cuota);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 


                    //No retornamos DataSets, siempre utilizamos objetos para hacernos 
                    //independientes de la estructura de las tablas en el resto
                    //de las capas. Para ellos leemos con el DataReader y creamos
                    //los objetos asociados que se esperan

                    //Ejecutamos el comando y retornamos los valores
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            lst.Add(new SelectListItem() { Text = (((decimal)oReader["Interes"]).ToString()), Value = (((decimal)oReader["Interes"]).ToString()) });
                        }
                    }
                }
            }

            return lst;
        }

        public List<FormasPagosCuotas> GetAll(string formaPagoId) 
        {
            var list = this.context.FormasPagosCuotas
                .Where(x => x.FormaPagoId == formaPagoId)
                .OrderBy(x => x.EntidadId)
                .OrderBy(x => x.Cuota)
                .ToList();

            return list;
        }

        public FormasPagosCuotas GetCuotaUno(string formaPagoId)
        {
            var obj = new FormasPagosCuotas();

            using (var oCnn = factoryConnection.GetConnection())
            {
                using (SqlCommand oCmd = new SqlCommand())
                {
                    //asignamos la conexion de trabajo
                    oCmd.Connection = oCnn;

                    //utilizamos stored procedures
                    oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                    //el indicamos cual stored procedure utilizar
                    oCmd.CommandText = "FormasPagosCuotaUnoGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@FormaPagoId", formaPagoId);
                    
                    //No retornamos DataSets, siempre utilizamos objetos para hacernos 
                    //independientes de la estructura de las tablas en el resto
                    //de las capas. Para ellos leemos con el DataReader y creamos
                    //los objetos asociados que se esperan

                    //Ejecutamos el comando y retornamos los valores
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        if(oReader.Read())
                        {
                            obj.Id = oReader["Id"] as string;
                            obj.FormaPagoId = oReader["FormaPagoId"] as string;
                            obj.EntidadId = oReader["EntidadId"] as string;
                            obj.Descripcion = oReader["Descripcion"] as string;
                            obj.Cuota = (int)oReader["Cuota"];
                            obj.Interes = (decimal)oReader["Interes"];
                            obj.FechaDesde = (DateTime)oReader["FechaDesde"];
                            obj.FechaHasta = (DateTime)oReader["FechaHasta"];
                            obj.Estado = (bool)oReader["Estado"];

                            return obj;
                        }
                    }
                }
            }

            return null;
        }


    }
}
