using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Gestion.Web.Data
{
    public class ComprobantesRepository : GenericRepository<Comprobantes>, IComprobantesRepository
    {
        private readonly DataContext context;
        private readonly IUserHelper userHelper;
        private readonly IFactoryConnection factoryConnection;
        private readonly IClientesRepository clientesRepository;
        private readonly IPresupuestosEstadosRepository presupuestosEstados;

        public ComprobantesRepository(DataContext context, 
                                    IUserHelper userHelper, 
                                    IFactoryConnection factoryConnection,
                                    IClientesRepository clientesRepository,
                                    IPresupuestosEstadosRepository presupuestosEstados) : base(context)
        {
            this.context = context;
            this.userHelper = userHelper;
            this.factoryConnection = factoryConnection;
            this.clientesRepository = clientesRepository;
            this.presupuestosEstados = presupuestosEstados;
        }
         
        public async Task<List<Comprobantes>> spComprobantes(string clienteId)
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
                    oCmd.CommandText = "ComprobantesGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@ClienteId", clienteId);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<Comprobantes>();

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
                                var obj = new Comprobantes();
                                obj.Id = oReader["Id"] as string;
                                obj.Codigo = oReader["Codigo"] as string;
                                
                                obj.TipoComprobanteId = oReader["TipoComprobanteId"] as string;
                                obj.TipoComprobante = oReader["TipoComprobante"] as string;
                                obj.TipoComprobanteCodigo = oReader["TipoComprobanteCodigo"] as string;
                                
                                obj.PresupuestoId = oReader["PresupuestoId"] as string;
                                obj.Letra = oReader["Letra"] as string;
                                obj.PtoVenta = (int)(oReader["PtoVenta"] ?? 0) ;
                                obj.Numero = (decimal)(oReader["Numero"] ?? 0);
                                obj.FechaComprobante = (DateTime)oReader["FechaComprobante"];
                                obj.ConceptoIncluidoId = oReader["ConceptoIncluidoId"] as string;
                                obj.ConceptoIncluidoCodigo = oReader["ConceptoIncluidoCodigo"] as string;
                                obj.ConceptoIncluido = oReader["ConceptoIncluido"] as string;

                                if (!DBNull.Value.Equals(oReader["PeriodoFacturadoDesde"]))
                                    obj.PeriodoFacturadoDesde = (DateTime)(oReader["PeriodoFacturadoDesde"] ?? DateTime.Now);
                                if (!DBNull.Value.Equals(oReader["PeriodoFacturadoDesde"]))
                                    obj.PeriodoFacturadoHasta = (DateTime)(oReader["PeriodoFacturadoDesde"] ?? DateTime.Now);
                                if (!DBNull.Value.Equals(oReader["FechaVencimiento"]))
                                    obj.FechaVencimiento = (DateTime)(oReader["FechaVencimiento"] ?? DateTime.Now); 
                                
                                obj.TipoResponsableId = oReader["TipoResponsableId"] as string;
                                obj.TipoResponsableCodigo = oReader["TipoResponsableCodigo"] as string;
                                obj.TipoResponsable = oReader["TipoResponsable"] as string;
                                
                                obj.ClienteId = oReader["ClienteId"] as string;
                                obj.ClienteCodigo = oReader["ClienteCodigo"] as string;
                                obj.TipoDocumentoId = oReader["TipoDocumentoId"] as string;
                                obj.TipoDocumentoCodigo = oReader["TipoDocumentoCodigo"] as string;
                                obj.TipoDocumento = oReader["TipoDocumento"] as string;
                                obj.NroDocumento = oReader["NroDocumento"] as string;
                                obj.CuilCuit = oReader["CuilCuit"] as string;
                                obj.RazonSocial = oReader["RazonSocial"] as string;

                                obj.ProvinciaId = oReader["ProvinciaId"] as string;
                                obj.ProvinciaCodigo = oReader["ProvinciaCodigo"] as string;
                                obj.Provincia = oReader["Provincia"] as string;
                                
                                obj.Localidad = oReader["Localidad"] as string;
                                obj.CodigoPostal = oReader["CodigoPostal"] as string;
                                obj.Calle = oReader["Calle"] as string;
                                obj.CalleNro = oReader["CalleNro"] as string;
                                obj.PisoDpto = oReader["PisoDpto"] as string;
                                obj.OtrasReferencias = oReader["OtrasReferencias"] as string;

                                obj.Email = oReader["Email"] as string;
                                obj.Telefono = oReader["Telefono"] as string;
                                obj.Celular = oReader["Celular"] as string;
                                
                                obj.Total = (decimal)(oReader["Total"] ?? 0);
                                obj.TotalSinImpuesto = (decimal)(oReader["TotalSinImpuesto"] ?? 0);
                                obj.TotalSinDescuento = (decimal)(oReader["TotalSinDescuento"] ?? 0);
                                obj.TotalSinImpuestoSinDescuento = (decimal)(oReader["TotalSinImpuestoSinDescuento"] ?? 0);
                                obj.DescuentoPorcentaje = (decimal)(oReader["DescuentoPorcentaje"] ?? 0);
                                obj.DescuentoTotal = (decimal)(oReader["DescuentoTotal"] ?? 0);
                                obj.DescuentoSinImpuesto = (decimal)(oReader["DescuentoSinImpuesto"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["ImporteTributos"]))
                                    obj.ImporteTributos = (decimal)(oReader["ImporteTributos"] ?? 0);                                
                                
                                obj.Observaciones = oReader["Observaciones"] as string;

                                obj.TipoComprobanteAnulaId = oReader["TipoComprobanteAnulaId"] as string;
                                obj.TipoComprobanteAnula = oReader["TipoComprobanteAnula"] as string;
                                obj.TipoComprobanteAnulaCodigo = oReader["TipoComprobanteAnulaCodigo"] as string;

                                obj.LetraAnula = oReader["LetraAnula"] as string;
                                if (!DBNull.Value.Equals(oReader["PtoVtaAnula"]))
                                    obj.PtoVtaAnula =  (int)oReader["PtoVtaAnula"];

                                if (!DBNull.Value.Equals(oReader["NumeroAnula"]))
                                    obj.NumeroAnula = (decimal)(oReader["NumeroAnula"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["FechaAnulacion"]))
                                    obj.FechaAnulacion = (DateTime)oReader["FechaAnulacion"];

                                if (!DBNull.Value.Equals(oReader["Saldo"]))
                                    obj.Saldo = (decimal)(oReader["Saldo"] ?? 0);

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

        public async Task<int> spRecibo(ComprobantesReciboDTO reciboDTO)
        {
            try
            {
                using (var oCnn = factoryConnection.GetConnection())
                {
                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        //asignamos la conexion de trabajo
                        oCmd.Connection = oCnn;

                        //utilizamos stored procedures
                        oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //el indicamos cual stored procedure utilizar
                        oCmd.CommandText = "ComprobantesInsertarRecibo";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@ComprobanteId", reciboDTO.ComprobanteId);
                        //oCmd.Parameters.AddWithValue("@ImporteCancela", reciboDTO.Importe);
                        oCmd.Parameters.AddWithValue("@Observaciones", reciboDTO.Observaciones);
                        oCmd.Parameters.AddWithValue("@Usuario", reciboDTO.Usuario);

                        //Ejecutamos el comando y retornamos el id generado
                        await oCmd.ExecuteScalarAsync();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el registro: " + ex.Message);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
        }

        public async Task<int> spEfectivo(ComprobantesEfectivoDTO reciboDTO)
        {
            try
            {
                using (var oCnn = factoryConnection.GetConnection())
                {
                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        //asignamos la conexion de trabajo
                        oCmd.Connection = oCnn;

                        //utilizamos stored procedures
                        oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //el indicamos cual stored procedure utilizar
                        oCmd.CommandText = "ComprobantesTmpInsertarEfectivo";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@ComprobanteId", reciboDTO.ComprobanteId);
                        oCmd.Parameters.AddWithValue("@FormaPagoId", reciboDTO.FormaPagoId);
                        oCmd.Parameters.AddWithValue("@Importe", reciboDTO.Importe);
                        oCmd.Parameters.AddWithValue("@TotalImporte", reciboDTO.Saldo);
                        oCmd.Parameters.AddWithValue("@DescuentoImporte", reciboDTO.SaldoConDescuento);
                        oCmd.Parameters.AddWithValue("@DescuentoPorcentaje", reciboDTO.Descuento);                        
                        oCmd.Parameters.AddWithValue("@Observaciones", reciboDTO.Observaciones);
                        oCmd.Parameters.AddWithValue("@Usuario", reciboDTO.Usuario);

                        //Ejecutamos el comando y retornamos el id generado
                        await oCmd.ExecuteScalarAsync();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el registro: " + ex.Message);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
        }
        
        public async Task<int> spOtro(ComprobantesOtroDTO reciboDTO)
        {
            try
            {
                using (var oCnn = factoryConnection.GetConnection())
                {
                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        //asignamos la conexion de trabajo
                        oCmd.Connection = oCnn;

                        //utilizamos stored procedures
                        oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //el indicamos cual stored procedure utilizar
                        oCmd.CommandText = "ComprobantesTmpInsertarOtro";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@ComprobanteId", reciboDTO.ComprobanteId);
                        oCmd.Parameters.AddWithValue("@FormaPagoId", reciboDTO.FormaPagoId);
                        oCmd.Parameters.AddWithValue("@Importe", reciboDTO.Importe);
                        oCmd.Parameters.AddWithValue("@Otros", reciboDTO.FormaPago);
                        oCmd.Parameters.AddWithValue("@Observaciones", reciboDTO.Observaciones);
                        oCmd.Parameters.AddWithValue("@CodigoAutorizacion", reciboDTO.CodigoAutorizacion);
                        oCmd.Parameters.AddWithValue("@Usuario", reciboDTO.Usuario);

                        //Ejecutamos el comando y retornamos el id generado
                        await oCmd.ExecuteScalarAsync();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el registro: " + ex.Message);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
        }

        public async Task<int> spTarjeta(ComprobantesTarjetaDTO reciboDTO)
        {
            try
            {
                using (var oCnn = factoryConnection.GetConnection())
                {
                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        //asignamos la conexion de trabajo
                        oCmd.Connection = oCnn;

                        //utilizamos stored procedures
                        oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //el indicamos cual stored procedure utilizar
                        oCmd.CommandText = "ComprobantesTmpInsertarTarjeta";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@ComprobanteId", reciboDTO.ComprobanteId);
                        oCmd.Parameters.AddWithValue("@FormaPagoId", reciboDTO.FormaPagoId);
                        oCmd.Parameters.AddWithValue("@TarjetaId", reciboDTO.TarjetaId);
                        oCmd.Parameters.AddWithValue("@Cuota", reciboDTO.Cuota);
                        oCmd.Parameters.AddWithValue("@Importe", reciboDTO.Importe);
                        oCmd.Parameters.AddWithValue("@Interes", reciboDTO.Interes);
                        oCmd.Parameters.AddWithValue("@Total", reciboDTO.Total);
                        oCmd.Parameters.AddWithValue("@TarjetaCliente", reciboDTO.TarjetaCliente);
                        oCmd.Parameters.AddWithValue("@TarjetaNumero", reciboDTO.TarjetaNumero);
                        oCmd.Parameters.AddWithValue("@TarjetaVenceAño", reciboDTO.TarjetaVenceAño);
                        oCmd.Parameters.AddWithValue("@TarjetaVenceMes", reciboDTO.TarjetaVenceMes);
                        oCmd.Parameters.AddWithValue("@TarjetaCodigoSeguridad", reciboDTO.TarjetaCodigoSeguridad);
                        oCmd.Parameters.AddWithValue("@TarjetaEsDebito", reciboDTO.TarjetaEsDebito);
                        oCmd.Parameters.AddWithValue("@Observaciones", reciboDTO.Observaciones);
                        oCmd.Parameters.AddWithValue("@CodigoAutorizacion", reciboDTO.CodigoAutorizacion);
                        oCmd.Parameters.AddWithValue("@Usuario", reciboDTO.Usuario);

                        //Ejecutamos el comando y retornamos el id generado
                        await oCmd.ExecuteScalarAsync();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el registro: " + ex.Message);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
        }

        public async Task<int> spCheque(ComprobantesChequeDTO reciboDTO)
        {
            try
            {
                using (var oCnn = factoryConnection.GetConnection())
                {
                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        //asignamos la conexion de trabajo
                        oCmd.Connection = oCnn;

                        //utilizamos stored procedures
                        oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //el indicamos cual stored procedure utilizar
                        oCmd.CommandText = "ComprobantesTmpInsertarCheque";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@ComprobanteId", reciboDTO.ComprobanteId);
                        oCmd.Parameters.AddWithValue("@FormaPagoId", reciboDTO.FormaPagoId);
                        oCmd.Parameters.AddWithValue("@Importe", reciboDTO.Importe);
                        oCmd.Parameters.AddWithValue("@ChequeBancoId", reciboDTO.ChequeBancoId);
                        oCmd.Parameters.AddWithValue("@ChequeNumero", reciboDTO.ChequeNumero);
                        oCmd.Parameters.AddWithValue("@ChequeFechaEmision", reciboDTO.ChequeFechaEmision);
                        oCmd.Parameters.AddWithValue("@ChequeFechaVencimiento", reciboDTO.ChequeFechaVencimiento);
                        oCmd.Parameters.AddWithValue("@ChequeCuit", reciboDTO.ChequeCuit);
                        oCmd.Parameters.AddWithValue("@ChequeNombre", reciboDTO.ChequeNombre);
                        oCmd.Parameters.AddWithValue("@ChequeCuenta", reciboDTO.ChequeCuenta);
                        oCmd.Parameters.AddWithValue("@Observaciones", reciboDTO.Observaciones);
                        oCmd.Parameters.AddWithValue("@Usuario", reciboDTO.Usuario);

                        //Ejecutamos el comando y retornamos el id generado
                        await oCmd.ExecuteScalarAsync();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el registro: " + ex.Message);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
        }

        public async Task<int> spDeleteFormaPago(string id)
        {
            try
            {
                using (var oCnn = factoryConnection.GetConnection())
                {
                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        //asignamos la conexion de trabajo
                        oCmd.Connection = oCnn;

                        //utilizamos stored procedures
                        oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //el indicamos cual stored procedure utilizar
                        oCmd.CommandText = "ComprobantesTmpDeleteFormaPago";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@id", id);
                        
                        //Ejecutamos el comando y retornamos el id generado
                        await oCmd.ExecuteScalarAsync();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el registro: " + ex.Message);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
        }

        public async Task<List<ComprobantesFormasPagosDTO>> spComprobantesTmpFormasPagos(string comprobanteId)
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
                    oCmd.CommandText = "ComprobantesTMPReciboGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@ComprobanteId", comprobanteId);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ComprobantesFormasPagosDTO>();

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
                                var obj = new ComprobantesFormasPagosDTO();
                                obj.Id = oReader["Id"] as string;
                                //obj.ClienteId = oReader["ClienteId"] as string;
                                obj.ComprobanteId = oReader["ComprobanteId"] as string;
                                obj.TipoComprobanteId = oReader["TipoComprobanteId"] as string;
                                obj.FormaPagoId = oReader["FormaPagoId"] as string;
                                obj.FormaPagoCodigo = oReader["FormaPagoCodigo"] as string;
                                obj.FormaPagoTipo = oReader["FormaPagoTipo"] as string;
                                obj.FormaPago = oReader["FormaPago"] as string;
                                obj.Importe = (decimal)oReader["Importe"];
                                obj.Cuota = (int)(oReader["Cuota"] ?? 0);
                                obj.Interes = (decimal)oReader["Interes"];
                                obj.Total = (decimal)oReader["Total"];
                                obj.TarjetaId = oReader["TarjetaId"] as string;
                                obj.TarjetaNombre = oReader["TarjetaNombre"] as string;
                                obj.TarjetaCliente = oReader["TarjetaCliente"] as string;
                                obj.TarjetaNumero = oReader["TarjetaNumero"] as string;

                                if (!DBNull.Value.Equals(oReader["TarjetaVenceMes"]))
                                    obj.TarjetaVenceMes = (int)(oReader["TarjetaVenceMes"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["TarjetaVenceAño"]))
                                    obj.TarjetaVenceAño = (int)(oReader["TarjetaVenceAño"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["TarjetaCodigoSeguridad"]))
                                    obj.TarjetaCodigoSeguridad = (int)(oReader["TarjetaCodigoSeguridad"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["TarjetaEsDebito"]))
                                    obj.TarjetaEsDebito = (bool)(oReader["TarjetaEsDebito"]);

                                obj.ChequeBancoId = oReader["ChequeBancoId"] as string;
                                obj.ChequeBanco = oReader["ChequeBanco"] as string;
                                obj.ChequeNumero = oReader["ChequeNumero"] as string;

                                if (!DBNull.Value.Equals(oReader["ChequeFechaEmision"]))
                                    obj.ChequeFechaEmision = (DateTime)(oReader["ChequeFechaEmision"] ?? DateTime.Now);

                                if (!DBNull.Value.Equals(oReader["ChequeFechaEmision"]))
                                    obj.ChequeFechaVencimiento = (DateTime)(oReader["ChequeFechaEmision"] ?? DateTime.Now); 
                                
                                obj.ChequeCuit = oReader["ChequeCuit"] as string;
                                obj.ChequeNombre = oReader["ChequeNombre"] as string;
                                obj.ChequeCuenta = oReader["ChequeCuenta"] as string;
                                obj.Otros = oReader["Otros"] as string;
                                obj.Observaciones = oReader["Observaciones"] as string;
                                obj.CodigoAutorizacion = oReader["CodigoAutorizacion"] as string;

                                if (!DBNull.Value.Equals(oReader["DolarImporte"]))
                                    obj.DolarImporte = (decimal)(oReader["DolarImporte"]);
                                if (!DBNull.Value.Equals(oReader["DolarCotizacion"]))
                                    obj.DolarCotizacion = (decimal)(oReader["DolarCotizacion"]);

                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;

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

        public async Task<List<ComprobantesFormasPagosDTO>> spComprobantesFormasPagos(string Id)
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
                    oCmd.CommandText = "ComprobanteFormaPagoGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Id", Id);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ComprobantesFormasPagosDTO>();

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
                                var obj = new ComprobantesFormasPagosDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.ComprobanteId = oReader["ComprobanteId"] as string;
                                //obj.TipoComprobanteId = oReader["TipoComprobanteId"] as string;
                                obj.FormaPagoId = oReader["FormaPagoId"] as string;
                                obj.FormaPagoCodigo = oReader["FormaPagoCodigo"] as string;
                                obj.FormaPagoTipo = oReader["FormaPagoTipo"] as string;
                                obj.FormaPago = oReader["FormaPago"] as string;
                                if (!DBNull.Value.Equals(oReader["Importe"]))
                                    obj.Importe = (decimal)oReader["Importe"];

                                obj.Cuota = (int)(oReader["Cuota"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["Interes"]))
                                    obj.Interes = (decimal)oReader["Interes"];

                                
                                if (!DBNull.Value.Equals(oReader["Total"]))
                                    obj.Total = (decimal)oReader["Total"];

                                obj.TarjetaId = oReader["TarjetaId"] as string;
                                obj.TarjetaNombre = oReader["TarjetaNombre"] as string;
                                obj.TarjetaCliente = oReader["TarjetaCliente"] as string;
                                obj.TarjetaNumero = oReader["TarjetaNumero"] as string;

                                if (!DBNull.Value.Equals(oReader["TarjetaVenceMes"]))
                                    obj.TarjetaVenceMes = (int)(oReader["TarjetaVenceMes"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["TarjetaVenceAño"]))
                                    obj.TarjetaVenceAño = (int)(oReader["TarjetaVenceAño"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["TarjetaCodigoSeguridad"]))
                                    obj.TarjetaCodigoSeguridad = (int)(oReader["TarjetaCodigoSeguridad"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["TarjetaEsDebito"]))
                                    obj.TarjetaEsDebito = (bool)(oReader["TarjetaEsDebito"]);

                                obj.ChequeBancoId = oReader["ChequeBancoId"] as string;
                                obj.ChequeBanco = oReader["ChequeBanco"] as string;
                                obj.ChequeNumero = oReader["ChequeNumero"] as string;

                                if (!DBNull.Value.Equals(oReader["ChequeFechaEmision"]))
                                    obj.ChequeFechaEmision = (DateTime)(oReader["ChequeFechaEmision"] ?? DateTime.Now);

                                if (!DBNull.Value.Equals(oReader["ChequeFechaEmision"]))
                                    obj.ChequeFechaVencimiento = (DateTime)(oReader["ChequeFechaEmision"] ?? DateTime.Now);

                                obj.ChequeCuit = oReader["ChequeCuit"] as string;
                                obj.ChequeNombre = oReader["ChequeNombre"] as string;
                                obj.ChequeCuenta = oReader["ChequeCuenta"] as string;
                                obj.Otros = oReader["Otros"] as string;
                                obj.Observaciones = oReader["Observaciones"] as string;
                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;

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

        public async Task<List<ComprobantesFormasPagosDTO>> spComprobantesFormasPagosPresupuesto(string presupuestoId)
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
                    oCmd.CommandText = "ComprobanteFormaPagoPresupuestoGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@PresupuestoId", presupuestoId);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ComprobantesFormasPagosDTO>();

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
                                var obj = new ComprobantesFormasPagosDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.ComprobanteId = oReader["ComprobanteId"] as string;
                                //obj.TipoComprobanteId = oReader["TipoComprobanteId"] as string;
                                obj.FormaPagoId = oReader["FormaPagoId"] as string;
                                obj.FormaPagoCodigo = oReader["FormaPagoCodigo"] as string;
                                obj.FormaPagoTipo = oReader["FormaPagoTipo"] as string;
                                obj.FormaPago = oReader["FormaPago"] as string;
                                if (!DBNull.Value.Equals(oReader["Importe"]))
                                    obj.Importe = (decimal)oReader["Importe"];

                                obj.Cuota = (int)(oReader["Cuota"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["Interes"]))
                                    obj.Interes = (decimal)oReader["Interes"];
                                if (!DBNull.Value.Equals(oReader["Descuento"]))
                                    obj.Descuento = (decimal)oReader["Descuento"];

                                if (!DBNull.Value.Equals(oReader["Total"]))
                                    obj.Total = (decimal)oReader["Total"];

                                obj.TarjetaId = oReader["TarjetaId"] as string;
                                obj.TarjetaNombre = oReader["TarjetaNombre"] as string;
                                obj.TarjetaCliente = oReader["TarjetaCliente"] as string;
                                obj.TarjetaNumero = oReader["TarjetaNumero"] as string;

                                if (!DBNull.Value.Equals(oReader["TarjetaVenceMes"]))
                                    obj.TarjetaVenceMes = (int)(oReader["TarjetaVenceMes"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["TarjetaVenceAño"]))
                                    obj.TarjetaVenceAño = (int)(oReader["TarjetaVenceAño"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["TarjetaCodigoSeguridad"]))
                                    obj.TarjetaCodigoSeguridad = (int)(oReader["TarjetaCodigoSeguridad"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["TarjetaEsDebito"]))
                                    obj.TarjetaEsDebito = (bool)(oReader["TarjetaEsDebito"]);

                                obj.ChequeBancoId = oReader["ChequeBancoId"] as string;
                                obj.ChequeBanco = oReader["ChequeBanco"] as string;
                                obj.ChequeNumero = oReader["ChequeNumero"] as string;

                                if (!DBNull.Value.Equals(oReader["ChequeFechaEmision"]))
                                    obj.ChequeFechaEmision = (DateTime)(oReader["ChequeFechaEmision"] ?? DateTime.Now);

                                if (!DBNull.Value.Equals(oReader["ChequeFechaEmision"]))
                                    obj.ChequeFechaVencimiento = (DateTime)(oReader["ChequeFechaEmision"] ?? DateTime.Now);

                                obj.ChequeCuit = oReader["ChequeCuit"] as string;
                                obj.ChequeNombre = oReader["ChequeNombre"] as string;
                                obj.ChequeCuenta = oReader["ChequeCuenta"] as string;
                                obj.Otros = oReader["Otros"] as string;
                                obj.CodigoAutorizacion = oReader["CodigoAutorizacion"] as string;
                                obj.Observaciones = oReader["Observaciones"] as string;
                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;

                                if (!DBNull.Value.Equals(oReader["DolarCotizacion"]))
                                    obj.DolarCotizacion = (decimal)oReader["DolarCotizacion"];

                                if (!DBNull.Value.Equals(oReader["DolarImporte"]))
                                    obj.DolarImporte = (decimal)oReader["DolarImporte"];


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

        public async Task<ComprobantesDTO> spComprobante(string Id)
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
                    oCmd.CommandText = "ComprobanteGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Id", Id);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var obj = new ComprobantesDTO();

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
                                obj.Id = oReader["Id"] as string;
                                obj.Codigo = oReader["Codigo"] as string;

                                obj.TipoComprobanteId = oReader["TipoComprobanteId"] as string;
                                obj.TipoComprobante = oReader["TipoComprobante"] as string;
                                obj.TipoComprobanteCodigo = oReader["TipoComprobanteCodigo"] as string;

                                obj.PresupuestoId = oReader["PresupuestoId"] as string;
                                obj.VentaRapidaId = oReader["VentaRapidaId"] as string;
                                obj.Letra = oReader["Letra"] as string;
                                obj.PtoVenta = (int)(oReader["PtoVenta"] ?? 0);
                                obj.Numero = (decimal)(oReader["Numero"] ?? 0);
                                obj.FechaComprobante = (DateTime)oReader["FechaComprobante"];
                                obj.ConceptoIncluidoId = oReader["ConceptoIncluidoId"] as string;
                                obj.ConceptoIncluidoCodigo = oReader["ConceptoIncluidoCodigo"] as string;
                                obj.ConceptoIncluido = oReader["ConceptoIncluido"] as string;

                                if (!DBNull.Value.Equals(oReader["PeriodoFacturadoDesde"]))
                                    obj.PeriodoFacturadoDesde = (DateTime)(oReader["PeriodoFacturadoDesde"] ?? DateTime.Now);
                                if (!DBNull.Value.Equals(oReader["PeriodoFacturadoDesde"]))
                                    obj.PeriodoFacturadoHasta = (DateTime)(oReader["PeriodoFacturadoDesde"] ?? DateTime.Now);
                                if (!DBNull.Value.Equals(oReader["FechaVencimiento"]))
                                    obj.FechaVencimiento = (DateTime)(oReader["FechaVencimiento"] ?? DateTime.Now);

                                obj.TipoResponsableId = oReader["TipoResponsableId"] as string;
                                obj.TipoResponsableCodigo = oReader["TipoResponsableCodigo"] as string;
                                obj.TipoResponsable = oReader["TipoResponsable"] as string;

                                obj.ClienteId = oReader["ClienteId"] as string;
                                obj.ClienteCodigo = oReader["ClienteCodigo"] as string;
                                obj.TipoDocumentoId = oReader["TipoDocumentoId"] as string;
                                obj.TipoDocumentoCodigo = oReader["TipoDocumentoCodigo"] as string;
                                obj.TipoDocumento = oReader["TipoDocumento"] as string;
                                obj.NroDocumento = oReader["NroDocumento"] as string;
                                obj.CuilCuit = oReader["CuilCuit"] as string;
                                obj.RazonSocial = oReader["RazonSocial"] as string;

                                obj.ProvinciaId = oReader["ProvinciaId"] as string;
                                obj.ProvinciaCodigo = oReader["ProvinciaCodigo"] as string;
                                obj.Provincia = oReader["Provincia"] as string;

                                obj.Localidad = oReader["Localidad"] as string;
                                obj.CodigoPostal = oReader["CodigoPostal"] as string;
                                obj.Calle = oReader["Calle"] as string;
                                obj.CalleNro = oReader["CalleNro"] as string;
                                obj.PisoDpto = oReader["PisoDpto"] as string;
                                obj.OtrasReferencias = oReader["OtrasReferencias"] as string;

                                obj.Email = oReader["Email"] as string;
                                obj.Telefono = oReader["Telefono"] as string;
                                obj.Celular = oReader["Celular"] as string;

                                obj.Total = (decimal)(oReader["Total"] ?? 0);
                                obj.TotalSinImpuesto = (decimal)(oReader["TotalSinImpuesto"] ?? 0);
                                obj.TotalSinDescuento = (decimal)(oReader["TotalSinDescuento"] ?? 0);
                                obj.TotalSinImpuestoSinDescuento = (decimal)(oReader["TotalSinImpuestoSinDescuento"] ?? 0);
                                obj.DescuentoPorcentaje = (decimal)(oReader["DescuentoPorcentaje"] ?? 0);
                                obj.DescuentoTotal = (decimal)(oReader["DescuentoTotal"] ?? 0);
                                obj.DescuentoSinImpuesto = (decimal)(oReader["DescuentoSinImpuesto"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["ImporteTributos"]))
                                    obj.ImporteTributos = (decimal)(oReader["ImporteTributos"] ?? 0);

                                obj.Observaciones = oReader["Observaciones"] as string;

                                obj.TipoComprobanteAnulaId = oReader["TipoComprobanteAnulaId"] as string;
                                obj.TipoComprobanteAnula = oReader["TipoComprobanteAnula"] as string;
                                obj.TipoComprobanteAnulaCodigo = oReader["TipoComprobanteAnulaCodigo"] as string;

                                obj.LetraAnula = oReader["LetraAnula"] as string;
                                if (!DBNull.Value.Equals(oReader["PtoVtaAnula"]))
                                    obj.PtoVtaAnula = (int)oReader["PtoVtaAnula"];

                                if (!DBNull.Value.Equals(oReader["NumeroAnula"]))
                                    obj.NumeroAnula = (decimal)(oReader["NumeroAnula"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["FechaAnulacion"]))
                                    obj.FechaAnulacion = (DateTime)oReader["FechaAnulacion"];

                                if (!DBNull.Value.Equals(oReader["Anulado"]))
                                    obj.Anulado = (bool)oReader["Anulado"];
                                obj.CodigoAnula = oReader["CodigoAnula"] as string;
                                obj.UsuarioAnula = oReader["UsuarioAnula"] as string;

                                if (!DBNull.Value.Equals(oReader["Saldo"]))
                                    obj.Saldo = (decimal)(oReader["Saldo"] ?? 0);

                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;

                                obj.SucursalNombre = oReader["SucursalNombre"] as string;
                                obj.SucursalCalle = oReader["SucursalCalle"] as string;
                                obj.SucursalCalleNro = oReader["SucursalCalleNro"] as string;
                                obj.SucursalLocalidad = oReader["SucursalLocalidad"] as string;
                                obj.SucursalCodigoPostal = oReader["SucursalCodigoPostal"] as string;
                                obj.SucursalTelefono = oReader["SucursalTelefono"] as string;

                                obj.CodigoPresupuesto = oReader["CodigoPresupuesto"] as string;

                                obj.TipoComprobanteFiscal = oReader["TipoComprobanteFiscal"] as string;
                                obj.LetraFiscal = oReader["LetraFiscal"] as string;
                                if (!DBNull.Value.Equals(oReader["PtoVentaFiscal"]))
                                    obj.PtoVentaFiscal = (int)(oReader["PtoVentaFiscal"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["NumeroFiscal"]))
                                    obj.NumeroFiscal = (decimal)(oReader["NumeroFiscal"] ?? 0);

                                
                            }
                        }
                        //retornamos los valores encontrados


                        return obj;
                    }

                    finally
                    {
                        //el Finally nos da siempre la oportunidad de liberar
                        //la memoria utilizada por los objetos 
                        obj = null;
                    }
                }
            }
        }

        public async Task<List<ComprobantesDTO>> spComprobantesPresupuesto(string PresupuestoId)
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
                    oCmd.CommandText = "ComprobantesPresupuestoGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Id", PresupuestoId);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ComprobantesDTO>();

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
                                var obj = new ComprobantesDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.Codigo = oReader["Codigo"] as string;

                                obj.TipoComprobanteId = oReader["TipoComprobanteId"] as string;
                                obj.TipoComprobante = oReader["TipoComprobante"] as string;
                                obj.TipoComprobanteCodigo = oReader["TipoComprobanteCodigo"] as string;

                                obj.PresupuestoId = oReader["PresupuestoId"] as string;
                                obj.Letra = oReader["Letra"] as string;
                                obj.PtoVenta = (int)(oReader["PtoVenta"] ?? 0);
                                obj.Numero = (decimal)(oReader["Numero"] ?? 0);
                                obj.FechaComprobante = (DateTime)oReader["FechaComprobante"];
                                obj.ConceptoIncluidoId = oReader["ConceptoIncluidoId"] as string;
                                obj.ConceptoIncluidoCodigo = oReader["ConceptoIncluidoCodigo"] as string;
                                obj.ConceptoIncluido = oReader["ConceptoIncluido"] as string;

                                if (!DBNull.Value.Equals(oReader["PeriodoFacturadoDesde"]))
                                    obj.PeriodoFacturadoDesde = (DateTime)(oReader["PeriodoFacturadoDesde"] ?? DateTime.Now);
                                if (!DBNull.Value.Equals(oReader["PeriodoFacturadoDesde"]))
                                    obj.PeriodoFacturadoHasta = (DateTime)(oReader["PeriodoFacturadoDesde"] ?? DateTime.Now);
                                if (!DBNull.Value.Equals(oReader["FechaVencimiento"]))
                                    obj.FechaVencimiento = (DateTime)(oReader["FechaVencimiento"] ?? DateTime.Now);

                                obj.TipoResponsableId = oReader["TipoResponsableId"] as string;
                                obj.TipoResponsableCodigo = oReader["TipoResponsableCodigo"] as string;
                                obj.TipoResponsable = oReader["TipoResponsable"] as string;

                                obj.ClienteId = oReader["ClienteId"] as string;
                                obj.ClienteCodigo = oReader["ClienteCodigo"] as string;
                                obj.TipoDocumentoId = oReader["TipoDocumentoId"] as string;
                                obj.TipoDocumentoCodigo = oReader["TipoDocumentoCodigo"] as string;
                                obj.TipoDocumento = oReader["TipoDocumento"] as string;
                                obj.NroDocumento = oReader["NroDocumento"] as string;
                                obj.CuilCuit = oReader["CuilCuit"] as string;
                                obj.RazonSocial = oReader["RazonSocial"] as string;

                                obj.ProvinciaId = oReader["ProvinciaId"] as string;
                                obj.ProvinciaCodigo = oReader["ProvinciaCodigo"] as string;
                                obj.Provincia = oReader["Provincia"] as string;

                                obj.Localidad = oReader["Localidad"] as string;
                                obj.CodigoPostal = oReader["CodigoPostal"] as string;
                                obj.Calle = oReader["Calle"] as string;
                                obj.CalleNro = oReader["CalleNro"] as string;
                                obj.PisoDpto = oReader["PisoDpto"] as string;
                                obj.OtrasReferencias = oReader["OtrasReferencias"] as string;

                                obj.Email = oReader["Email"] as string;
                                obj.Telefono = oReader["Telefono"] as string;
                                obj.Celular = oReader["Celular"] as string;

                                obj.Total = (decimal)(oReader["Total"] ?? 0);
                                obj.TotalSinImpuesto = (decimal)(oReader["TotalSinImpuesto"] ?? 0);
                                obj.TotalSinDescuento = (decimal)(oReader["TotalSinDescuento"] ?? 0);
                                obj.TotalSinImpuestoSinDescuento = (decimal)(oReader["TotalSinImpuestoSinDescuento"] ?? 0);
                                obj.DescuentoPorcentaje = (decimal)(oReader["DescuentoPorcentaje"] ?? 0);
                                obj.DescuentoTotal = (decimal)(oReader["DescuentoTotal"] ?? 0);
                                obj.DescuentoSinImpuesto = (decimal)(oReader["DescuentoSinImpuesto"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["ImporteTributos"]))
                                    obj.ImporteTributos = (decimal)(oReader["ImporteTributos"] ?? 0);

                                obj.Observaciones = oReader["Observaciones"] as string;

                                obj.TipoComprobanteAnulaId = oReader["TipoComprobanteAnulaId"] as string;
                                obj.TipoComprobanteAnula = oReader["TipoComprobanteAnula"] as string;
                                obj.TipoComprobanteAnulaCodigo = oReader["TipoComprobanteAnulaCodigo"] as string;

                                obj.LetraAnula = oReader["LetraAnula"] as string;
                                if (!DBNull.Value.Equals(oReader["PtoVtaAnula"]))
                                    obj.PtoVtaAnula = (int)oReader["PtoVtaAnula"];

                                if (!DBNull.Value.Equals(oReader["NumeroAnula"]))
                                    obj.NumeroAnula = (decimal)(oReader["NumeroAnula"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["FechaAnulacion"]))
                                    obj.FechaAnulacion = (DateTime)oReader["FechaAnulacion"];

                                if (!DBNull.Value.Equals(oReader["Anulado"]))
                                    obj.Anulado = (bool)oReader["Anulado"];
                                obj.CodigoAnula = oReader["CodigoAnula"] as string;
                                obj.UsuarioAnula = oReader["UsuarioAnula"] as string;

                                if (!DBNull.Value.Equals(oReader["Saldo"]))
                                    obj.Saldo = (decimal)(oReader["Saldo"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["SaldoContado"]))
                                    obj.SaldoContado = (decimal)(oReader["SaldoContado"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["Saldo_Porcentaje"]))
                                    obj.Saldo_Porcentaje = (decimal)(oReader["Saldo_Porcentaje"] ?? 0);

                                obj.TipoComprobanteFiscal = oReader["TipoComprobanteFiscal"] as string;
                                obj.LetraFiscal = oReader["LetraFiscal"] as string;
                                if (!DBNull.Value.Equals(oReader["PtoVentaFiscal"]))
                                    obj.PtoVentaFiscal = (int)(oReader["PtoVentaFiscal"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["NumeroFiscal"]))
                                    obj.NumeroFiscal = (decimal)(oReader["NumeroFiscal"] ?? 0);

                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;
                                obj.CodigoPresupuesto = oReader["CodigoPresupuesto"] as string;

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

        public async Task<List<Comprobantes>> spComprobanteImputacion(string Id)
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
                    oCmd.CommandText = "ComprobanteImputacionGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Id", Id);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<Comprobantes>();

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
                                var obj = new Comprobantes();
                                obj.Id = oReader["Id"] as string;
                                obj.Codigo = oReader["Codigo"] as string;

                                obj.TipoComprobanteId = oReader["TipoComprobanteId"] as string;
                                obj.TipoComprobante = oReader["TipoComprobante"] as string;
                                obj.TipoComprobanteCodigo = oReader["TipoComprobanteCodigo"] as string;

                                obj.PresupuestoId = oReader["PresupuestoId"] as string;
                                obj.Letra = oReader["Letra"] as string;
                                obj.PtoVenta = (int)(oReader["PtoVenta"] ?? 0);
                                obj.Numero = (decimal)(oReader["Numero"] ?? 0);
                                obj.FechaComprobante = (DateTime)oReader["FechaComprobante"];
                                obj.ConceptoIncluidoId = oReader["ConceptoIncluidoId"] as string;
                                obj.ConceptoIncluidoCodigo = oReader["ConceptoIncluidoCodigo"] as string;
                                obj.ConceptoIncluido = oReader["ConceptoIncluido"] as string;

                                if (!DBNull.Value.Equals(oReader["PeriodoFacturadoDesde"]))
                                    obj.PeriodoFacturadoDesde = (DateTime)(oReader["PeriodoFacturadoDesde"] ?? DateTime.Now);
                                if (!DBNull.Value.Equals(oReader["PeriodoFacturadoDesde"]))
                                    obj.PeriodoFacturadoHasta = (DateTime)(oReader["PeriodoFacturadoDesde"] ?? DateTime.Now);
                                if (!DBNull.Value.Equals(oReader["FechaVencimiento"]))
                                    obj.FechaVencimiento = (DateTime)(oReader["FechaVencimiento"] ?? DateTime.Now);

                                obj.TipoResponsableId = oReader["TipoResponsableId"] as string;
                                obj.TipoResponsableCodigo = oReader["TipoResponsableCodigo"] as string;
                                obj.TipoResponsable = oReader["TipoResponsable"] as string;

                                obj.ClienteId = oReader["ClienteId"] as string;
                                obj.ClienteCodigo = oReader["ClienteCodigo"] as string;
                                obj.TipoDocumentoId = oReader["TipoDocumentoId"] as string;
                                obj.TipoDocumentoCodigo = oReader["TipoDocumentoCodigo"] as string;
                                obj.TipoDocumento = oReader["TipoDocumento"] as string;
                                obj.NroDocumento = oReader["NroDocumento"] as string;
                                obj.CuilCuit = oReader["CuilCuit"] as string;
                                obj.RazonSocial = oReader["RazonSocial"] as string;

                                obj.ProvinciaId = oReader["ProvinciaId"] as string;
                                obj.ProvinciaCodigo = oReader["ProvinciaCodigo"] as string;
                                obj.Provincia = oReader["Provincia"] as string;

                                obj.Localidad = oReader["Localidad"] as string;
                                obj.CodigoPostal = oReader["CodigoPostal"] as string;
                                obj.Calle = oReader["Calle"] as string;
                                obj.CalleNro = oReader["CalleNro"] as string;
                                obj.PisoDpto = oReader["PisoDpto"] as string;
                                obj.OtrasReferencias = oReader["OtrasReferencias"] as string;

                                obj.Email = oReader["Email"] as string;
                                obj.Telefono = oReader["Telefono"] as string;
                                obj.Celular = oReader["Celular"] as string;

                                obj.Total = (decimal)(oReader["Total"] ?? 0);
                                obj.TotalSinImpuesto = (decimal)(oReader["TotalSinImpuesto"] ?? 0);
                                obj.TotalSinDescuento = (decimal)(oReader["TotalSinDescuento"] ?? 0);
                                obj.TotalSinImpuestoSinDescuento = (decimal)(oReader["TotalSinImpuestoSinDescuento"] ?? 0);
                                obj.DescuentoPorcentaje = (decimal)(oReader["DescuentoPorcentaje"] ?? 0);
                                obj.DescuentoTotal = (decimal)(oReader["DescuentoTotal"] ?? 0);
                                obj.DescuentoSinImpuesto = (decimal)(oReader["DescuentoSinImpuesto"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["ImporteTributos"]))
                                    obj.ImporteTributos = (decimal)(oReader["ImporteTributos"] ?? 0);

                                obj.Observaciones = oReader["Observaciones"] as string;

                                obj.TipoComprobanteAnulaId = oReader["TipoComprobanteAnulaId"] as string;
                                obj.TipoComprobanteAnula = oReader["TipoComprobanteAnula"] as string;
                                obj.TipoComprobanteAnulaCodigo = oReader["TipoComprobanteAnulaCodigo"] as string;

                                obj.LetraAnula = oReader["LetraAnula"] as string;
                                if (!DBNull.Value.Equals(oReader["PtoVtaAnula"]))
                                    obj.PtoVtaAnula = (int)oReader["PtoVtaAnula"];

                                if (!DBNull.Value.Equals(oReader["NumeroAnula"]))
                                    obj.NumeroAnula = (decimal)(oReader["NumeroAnula"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["FechaAnulacion"]))
                                    obj.FechaAnulacion = (DateTime)oReader["FechaAnulacion"];
                                
                                if (!DBNull.Value.Equals(oReader["Anulado"]))
                                    obj.Anulado = (bool)oReader["Anulado"];
                                obj.CodigoAnula = oReader["CodigoAnula"] as string;
                                obj.UsuarioAnula = oReader["UsuarioAnula"] as string;
                                
                                if (!DBNull.Value.Equals(oReader["Saldo"]))
                                    obj.Saldo = (decimal)(oReader["Saldo"] ?? 0);

                                obj.TipoComprobanteFiscal = oReader["TipoComprobanteFiscal"] as string;
                                obj.LetraFiscal = oReader["LetraFiscal"] as string;
                                if (!DBNull.Value.Equals(oReader["PtoVentaFiscal"]))
                                    obj.PtoVentaFiscal = (int)(oReader["PtoVentaFiscal"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["NumeroFiscal"]))
                                    obj.NumeroFiscal = (decimal)(oReader["NumeroFiscal"] ?? 0);

                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;
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

        public async Task<List<ComprobantesDTO>> spComprobantesPresupuestos()
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
                    oCmd.CommandText = "ComprobantesPrespuestosGet";

                    
                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ComprobantesDTO>();

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
                                var obj = new ComprobantesDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.Codigo = oReader["Codigo"] as string;

                                obj.TipoComprobanteId = oReader["TipoComprobanteId"] as string;
                                obj.TipoComprobante = oReader["TipoComprobante"] as string;
                                obj.TipoComprobanteCodigo = oReader["TipoComprobanteCodigo"] as string;

                                obj.PresupuestoId = oReader["PresupuestoId"] as string;
                                obj.Letra = oReader["Letra"] as string;
                                obj.PtoVenta = (int)(oReader["PtoVenta"] ?? 0);
                                obj.Numero = (decimal)(oReader["Numero"] ?? 0);
                                obj.FechaComprobante = (DateTime)oReader["FechaComprobante"];
                                obj.ConceptoIncluidoId = oReader["ConceptoIncluidoId"] as string;
                                obj.ConceptoIncluidoCodigo = oReader["ConceptoIncluidoCodigo"] as string;
                                obj.ConceptoIncluido = oReader["ConceptoIncluido"] as string;

                                if (!DBNull.Value.Equals(oReader["PeriodoFacturadoDesde"]))
                                    obj.PeriodoFacturadoDesde = (DateTime)(oReader["PeriodoFacturadoDesde"] ?? DateTime.Now);
                                if (!DBNull.Value.Equals(oReader["PeriodoFacturadoDesde"]))
                                    obj.PeriodoFacturadoHasta = (DateTime)(oReader["PeriodoFacturadoDesde"] ?? DateTime.Now);
                                if (!DBNull.Value.Equals(oReader["FechaVencimiento"]))
                                    obj.FechaVencimiento = (DateTime)(oReader["FechaVencimiento"] ?? DateTime.Now);

                                obj.TipoResponsableId = oReader["TipoResponsableId"] as string;
                                obj.TipoResponsableCodigo = oReader["TipoResponsableCodigo"] as string;
                                obj.TipoResponsable = oReader["TipoResponsable"] as string;

                                obj.ClienteId = oReader["ClienteId"] as string;
                                obj.ClienteCodigo = oReader["ClienteCodigo"] as string;
                                obj.TipoDocumentoId = oReader["TipoDocumentoId"] as string;
                                obj.TipoDocumentoCodigo = oReader["TipoDocumentoCodigo"] as string;
                                obj.TipoDocumento = oReader["TipoDocumento"] as string;
                                obj.NroDocumento = oReader["NroDocumento"] as string;
                                obj.CuilCuit = oReader["CuilCuit"] as string;
                                obj.RazonSocial = oReader["RazonSocial"] as string;

                                obj.ProvinciaId = oReader["ProvinciaId"] as string;
                                obj.ProvinciaCodigo = oReader["ProvinciaCodigo"] as string;
                                obj.Provincia = oReader["Provincia"] as string;

                                obj.Localidad = oReader["Localidad"] as string;
                                obj.CodigoPostal = oReader["CodigoPostal"] as string;
                                obj.Calle = oReader["Calle"] as string;
                                obj.CalleNro = oReader["CalleNro"] as string;
                                obj.PisoDpto = oReader["PisoDpto"] as string;
                                obj.OtrasReferencias = oReader["OtrasReferencias"] as string;

                                obj.Email = oReader["Email"] as string;
                                obj.Telefono = oReader["Telefono"] as string;
                                obj.Celular = oReader["Celular"] as string;

                                obj.Total = (decimal)(oReader["Total"] ?? 0);
                                obj.TotalSinImpuesto = (decimal)(oReader["TotalSinImpuesto"] ?? 0);
                                obj.TotalSinDescuento = (decimal)(oReader["TotalSinDescuento"] ?? 0);
                                obj.TotalSinImpuestoSinDescuento = (decimal)(oReader["TotalSinImpuestoSinDescuento"] ?? 0);
                                obj.DescuentoPorcentaje = (decimal)(oReader["DescuentoPorcentaje"] ?? 0);
                                obj.DescuentoTotal = (decimal)(oReader["DescuentoTotal"] ?? 0);
                                obj.DescuentoSinImpuesto = (decimal)(oReader["DescuentoSinImpuesto"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["ImporteTributos"]))
                                    obj.ImporteTributos = (decimal)(oReader["ImporteTributos"] ?? 0);

                                obj.Observaciones = oReader["Observaciones"] as string;

                                obj.TipoComprobanteAnulaId = oReader["TipoComprobanteAnulaId"] as string;
                                obj.TipoComprobanteAnula = oReader["TipoComprobanteAnula"] as string;
                                obj.TipoComprobanteAnulaCodigo = oReader["TipoComprobanteAnulaCodigo"] as string;

                                obj.LetraAnula = oReader["LetraAnula"] as string;
                                if (!DBNull.Value.Equals(oReader["PtoVtaAnula"]))
                                    obj.PtoVtaAnula = (int)oReader["PtoVtaAnula"];

                                if (!DBNull.Value.Equals(oReader["NumeroAnula"]))
                                    obj.NumeroAnula = (decimal)(oReader["NumeroAnula"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["FechaAnulacion"]))
                                    obj.FechaAnulacion = (DateTime)oReader["FechaAnulacion"];

                                if (!DBNull.Value.Equals(oReader["Saldo"]))
                                    obj.Saldo = (decimal)(oReader["Saldo"] ?? 0);

                                
                                if (!DBNull.Value.Equals(oReader["Saldo_Porcentaje"]))
                                    obj.Saldo_Porcentaje = (decimal)(oReader["Saldo_Porcentaje"] ?? 0);

                                obj.TipoComprobanteFiscal = oReader["TipoComprobanteFiscal"] as string;
                                obj.LetraFiscal = oReader["LetraFiscal"] as string;
                                if (!DBNull.Value.Equals(oReader["PtoVentaFiscal"]))
                                    obj.PtoVentaFiscal = (int)(oReader["PtoVentaFiscal"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["NumeroFiscal"]))
                                    obj.NumeroFiscal = (decimal)(oReader["NumeroFiscal"] ?? 0);

                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;

                                obj.CodigoPresupuesto = oReader["CodigoPresupuesto"] as string;
                                

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
        public async Task<List<ComprobantesDTO>> spComprobantesImputaciones()
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
                    oCmd.CommandText = "ComprobantesImputacionesGet";


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ComprobantesDTO>();

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
                                var obj = new ComprobantesDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.Codigo = oReader["Codigo"] as string;

                                obj.TipoComprobanteId = oReader["TipoComprobanteId"] as string;
                                obj.TipoComprobante = oReader["TipoComprobante"] as string;
                                obj.TipoComprobanteCodigo = oReader["TipoComprobanteCodigo"] as string;

                                obj.PresupuestoId = oReader["PresupuestoId"] as string;
                                obj.Letra = oReader["Letra"] as string;
                                obj.PtoVenta = (int)(oReader["PtoVenta"] ?? 0);
                                obj.Numero = (decimal)(oReader["Numero"] ?? 0);
                                obj.FechaComprobante = (DateTime)oReader["FechaComprobante"];
                                obj.ConceptoIncluidoId = oReader["ConceptoIncluidoId"] as string;
                                obj.ConceptoIncluidoCodigo = oReader["ConceptoIncluidoCodigo"] as string;
                                obj.ConceptoIncluido = oReader["ConceptoIncluido"] as string;

                                if (!DBNull.Value.Equals(oReader["PeriodoFacturadoDesde"]))
                                    obj.PeriodoFacturadoDesde = (DateTime)(oReader["PeriodoFacturadoDesde"] ?? DateTime.Now);
                                if (!DBNull.Value.Equals(oReader["PeriodoFacturadoDesde"]))
                                    obj.PeriodoFacturadoHasta = (DateTime)(oReader["PeriodoFacturadoDesde"] ?? DateTime.Now);
                                if (!DBNull.Value.Equals(oReader["FechaVencimiento"]))
                                    obj.FechaVencimiento = (DateTime)(oReader["FechaVencimiento"] ?? DateTime.Now);

                                obj.TipoResponsableId = oReader["TipoResponsableId"] as string;
                                obj.TipoResponsableCodigo = oReader["TipoResponsableCodigo"] as string;
                                obj.TipoResponsable = oReader["TipoResponsable"] as string;

                                obj.ClienteId = oReader["ClienteId"] as string;
                                obj.ClienteCodigo = oReader["ClienteCodigo"] as string;
                                obj.TipoDocumentoId = oReader["TipoDocumentoId"] as string;
                                obj.TipoDocumentoCodigo = oReader["TipoDocumentoCodigo"] as string;
                                obj.TipoDocumento = oReader["TipoDocumento"] as string;
                                obj.NroDocumento = oReader["NroDocumento"] as string;
                                obj.CuilCuit = oReader["CuilCuit"] as string;
                                obj.RazonSocial = oReader["RazonSocial"] as string;

                                obj.ProvinciaId = oReader["ProvinciaId"] as string;
                                obj.ProvinciaCodigo = oReader["ProvinciaCodigo"] as string;
                                obj.Provincia = oReader["Provincia"] as string;

                                obj.Localidad = oReader["Localidad"] as string;
                                obj.CodigoPostal = oReader["CodigoPostal"] as string;
                                obj.Calle = oReader["Calle"] as string;
                                obj.CalleNro = oReader["CalleNro"] as string;
                                obj.PisoDpto = oReader["PisoDpto"] as string;
                                obj.OtrasReferencias = oReader["OtrasReferencias"] as string;

                                obj.Email = oReader["Email"] as string;
                                obj.Telefono = oReader["Telefono"] as string;
                                obj.Celular = oReader["Celular"] as string;

                                obj.Total = (decimal)(oReader["Total"] ?? 0);
                                obj.TotalSinImpuesto = (decimal)(oReader["TotalSinImpuesto"] ?? 0);
                                obj.TotalSinDescuento = (decimal)(oReader["TotalSinDescuento"] ?? 0);
                                obj.TotalSinImpuestoSinDescuento = (decimal)(oReader["TotalSinImpuestoSinDescuento"] ?? 0);
                                obj.DescuentoPorcentaje = (decimal)(oReader["DescuentoPorcentaje"] ?? 0);
                                obj.DescuentoTotal = (decimal)(oReader["DescuentoTotal"] ?? 0);
                                obj.DescuentoSinImpuesto = (decimal)(oReader["DescuentoSinImpuesto"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["ImporteTributos"]))
                                    obj.ImporteTributos = (decimal)(oReader["ImporteTributos"] ?? 0);

                                obj.Observaciones = oReader["Observaciones"] as string;

                                obj.TipoComprobanteAnulaId = oReader["TipoComprobanteAnulaId"] as string;
                                obj.TipoComprobanteAnula = oReader["TipoComprobanteAnula"] as string;
                                obj.TipoComprobanteAnulaCodigo = oReader["TipoComprobanteAnulaCodigo"] as string;

                                obj.LetraAnula = oReader["LetraAnula"] as string;
                                if (!DBNull.Value.Equals(oReader["PtoVtaAnula"]))
                                    obj.PtoVtaAnula = (int)oReader["PtoVtaAnula"];

                                if (!DBNull.Value.Equals(oReader["NumeroAnula"]))
                                    obj.NumeroAnula = (decimal)(oReader["NumeroAnula"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["FechaAnulacion"]))
                                    obj.FechaAnulacion = (DateTime)oReader["FechaAnulacion"];

                                if (!DBNull.Value.Equals(oReader["Saldo"]))
                                    obj.Saldo = (decimal)(oReader["Saldo"] ?? 0);


                                if (!DBNull.Value.Equals(oReader["Saldo_Porcentaje"]))
                                    obj.Saldo_Porcentaje = (decimal)(oReader["Saldo_Porcentaje"] ?? 0);

                                obj.TipoComprobanteFiscal = oReader["TipoComprobanteFiscal"] as string;
                                obj.LetraFiscal = oReader["LetraFiscal"] as string;
                                if (!DBNull.Value.Equals(oReader["PtoVentaFiscal"]))
                                    obj.PtoVentaFiscal = (int)(oReader["PtoVentaFiscal"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["NumeroFiscal"]))
                                    obj.NumeroFiscal = (decimal)(oReader["NumeroFiscal"] ?? 0);

                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;

                                obj.CodigoPresupuesto = oReader["CodigoPresupuesto"] as string;


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

        public async Task<List<ComprobantesDTO>> spComprobantesPresupuestosCliente(string clienteId)
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
                    oCmd.CommandText = "ComprobantesPrespuestosClienteGet";
                    
                    oCmd.Parameters.AddWithValue("@ClienteId", clienteId);

                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ComprobantesDTO>();

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
                                var obj = new ComprobantesDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.Codigo = oReader["Codigo"] as string;

                                obj.TipoComprobanteId = oReader["TipoComprobanteId"] as string;
                                obj.TipoComprobante = oReader["TipoComprobante"] as string;
                                obj.TipoComprobanteCodigo = oReader["TipoComprobanteCodigo"] as string;

                                obj.PresupuestoId = oReader["PresupuestoId"] as string;
                                obj.Letra = oReader["Letra"] as string;
                                obj.PtoVenta = (int)(oReader["PtoVenta"] ?? 0);
                                obj.Numero = (decimal)(oReader["Numero"] ?? 0);
                                obj.FechaComprobante = (DateTime)oReader["FechaComprobante"];
                                obj.ConceptoIncluidoId = oReader["ConceptoIncluidoId"] as string;
                                obj.ConceptoIncluidoCodigo = oReader["ConceptoIncluidoCodigo"] as string;
                                obj.ConceptoIncluido = oReader["ConceptoIncluido"] as string;

                                if (!DBNull.Value.Equals(oReader["PeriodoFacturadoDesde"]))
                                    obj.PeriodoFacturadoDesde = (DateTime)(oReader["PeriodoFacturadoDesde"] ?? DateTime.Now);
                                if (!DBNull.Value.Equals(oReader["PeriodoFacturadoDesde"]))
                                    obj.PeriodoFacturadoHasta = (DateTime)(oReader["PeriodoFacturadoDesde"] ?? DateTime.Now);
                                if (!DBNull.Value.Equals(oReader["FechaVencimiento"]))
                                    obj.FechaVencimiento = (DateTime)(oReader["FechaVencimiento"] ?? DateTime.Now);

                                obj.TipoResponsableId = oReader["TipoResponsableId"] as string;
                                obj.TipoResponsableCodigo = oReader["TipoResponsableCodigo"] as string;
                                obj.TipoResponsable = oReader["TipoResponsable"] as string;

                                obj.ClienteId = oReader["ClienteId"] as string;
                                obj.ClienteCodigo = oReader["ClienteCodigo"] as string;
                                obj.TipoDocumentoId = oReader["TipoDocumentoId"] as string;
                                obj.TipoDocumentoCodigo = oReader["TipoDocumentoCodigo"] as string;
                                obj.TipoDocumento = oReader["TipoDocumento"] as string;
                                obj.NroDocumento = oReader["NroDocumento"] as string;
                                obj.CuilCuit = oReader["CuilCuit"] as string;
                                obj.RazonSocial = oReader["RazonSocial"] as string;

                                obj.ProvinciaId = oReader["ProvinciaId"] as string;
                                obj.ProvinciaCodigo = oReader["ProvinciaCodigo"] as string;
                                obj.Provincia = oReader["Provincia"] as string;

                                obj.Localidad = oReader["Localidad"] as string;
                                obj.CodigoPostal = oReader["CodigoPostal"] as string;
                                obj.Calle = oReader["Calle"] as string;
                                obj.CalleNro = oReader["CalleNro"] as string;
                                obj.PisoDpto = oReader["PisoDpto"] as string;
                                obj.OtrasReferencias = oReader["OtrasReferencias"] as string;

                                obj.Email = oReader["Email"] as string;
                                obj.Telefono = oReader["Telefono"] as string;
                                obj.Celular = oReader["Celular"] as string;

                                obj.Total = (decimal)(oReader["Total"] ?? 0);
                                obj.TotalSinImpuesto = (decimal)(oReader["TotalSinImpuesto"] ?? 0);
                                obj.TotalSinDescuento = (decimal)(oReader["TotalSinDescuento"] ?? 0);
                                obj.TotalSinImpuestoSinDescuento = (decimal)(oReader["TotalSinImpuestoSinDescuento"] ?? 0);
                                obj.DescuentoPorcentaje = (decimal)(oReader["DescuentoPorcentaje"] ?? 0);
                                obj.DescuentoTotal = (decimal)(oReader["DescuentoTotal"] ?? 0);
                                obj.DescuentoSinImpuesto = (decimal)(oReader["DescuentoSinImpuesto"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["ImporteTributos"]))
                                    obj.ImporteTributos = (decimal)(oReader["ImporteTributos"] ?? 0);

                                obj.Observaciones = oReader["Observaciones"] as string;

                                obj.TipoComprobanteAnulaId = oReader["TipoComprobanteAnulaId"] as string;
                                obj.TipoComprobanteAnula = oReader["TipoComprobanteAnula"] as string;
                                obj.TipoComprobanteAnulaCodigo = oReader["TipoComprobanteAnulaCodigo"] as string;

                                obj.LetraAnula = oReader["LetraAnula"] as string;
                                if (!DBNull.Value.Equals(oReader["PtoVtaAnula"]))
                                    obj.PtoVtaAnula = (int)oReader["PtoVtaAnula"];

                                if (!DBNull.Value.Equals(oReader["NumeroAnula"]))
                                    obj.NumeroAnula = (decimal)(oReader["NumeroAnula"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["FechaAnulacion"]))
                                    obj.FechaAnulacion = (DateTime)oReader["FechaAnulacion"];

                                if (!DBNull.Value.Equals(oReader["Saldo"]))
                                    obj.Saldo = (decimal)(oReader["Saldo"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["Saldo_Porcentaje"]))
                                    obj.Saldo_Porcentaje = (decimal)(oReader["Saldo_Porcentaje"] ?? 0);

                                obj.TipoComprobanteFiscal = oReader["TipoComprobanteFiscal"] as string;
                                obj.LetraFiscal = oReader["LetraFiscal"] as string;
                                if (!DBNull.Value.Equals(oReader["PtoVentaFiscal"]))
                                    obj.PtoVentaFiscal = (int)(oReader["PtoVentaFiscal"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["NumeroFiscal"]))
                                    obj.NumeroFiscal = (decimal)(oReader["NumeroFiscal"] ?? 0);

                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;

                                obj.CodigoPresupuesto = oReader["CodigoPresupuesto"] as string;


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

        public async Task<List<ComprobantesFormasPagosDTO>> spComprobantesFormasPagosImprimir(string Id)
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
                    oCmd.CommandText = "ComprobanteFormaPagoGetImprimir";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Id", Id);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ComprobantesFormasPagosDTO>();

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
                                var obj = new ComprobantesFormasPagosDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.ComprobanteId = oReader["ComprobanteId"] as string;
                                //obj.TipoComprobanteId = oReader["TipoComprobanteId"] as string;
                                obj.FormaPagoId = oReader["FormaPagoId"] as string;
                                obj.FormaPagoCodigo = oReader["FormaPagoCodigo"] as string;
                                obj.FormaPagoTipo = oReader["FormaPagoTipo"] as string;
                                obj.FormaPago = oReader["FormaPago"] as string;
                                obj.Importe = (decimal)oReader["Importe"];
                                obj.Cuota = (int)(oReader["Cuota"] ?? 0);
                                obj.Interes = (decimal)oReader["Interes"];
                                obj.Total = (decimal)oReader["Total"];
                                obj.TarjetaId = oReader["TarjetaId"] as string;
                                obj.TarjetaNombre = oReader["TarjetaNombre"] as string;
                                obj.TarjetaCliente = oReader["TarjetaCliente"] as string;
                                obj.TarjetaNumero = oReader["TarjetaNumero"] as string;

                                if (!DBNull.Value.Equals(oReader["TarjetaVenceMes"]))
                                    obj.TarjetaVenceMes = (int)(oReader["TarjetaVenceMes"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["TarjetaVenceAño"]))
                                    obj.TarjetaVenceAño = (int)(oReader["TarjetaVenceAño"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["TarjetaCodigoSeguridad"]))
                                    obj.TarjetaCodigoSeguridad = (int)(oReader["TarjetaCodigoSeguridad"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["TarjetaEsDebito"]))
                                    obj.TarjetaEsDebito = (bool)(oReader["TarjetaEsDebito"]);

                                obj.ChequeBancoId = oReader["ChequeBancoId"] as string;
                                obj.ChequeBanco = oReader["ChequeBanco"] as string;
                                obj.ChequeNumero = oReader["ChequeNumero"] as string;

                                if (!DBNull.Value.Equals(oReader["ChequeFechaEmision"]))
                                    obj.ChequeFechaEmision = (DateTime)(oReader["ChequeFechaEmision"] ?? DateTime.Now);

                                if (!DBNull.Value.Equals(oReader["ChequeFechaEmision"]))
                                    obj.ChequeFechaVencimiento = (DateTime)(oReader["ChequeFechaEmision"] ?? DateTime.Now);

                                obj.ChequeCuit = oReader["ChequeCuit"] as string;
                                obj.ChequeNombre = oReader["ChequeNombre"] as string;
                                obj.ChequeCuenta = oReader["ChequeCuenta"] as string;
                                obj.Otros = oReader["Otros"] as string;
                                obj.Observaciones = oReader["Observaciones"] as string;

                                obj.CodigoAutorizacion = oReader["CodigoAutorizacion"] as string;

                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;

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

        public async Task<int> spComprobantesReciboAnular(string Id, string Motivo, string Usuario) 
        {
            try
            {
                using (var oCnn = factoryConnection.GetConnection())
                {
                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        //asignamos la conexion de trabajo
                        oCmd.Connection = oCnn;

                        //utilizamos stored procedures
                        oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //el indicamos cual stored procedure utilizar
                        oCmd.CommandText = "ComprobantesInsertarReciboAnular";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@ComprobanteId", Id);
                        oCmd.Parameters.AddWithValue("@Observaciones", Motivo);
                        oCmd.Parameters.AddWithValue("@Usuario", Usuario);

                        //Ejecutamos el comando y retornamos el id generado
                        await oCmd.ExecuteScalarAsync();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el registro: " + ex.Message);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
        }

        public async Task<List<ComprobantesDetalleImputaDTO>> spComprobanteDetalleImputacion(string Id)
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
                    oCmd.CommandText = "ComprobanteImputaEntregaGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@ComprobanteId", Id);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ComprobantesDetalleImputaDTO>();

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
                                var obj = new ComprobantesDetalleImputaDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.ComprobanteId = oReader["ComprobanteId"] as string;
                                obj.DetalleId = oReader["ComprobanteId"] as string;
                                obj.ProductoId = oReader["ProductoId"] as string;
                                obj.ProductoNombre = oReader["ProductoNombre"] as string;
                                obj.ProductoCodigo = oReader["ProductoCodigo"] as string;

                                obj.Cantidad = (int)(oReader["Cantidad"] ?? 0);
                                
                                //obj.PrecioUnitario = (decimal)(oReader["PrecioUnitario"] ?? 0);
                                obj.Precio = (decimal)(oReader["Precio"] ?? 0);

                                obj.Imputado = (decimal)(oReader["ImporteImputado"] ?? 0);
                                obj.ImputadoPorcentaje = (decimal)(oReader["PorcentajeImputado"] ?? 0);

                                obj.Estado = (bool)oReader["Estado"];
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.Usuario = oReader["Usuario"] as string;

                                if (!DBNull.Value.Equals(oReader["Entrega"]))
                                    obj.EntregaEstado = (bool)(oReader["Entrega"] ?? false);
                                
                                if (!DBNull.Value.Equals(oReader["FechaEntrega"]))
                                    obj.EntregaFecha = (DateTime)(oReader["FechaEntrega"] ?? DateTime.Now);
                                
                                obj.EntregaUsuario = oReader["UsuarioEntrega"] as string;

                                if (!DBNull.Value.Equals(oReader["AutorizaEntrega"]))
                                    obj.AutorizaEstado = (bool)(oReader["AutorizaEntrega"] ?? false);

                                if (!DBNull.Value.Equals(oReader["FechaAutoriza"]))
                                    obj.AutorizaFecha = (DateTime)(oReader["FechaAutoriza"] ?? DateTime.Now);
                                
                                obj.AutorizaUsuario = oReader["UsuarioDespacha"] as string;


                                if (!DBNull.Value.Equals(oReader["Despacha"]))
                                    obj.DespachaEstado = (bool)oReader["Despacha"];

                                if (!DBNull.Value.Equals(oReader["FechaDespacha"]))
                                    obj.DespachaFecha = (DateTime)oReader["FechaDespacha"];

                                obj.DespachaUsuario = oReader["UsuarioDespacha"] as string;

                                if (!DBNull.Value.Equals(oReader["Devolucion"])) 
                                    obj.DevolucionEstado = (bool)oReader["Devolucion"];

                                if (!DBNull.Value.Equals(oReader["FechaDevolucion"]))
                                    obj.DevolucionFecha = (DateTime)oReader["FechaDevolucion"];
                                
                                obj.DevolucionUsuario = oReader["UsuarioDevolucion"] as string;
                                obj.DevolucionMotivo = oReader["MotivoDevolucion"] as string;

                                obj.Porcentaje_Config = (decimal)oReader["Porcentaje_Config"];
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

        public async Task<List<ComprobantesDetalleDTO>> spComprobanteDetalleImputa(string ProductoId)
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
                    oCmd.CommandText = "ComprobanteImputaEntregaProductoGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@ProductoId", ProductoId);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ComprobantesDetalleDTO>();

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
                                var obj = new ComprobantesDetalleDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.ComprobanteId = oReader["ComprobanteId"] as string;

                                obj.ProductoId = oReader["ProductoId"] as string;
                                obj.ProductoNombre = oReader["ProductoNombre"] as string;
                                obj.ProductoCodigo = oReader["ProductoCodigo"] as string;

                                obj.Cantidad = (int)(oReader["Cantidad"] ?? 0);

                                //obj.PrecioUnitario = (decimal)(oReader["PrecioUnitario"] ?? 0);
                                //obj.Precio = (decimal)(oReader["Precio"] ?? 0);

                                //obj.Imputado = (decimal)(oReader["Imputado"] ?? 0);
                                //obj.ImputadoPorcentaje = (decimal)(oReader["ImputadoPorcentaje"] ?? 0);

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

        public async Task<int> spComprobanteDetalleInsertImputacion(ComprobantesDetalleDTO detalleDTO)
        {
            try
            {
                using (var oCnn = factoryConnection.GetConnection())
                {
                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        //asignamos la conexion de trabajo
                        oCmd.Connection = oCnn;

                        //utilizamos stored procedures
                        oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //el indicamos cual stored procedure utilizar
                        oCmd.CommandText = "ComprobantesDetalleImputar";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", detalleDTO.Id);
                        oCmd.Parameters.AddWithValue("@Cantidad", detalleDTO.Cantidad);
                        oCmd.Parameters.AddWithValue("@ImporteImputado", detalleDTO.Precio);
                        oCmd.Parameters.AddWithValue("@Usuario", detalleDTO.UsuarioAlta);
                        
                        //Ejecutamos el comando y retornamos el id generado
                        await oCmd.ExecuteScalarAsync();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el registro: " + ex.Message);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
        }

        public async Task<int> spComprobanteDetalleInsertEntrega(ComprobantesDetalleDTO detalleDTO)
        {
            try
            {
                using (var oCnn = factoryConnection.GetConnection())
                {
                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        //asignamos la conexion de trabajo
                        oCmd.Connection = oCnn;

                        //utilizamos stored procedures
                        oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //el indicamos cual stored procedure utilizar
                        oCmd.CommandText = "ComprobantesDetalleEntregar";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", detalleDTO.Id);
                        oCmd.Parameters.AddWithValue("@Usuario", detalleDTO.UsuarioAlta);

                        //Ejecutamos el comando y retornamos el id generado
                        await oCmd.ExecuteScalarAsync();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el registro: " + ex.Message);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
        }

        public async Task<int> spComprobanteDetalleInsertEntregaAnula(ComprobantesDetalleDTO detalleDTO)
        {
            try
            {
                using (var oCnn = factoryConnection.GetConnection())
                {
                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        //asignamos la conexion de trabajo
                        oCmd.Connection = oCnn;

                        //utilizamos stored procedures
                        oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //el indicamos cual stored procedure utilizar
                        oCmd.CommandText = "ComprobantesDetalleEntregarAnula";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", detalleDTO.Id);
                        oCmd.Parameters.AddWithValue("@Usuario", detalleDTO.UsuarioAlta);

                        //Ejecutamos el comando y retornamos el id generado
                        await oCmd.ExecuteScalarAsync();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el registro: " + ex.Message);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
        }

        public async Task<int> spComprobanteDetalleInsertAutoriza(ComprobantesDetalleDTO detalleDTO)
        {
            try
            {
                using (var oCnn = factoryConnection.GetConnection())
                {
                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        //asignamos la conexion de trabajo
                        oCmd.Connection = oCnn;

                        //utilizamos stored procedures
                        oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //el indicamos cual stored procedure utilizar
                        oCmd.CommandText = "ComprobantesDetalleAutoriza";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", detalleDTO.Id);
                        oCmd.Parameters.AddWithValue("@Usuario", detalleDTO.UsuarioAlta);

                        //Ejecutamos el comando y retornamos el id generado
                        await oCmd.ExecuteScalarAsync();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el registro: " + ex.Message);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
        }

        public async Task<int> spComprobanteDetalleInsertAutorizaAnula(ComprobantesDetalleDTO detalleDTO)
        {
            try
            {
                using (var oCnn = factoryConnection.GetConnection())
                {
                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        //asignamos la conexion de trabajo
                        oCmd.Connection = oCnn;

                        //utilizamos stored procedures
                        oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //el indicamos cual stored procedure utilizar
                        oCmd.CommandText = "ComprobantesDetalleAutorizaAnula";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", detalleDTO.Id);
                        oCmd.Parameters.AddWithValue("@Usuario", detalleDTO.UsuarioAlta);

                        //Ejecutamos el comando y retornamos el id generado
                        await oCmd.ExecuteScalarAsync();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el registro: " + ex.Message);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
        }

        public async Task<List<ComprobantesDTO>> spPresupuestosComprobantesEntrega()
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
                    oCmd.CommandText = "ComprobantesPrespuestosEntregaGet";


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ComprobantesDTO>();

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
                                var obj = new ComprobantesDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.Codigo = oReader["Codigo"] as string;

                                obj.TipoComprobanteId = oReader["TipoComprobanteId"] as string;
                                obj.TipoComprobante = oReader["TipoComprobante"] as string;
                                obj.TipoComprobanteCodigo = oReader["TipoComprobanteCodigo"] as string;

                                obj.PresupuestoId = oReader["PresupuestoId"] as string;
                                obj.Letra = oReader["Letra"] as string;
                                obj.PtoVenta = (int)(oReader["PtoVenta"] ?? 0);
                                obj.Numero = (decimal)(oReader["Numero"] ?? 0);
                                obj.FechaComprobante = (DateTime)oReader["FechaComprobante"];
                                obj.ConceptoIncluidoId = oReader["ConceptoIncluidoId"] as string;
                                obj.ConceptoIncluidoCodigo = oReader["ConceptoIncluidoCodigo"] as string;
                                obj.ConceptoIncluido = oReader["ConceptoIncluido"] as string;

                                if (!DBNull.Value.Equals(oReader["PeriodoFacturadoDesde"]))
                                    obj.PeriodoFacturadoDesde = (DateTime)(oReader["PeriodoFacturadoDesde"] ?? DateTime.Now);
                                if (!DBNull.Value.Equals(oReader["PeriodoFacturadoDesde"]))
                                    obj.PeriodoFacturadoHasta = (DateTime)(oReader["PeriodoFacturadoDesde"] ?? DateTime.Now);
                                if (!DBNull.Value.Equals(oReader["FechaVencimiento"]))
                                    obj.FechaVencimiento = (DateTime)(oReader["FechaVencimiento"] ?? DateTime.Now);

                                obj.TipoResponsableId = oReader["TipoResponsableId"] as string;
                                obj.TipoResponsableCodigo = oReader["TipoResponsableCodigo"] as string;
                                obj.TipoResponsable = oReader["TipoResponsable"] as string;

                                obj.ClienteId = oReader["ClienteId"] as string;
                                obj.ClienteCodigo = oReader["ClienteCodigo"] as string;
                                obj.TipoDocumentoId = oReader["TipoDocumentoId"] as string;
                                obj.TipoDocumentoCodigo = oReader["TipoDocumentoCodigo"] as string;
                                obj.TipoDocumento = oReader["TipoDocumento"] as string;
                                obj.NroDocumento = oReader["NroDocumento"] as string;
                                obj.CuilCuit = oReader["CuilCuit"] as string;
                                obj.RazonSocial = oReader["RazonSocial"] as string;

                                obj.ProvinciaId = oReader["ProvinciaId"] as string;
                                obj.ProvinciaCodigo = oReader["ProvinciaCodigo"] as string;
                                obj.Provincia = oReader["Provincia"] as string;

                                obj.Localidad = oReader["Localidad"] as string;
                                obj.CodigoPostal = oReader["CodigoPostal"] as string;
                                obj.Calle = oReader["Calle"] as string;
                                obj.CalleNro = oReader["CalleNro"] as string;
                                obj.PisoDpto = oReader["PisoDpto"] as string;
                                obj.OtrasReferencias = oReader["OtrasReferencias"] as string;

                                obj.Email = oReader["Email"] as string;
                                obj.Telefono = oReader["Telefono"] as string;
                                obj.Celular = oReader["Celular"] as string;

                                obj.Total = (decimal)(oReader["Total"] ?? 0);
                                obj.TotalSinImpuesto = (decimal)(oReader["TotalSinImpuesto"] ?? 0);
                                obj.TotalSinDescuento = (decimal)(oReader["TotalSinDescuento"] ?? 0);
                                obj.TotalSinImpuestoSinDescuento = (decimal)(oReader["TotalSinImpuestoSinDescuento"] ?? 0);
                                obj.DescuentoPorcentaje = (decimal)(oReader["DescuentoPorcentaje"] ?? 0);
                                obj.DescuentoTotal = (decimal)(oReader["DescuentoTotal"] ?? 0);
                                obj.DescuentoSinImpuesto = (decimal)(oReader["DescuentoSinImpuesto"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["ImporteTributos"]))
                                    obj.ImporteTributos = (decimal)(oReader["ImporteTributos"] ?? 0);

                                obj.Observaciones = oReader["Observaciones"] as string;

                                obj.TipoComprobanteAnulaId = oReader["TipoComprobanteAnulaId"] as string;
                                obj.TipoComprobanteAnula = oReader["TipoComprobanteAnula"] as string;
                                obj.TipoComprobanteAnulaCodigo = oReader["TipoComprobanteAnulaCodigo"] as string;

                                obj.LetraAnula = oReader["LetraAnula"] as string;
                                if (!DBNull.Value.Equals(oReader["PtoVtaAnula"]))
                                    obj.PtoVtaAnula = (int)oReader["PtoVtaAnula"];

                                if (!DBNull.Value.Equals(oReader["NumeroAnula"]))
                                    obj.NumeroAnula = (decimal)(oReader["NumeroAnula"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["FechaAnulacion"]))
                                    obj.FechaAnulacion = (DateTime)oReader["FechaAnulacion"];

                                if (!DBNull.Value.Equals(oReader["Saldo"]))
                                    obj.Saldo = (decimal)(oReader["Saldo"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["Saldo_Porcentaje"]))
                                    obj.Saldo_Porcentaje = (decimal)(oReader["Saldo_Porcentaje"] ?? 0);

                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;
                                obj.CodigoPresupuesto = oReader["CodigoPresupuesto"] as string;

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

        public async Task<List<ComprobantesDTO>> spPresupuestosComprobantesDevolucion()
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
                    oCmd.CommandText = "ComprobantesPrespuestosDevolucionGet";


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ComprobantesDTO>();

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
                                var obj = new ComprobantesDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.Codigo = oReader["Codigo"] as string;

                                obj.TipoComprobanteId = oReader["TipoComprobanteId"] as string;
                                obj.TipoComprobante = oReader["TipoComprobante"] as string;
                                obj.TipoComprobanteCodigo = oReader["TipoComprobanteCodigo"] as string;

                                obj.PresupuestoId = oReader["PresupuestoId"] as string;
                                obj.Letra = oReader["Letra"] as string;
                                obj.PtoVenta = (int)(oReader["PtoVenta"] ?? 0);
                                obj.Numero = (decimal)(oReader["Numero"] ?? 0);
                                obj.FechaComprobante = (DateTime)oReader["FechaComprobante"];
                                obj.ConceptoIncluidoId = oReader["ConceptoIncluidoId"] as string;
                                obj.ConceptoIncluidoCodigo = oReader["ConceptoIncluidoCodigo"] as string;
                                obj.ConceptoIncluido = oReader["ConceptoIncluido"] as string;

                                if (!DBNull.Value.Equals(oReader["PeriodoFacturadoDesde"]))
                                    obj.PeriodoFacturadoDesde = (DateTime)(oReader["PeriodoFacturadoDesde"] ?? DateTime.Now);
                                if (!DBNull.Value.Equals(oReader["PeriodoFacturadoDesde"]))
                                    obj.PeriodoFacturadoHasta = (DateTime)(oReader["PeriodoFacturadoDesde"] ?? DateTime.Now);
                                if (!DBNull.Value.Equals(oReader["FechaVencimiento"]))
                                    obj.FechaVencimiento = (DateTime)(oReader["FechaVencimiento"] ?? DateTime.Now);

                                obj.TipoResponsableId = oReader["TipoResponsableId"] as string;
                                obj.TipoResponsableCodigo = oReader["TipoResponsableCodigo"] as string;
                                obj.TipoResponsable = oReader["TipoResponsable"] as string;

                                obj.ClienteId = oReader["ClienteId"] as string;
                                obj.ClienteCodigo = oReader["ClienteCodigo"] as string;
                                obj.TipoDocumentoId = oReader["TipoDocumentoId"] as string;
                                obj.TipoDocumentoCodigo = oReader["TipoDocumentoCodigo"] as string;
                                obj.TipoDocumento = oReader["TipoDocumento"] as string;
                                obj.NroDocumento = oReader["NroDocumento"] as string;
                                obj.CuilCuit = oReader["CuilCuit"] as string;
                                obj.RazonSocial = oReader["RazonSocial"] as string;

                                obj.ProvinciaId = oReader["ProvinciaId"] as string;
                                obj.ProvinciaCodigo = oReader["ProvinciaCodigo"] as string;
                                obj.Provincia = oReader["Provincia"] as string;

                                obj.Localidad = oReader["Localidad"] as string;
                                obj.CodigoPostal = oReader["CodigoPostal"] as string;
                                obj.Calle = oReader["Calle"] as string;
                                obj.CalleNro = oReader["CalleNro"] as string;
                                obj.PisoDpto = oReader["PisoDpto"] as string;
                                obj.OtrasReferencias = oReader["OtrasReferencias"] as string;

                                obj.Email = oReader["Email"] as string;
                                obj.Telefono = oReader["Telefono"] as string;
                                obj.Celular = oReader["Celular"] as string;

                                obj.Total = (decimal)(oReader["Total"] ?? 0);
                                obj.TotalSinImpuesto = (decimal)(oReader["TotalSinImpuesto"] ?? 0);
                                obj.TotalSinDescuento = (decimal)(oReader["TotalSinDescuento"] ?? 0);
                                obj.TotalSinImpuestoSinDescuento = (decimal)(oReader["TotalSinImpuestoSinDescuento"] ?? 0);
                                obj.DescuentoPorcentaje = (decimal)(oReader["DescuentoPorcentaje"] ?? 0);
                                obj.DescuentoTotal = (decimal)(oReader["DescuentoTotal"] ?? 0);
                                obj.DescuentoSinImpuesto = (decimal)(oReader["DescuentoSinImpuesto"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["ImporteTributos"]))
                                    obj.ImporteTributos = (decimal)(oReader["ImporteTributos"] ?? 0);

                                obj.Observaciones = oReader["Observaciones"] as string;

                                obj.TipoComprobanteAnulaId = oReader["TipoComprobanteAnulaId"] as string;
                                obj.TipoComprobanteAnula = oReader["TipoComprobanteAnula"] as string;
                                obj.TipoComprobanteAnulaCodigo = oReader["TipoComprobanteAnulaCodigo"] as string;

                                obj.LetraAnula = oReader["LetraAnula"] as string;
                                if (!DBNull.Value.Equals(oReader["PtoVtaAnula"]))
                                    obj.PtoVtaAnula = (int)oReader["PtoVtaAnula"];

                                if (!DBNull.Value.Equals(oReader["NumeroAnula"]))
                                    obj.NumeroAnula = (decimal)(oReader["NumeroAnula"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["FechaAnulacion"]))
                                    obj.FechaAnulacion = (DateTime)oReader["FechaAnulacion"];

                                if (!DBNull.Value.Equals(oReader["Saldo"]))
                                    obj.Saldo = (decimal)(oReader["Saldo"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["Saldo_Porcentaje"]))
                                    obj.Saldo_Porcentaje = (decimal)(oReader["Saldo_Porcentaje"] ?? 0);

                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;
                                obj.CodigoPresupuesto = oReader["CodigoPresupuesto"] as string;

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

        public async Task<List<ComprobantesDetalleImputaDTO>> spComprobanteDetalleEntrega(string Id)
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
                    oCmd.CommandText = "ComprobantesDetalleEntregarGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@ComprobanteId", Id);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ComprobantesDetalleImputaDTO>();

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
                                var obj = new ComprobantesDetalleImputaDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.ComprobanteId = oReader["ComprobanteId"] as string;
                                obj.DetalleId = oReader["ComprobanteId"] as string;
                                obj.ProductoId = oReader["ProductoId"] as string;
                                obj.ProductoNombre = oReader["ProductoNombre"] as string;
                                obj.ProductoCodigo = oReader["ProductoCodigo"] as string;

                                obj.Cantidad = (int)(oReader["Cantidad"] ?? 0);

                                //obj.PrecioUnitario = (decimal)(oReader["PrecioUnitario"] ?? 0);
                                obj.Precio = (decimal)(oReader["Precio"] ?? 0);

                                obj.Imputado = (decimal)(oReader["ImporteImputado"] ?? 0);
                                obj.ImputadoPorcentaje = (decimal)(oReader["PorcentajeImputado"] ?? 0);

                                obj.Estado = (bool)oReader["Estado"];
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.Usuario = oReader["Usuario"] as string;

                                if (!DBNull.Value.Equals(oReader["Entrega"]))
                                    obj.EntregaEstado = (bool)(oReader["Entrega"] ?? false);

                                if (!DBNull.Value.Equals(oReader["FechaEntrega"]))
                                    obj.EntregaFecha = (DateTime)(oReader["FechaEntrega"] ?? DateTime.Now);

                                obj.EntregaUsuario = oReader["UsuarioEntrega"] as string;

                                if (!DBNull.Value.Equals(oReader["AutorizaEntrega"]))
                                    obj.AutorizaEstado = (bool)(oReader["AutorizaEntrega"] ?? false);

                                if (!DBNull.Value.Equals(oReader["FechaAutoriza"]))
                                    obj.AutorizaFecha = (DateTime)(oReader["FechaAutoriza"] ?? DateTime.Now);

                                obj.AutorizaUsuario = oReader["UsuarioDespacha"] as string;


                                if (!DBNull.Value.Equals(oReader["Despacha"]))
                                    obj.DespachaEstado = (bool)oReader["Despacha"];

                                if (!DBNull.Value.Equals(oReader["FechaDespacha"]))
                                    obj.DespachaFecha = (DateTime)oReader["FechaDespacha"];

                                obj.DespachaUsuario = oReader["UsuarioDespacha"] as string;

                                if (!DBNull.Value.Equals(oReader["Devolucion"]))
                                    obj.DevolucionEstado = (bool)oReader["Devolucion"];

                                if (!DBNull.Value.Equals(oReader["FechaDevolucion"]))
                                    obj.DevolucionFecha = (DateTime)oReader["FechaDevolucion"];

                                obj.DevolucionUsuario = oReader["UsuarioDevolucion"] as string;
                                obj.DevolucionMotivo = oReader["MotivoDevolucion"] as string;

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

        public async Task<List<ComprobantesDetalleDTO>> spComprobanteDetalleDevolucion(string Id)
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
                    oCmd.CommandText = "ComprobantesDetalleDevolucionGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@ComprobanteId", Id);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ComprobantesDetalleDTO>();

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
                                var obj = new ComprobantesDetalleDTO();
                                //obj.Id = oReader["Id"] as string;
                                //obj.ComprobanteId = oReader["ComprobanteId"] as string;
                                //obj.DetalleId = oReader["ComprobanteId"] as string;
                                //obj.ProductoId = oReader["ProductoId"] as string;
                                //obj.ProductoNombre = oReader["ProductoNombre"] as string;
                                //obj.ProductoCodigo = oReader["ProductoCodigo"] as string;

                                //obj.Cantidad = (int)(oReader["Cantidad"] ?? 0);

                                ////obj.PrecioUnitario = (decimal)(oReader["PrecioUnitario"] ?? 0);
                                //obj.Precio = (decimal)(oReader["Precio"] ?? 0);

                                //obj.Imputado = (decimal)(oReader["ImporteImputado"] ?? 0);
                                //obj.ImputadoPorcentaje = (decimal)(oReader["PorcentajeImputado"] ?? 0);

                                //obj.Estado = (bool)oReader["Estado"];
                                //obj.Fecha = (DateTime)oReader["Fecha"];
                                //obj.Usuario = oReader["Usuario"] as string;

                                //if (!DBNull.Value.Equals(oReader["Entrega"]))
                                //    obj.EntregaEstado = (bool)(oReader["Entrega"] ?? false);

                                //if (!DBNull.Value.Equals(oReader["FechaEntrega"]))
                                //    obj.EntregaFecha = (DateTime)(oReader["FechaEntrega"] ?? DateTime.Now);

                                //obj.EntregaUsuario = oReader["UsuarioEntrega"] as string;

                                //if (!DBNull.Value.Equals(oReader["AutorizaEntrega"]))
                                //    obj.AutorizaEstado = (bool)(oReader["AutorizaEntrega"] ?? false);

                                //if (!DBNull.Value.Equals(oReader["FechaAutoriza"]))
                                //    obj.AutorizaFecha = (DateTime)(oReader["FechaAutoriza"] ?? DateTime.Now);

                                //obj.AutorizaUsuario = oReader["UsuarioDespacha"] as string;


                                //if (!DBNull.Value.Equals(oReader["Despacha"]))
                                //    obj.DespachaEstado = (bool)oReader["Despacha"];

                                //if (!DBNull.Value.Equals(oReader["FechaDespacha"]))
                                //    obj.DespachaFecha = (DateTime)oReader["FechaDespacha"];

                                //obj.DespachaUsuario = oReader["UsuarioDespacha"] as string;

                                //if (!DBNull.Value.Equals(oReader["Devolucion"]))
                                //    obj.DevolucionEstado = (bool)oReader["Devolucion"];

                                //if (!DBNull.Value.Equals(oReader["FechaDevolucion"]))
                                //    obj.DevolucionFecha = (DateTime)oReader["FechaDevolucion"];

                                //obj.DevolucionUsuario = oReader["UsuarioDevolucion"] as string;
                                //obj.DevolucionMotivo = oReader["MotivoDevolucion"] as string;

                                //var obj = new ComprobantesDetalleDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.ComprobanteId = oReader["ComprobanteId"] as string;
                                obj.ProductoId = oReader["ProductoId"] as string;
                                obj.ProductoNombre = oReader["ProductoNombre"] as string;
                                obj.ProductoCodigo = oReader["ProductoCodigo"] as string;
                                obj.Cantidad = 1;
                                //obj.Precio = (decimal)(oReader["Precio"] ?? 0);
                                //obj.PrecioLista = (decimal)(oReader["PrecioLista"] ?? 0);
                                //obj.PrecioSinIva = (decimal)(oReader["PrecioSinIva"] ?? 0);
                                //obj.PrecioListaSinIva = (decimal)(oReader["PrecioListaSinIva"] ?? 0);
                                obj.FechaAlta = (DateTime)(oReader["Fecha"]);
                                obj.UsuarioAlta = oReader["Usuario"] as string;

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

        public async Task<int> spComprobanteDetalleInsertEntregaTMP(ComprobantesDetalleDTO detalleDTO)
        {
            try
            {
                using (var oCnn = factoryConnection.GetConnection())
                {
                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        //asignamos la conexion de trabajo
                        oCmd.Connection = oCnn;

                        //utilizamos stored procedures
                        oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //el indicamos cual stored procedure utilizar
                        oCmd.CommandText = "ComprobantesEntregaInsertarTMP";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", detalleDTO.Id);
                        oCmd.Parameters.AddWithValue("@Usuario", detalleDTO.UsuarioAlta);

                        //Ejecutamos el comando y retornamos el id generado
                        await oCmd.ExecuteScalarAsync();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el registro: " + ex.Message);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
        }

        public async Task<int> spComprobanteDetalleInsertDevolucionTMP(ComprobantesDetalleDTO detalleDTO)
        {
            try
            {
                using (var oCnn = factoryConnection.GetConnection())
                {
                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        //asignamos la conexion de trabajo
                        oCmd.Connection = oCnn;

                        //utilizamos stored procedures
                        oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //el indicamos cual stored procedure utilizar
                        oCmd.CommandText = "ComprobantesDevolucionInsertarTMP";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", detalleDTO.Id);
                        oCmd.Parameters.AddWithValue("@Usuario", detalleDTO.UsuarioAlta);

                        //Ejecutamos el comando y retornamos el id generado
                        await oCmd.ExecuteScalarAsync();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el registro: " + ex.Message);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
        }

        public async Task<List<ComprobantesDetalleImputaDTO>> spComprobanteDetalleEntregaTMP(string Id)
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
                    oCmd.CommandText = "ComprobantesDetalleEntregarGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@ComprobanteId", Id);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ComprobantesDetalleImputaDTO>();

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
                                var obj = new ComprobantesDetalleImputaDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.ComprobanteId = oReader["ComprobanteId"] as string;
                                obj.DetalleId = oReader["ComprobanteId"] as string;
                                obj.ProductoId = oReader["ProductoId"] as string;
                                obj.ProductoNombre = oReader["ProductoNombre"] as string;
                                obj.ProductoCodigo = oReader["ProductoCodigo"] as string;

                                obj.Cantidad = (int)(oReader["Cantidad"] ?? 0);

                                //obj.PrecioUnitario = (decimal)(oReader["PrecioUnitario"] ?? 0);
                                obj.Precio = (decimal)(oReader["Precio"] ?? 0);

                                obj.Imputado = (decimal)(oReader["ImporteImputado"] ?? 0);
                                obj.ImputadoPorcentaje = (decimal)(oReader["PorcentajeImputado"] ?? 0);

                                obj.Estado = (bool)oReader["Estado"];
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.Usuario = oReader["Usuario"] as string;

                                if (!DBNull.Value.Equals(oReader["Entrega"]))
                                    obj.EntregaEstado = (bool)(oReader["Entrega"] ?? false);

                                if (!DBNull.Value.Equals(oReader["FechaEntrega"]))
                                    obj.EntregaFecha = (DateTime)(oReader["FechaEntrega"] ?? DateTime.Now);

                                obj.EntregaUsuario = oReader["UsuarioEntrega"] as string;

                                if (!DBNull.Value.Equals(oReader["AutorizaEntrega"]))
                                    obj.AutorizaEstado = (bool)(oReader["AutorizaEntrega"] ?? false);

                                if (!DBNull.Value.Equals(oReader["FechaAutoriza"]))
                                    obj.AutorizaFecha = (DateTime)(oReader["FechaAutoriza"] ?? DateTime.Now);

                                obj.AutorizaUsuario = oReader["UsuarioDespacha"] as string;


                                if (!DBNull.Value.Equals(oReader["Despacha"]))
                                    obj.DespachaEstado = (bool)oReader["Despacha"];

                                if (!DBNull.Value.Equals(oReader["FechaDespacha"]))
                                    obj.DespachaFecha = (DateTime)oReader["FechaDespacha"];

                                obj.DespachaUsuario = oReader["UsuarioDespacha"] as string;

                                if (!DBNull.Value.Equals(oReader["Devolucion"]))
                                    obj.DevolucionEstado = (bool)oReader["Devolucion"];

                                if (!DBNull.Value.Equals(oReader["FechaDevolucion"]))
                                    obj.DevolucionFecha = (DateTime)oReader["FechaDevolucion"];

                                obj.DevolucionUsuario = oReader["UsuarioDevolucion"] as string;
                                obj.DevolucionMotivo = oReader["MotivoDevolucion"] as string;

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

        public async Task<List<ComprobantesDetalleDTO>> spComprobanteDetalleTMP(string ComprobanteId)
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
                    oCmd.CommandText = "ComprobantesDetalleTMPGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@ComprobanteId", ComprobanteId);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ComprobantesDetalleDTO>();

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
                                var obj = new ComprobantesDetalleDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.ComprobanteId = oReader["ComprobanteId"] as string;
                                obj.ProductoId = oReader["ProductoId"] as string;
                                obj.ProductoNombre = oReader["ProductoNombre"] as string;
                                obj.ProductoCodigo = oReader["ProductoCodigo"] as string;
                                obj.Cantidad = 1;
                                obj.Precio = (decimal)(oReader["Precio"] ?? 0);
                                obj.PrecioSinIva = (decimal)(oReader["PrecioSinIva"] ?? 0);
                                obj.FechaAlta = (DateTime)(oReader["FechaAlta"]);
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;

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

        public async Task<int> spComprobanteDetalleEliminarEntregaTMP(ComprobantesDetalleDTO detalleDTO)
        {
            try
            {
                using (var oCnn = factoryConnection.GetConnection())
                {
                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        //asignamos la conexion de trabajo
                        oCmd.Connection = oCnn;

                        //utilizamos stored procedures
                        oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //el indicamos cual stored procedure utilizar
                        oCmd.CommandText = "ComprobantesEntregaEliminarTMP";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", detalleDTO.Id);
                        oCmd.Parameters.AddWithValue("@Usuario", detalleDTO.UsuarioAlta);

                        //Ejecutamos el comando y retornamos el id generado
                        await oCmd.ExecuteScalarAsync();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el registro: " + ex.Message);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
        }

        public async Task<int> spRemito(ComprobantesDetalleDTO detalleDTO)
        {
            try
            {
                using (var oCnn = factoryConnection.GetConnection())
                {
                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        //asignamos la conexion de trabajo
                        oCmd.Connection = oCnn;

                        //utilizamos stored procedures
                        oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //el indicamos cual stored procedure utilizar
                        oCmd.CommandText = "ComprobantesInsertarRemito";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@ComprobanteId", detalleDTO.ComprobanteId);
                        oCmd.Parameters.AddWithValue("@Usuario", detalleDTO.UsuarioAlta);

                        //Ejecutamos el comando y retornamos el id generado
                        await oCmd.ExecuteScalarAsync();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el registro: " + ex.Message);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
        }

        public async Task<int> spDevolucion(ComprobantesDTO detalleDTO)
        {
            try
            {
                using (var oCnn = factoryConnection.GetConnection())
                {
                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        //asignamos la conexion de trabajo
                        oCmd.Connection = oCnn;

                        //utilizamos stored procedures
                        oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //el indicamos cual stored procedure utilizar
                        oCmd.CommandText = "ComprobantesInsertarDevolucion";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@ComprobanteId", detalleDTO.Id);
                        oCmd.Parameters.AddWithValue("@Observaciones", detalleDTO.Observaciones);
                        oCmd.Parameters.AddWithValue("@Usuario", detalleDTO.UsuarioAlta);

                        //Ejecutamos el comando y retornamos el id generado
                        await oCmd.ExecuteScalarAsync();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el registro: " + ex.Message);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
        }

        public async Task<List<ComprobantesDetalleDTO>> spComprobanteDetalleGet(string ComprobanteId)
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
                    oCmd.CommandText = "ComprobanteDetalleGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@ComprobanteId", ComprobanteId);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ComprobantesDetalleDTO>();

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
                                var obj = new ComprobantesDetalleDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.ComprobanteId = oReader["ComprobanteId"] as string;
                                obj.ProductoId = oReader["ProductoId"] as string;
                                obj.ProductoNombre = oReader["ProductoNombre"] as string;
                                obj.ProductoCodigo = oReader["ProductoCodigo"] as string;
                                obj.Cantidad = 1;                                
                                obj.Precio = (decimal)(oReader["Precio"] ?? 0);
                                obj.PrecioSinIva = (decimal)(oReader["PrecioSinIva"] ?? 0);
                                obj.FechaAlta = (DateTime)(oReader["Fecha"]);
                                obj.UsuarioAlta = oReader["Usuario"] as string;

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

        public async Task<List<ComprobantesDetalleDTO>> spComprobanteDetallePresupuestoGet(string presupuestoId)
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
                    oCmd.CommandText = "ComprobantesDetallePresupuestoGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@PresupuestoId", presupuestoId);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ComprobantesDetalleDTO>();

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
                                var obj = new ComprobantesDetalleDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.ComprobanteId = oReader["ComprobanteId"] as string;
                                obj.ProductoId = oReader["ProductoId"] as string;
                                obj.ProductoNombre = oReader["ProductoNombre"] as string;
                                obj.ProductoCodigo = oReader["ProductoCodigo"] as string;
                                obj.Cantidad = (int)oReader["Cantidad"]; 
                                obj.Precio = (decimal)(oReader["Precio"] ?? 0);
                                obj.PrecioSinIva = (decimal)(oReader["PrecioSinIva"] ?? 0);
                                obj.FechaAlta = (DateTime)(oReader["FechaAlta"]);
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;

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

        public async Task<List<ComprobantesDetalleDTO>> spComprobanteRemitosDevolucionesGet(string presupuestoId)
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
                    oCmd.CommandText = "ComprobantesRemitosDevolucionesGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@PresupuestoId", presupuestoId);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ComprobantesDetalleDTO>();

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
                                var obj = new ComprobantesDetalleDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.ComprobanteId = oReader["ComprobanteId"] as string;
                                obj.ProductoId = oReader["ProductoId"] as string;
                                obj.ProductoNombre = oReader["ProductoNombre"] as string;
                                obj.ProductoCodigo = oReader["ProductoCodigo"] as string;
                                obj.Cantidad = 1;
                                obj.Precio = (decimal)(oReader["Precio"] ?? 0);
                                obj.PrecioSinIva = (decimal)(oReader["PrecioSinIva"] ?? 0);
                                obj.FechaAlta = (DateTime)(oReader["FechaAlta"]);
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;

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
        public async Task<List<ComprobantesDetalleDTO>> spComprobantesDetalleGet(string ComprobanteId)
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
                    oCmd.CommandText = "ComprobantesDetalleGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@ComprobanteId", ComprobanteId);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ComprobantesDetalleDTO>();

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
                                var obj = new ComprobantesDetalleDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.ComprobanteId = oReader["ComprobanteId"] as string;

                                obj.ProductoId = oReader["ProductoId"] as string;
                                obj.ProductoNombre = oReader["ProductoNombre"] as string;
                                obj.ProductoCodigo = oReader["ProductoCodigo"] as string;

                                obj.Cantidad = (int)(oReader["Cantidad"] ?? 0); 

                                obj.Precio = (decimal)(oReader["Precio"] ?? 0);

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

        public async Task<List<ComprobantesDetalleDTO>> spComprobanteDetalleImprimirGet(string ComprobanteId)
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
                    oCmd.CommandText = "ComprobanteDetalleImprimirGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@ComprobanteId", ComprobanteId);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ComprobantesDetalleDTO>();

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
                                var obj = new ComprobantesDetalleDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.ComprobanteId = oReader["ComprobanteId"] as string;
                                obj.ProductoId = oReader["ProductoId"] as string;
                                obj.ProductoNombre = oReader["ProductoNombre"] as string;
                                obj.ProductoCodigo = oReader["ProductoCodigo"] as string;
                                obj.Cantidad = 1;
                                //if (!DBNull.Value.Equals(oReader["Total"]))
                                //    obj.Precio = (decimal)(oReader["Total"] ?? 0);
                                //if (!DBNull.Value.Equals(oReader["TotalLista"]))
                                //    obj.PrecioLista = (decimal)(oReader["TotalLista"] ?? 0);
                                //if (!DBNull.Value.Equals(oReader["PrecioSinIva"]))
                                //    obj.PrecioSinIva = (decimal)(oReader["PrecioSinIva"] ?? 0);
                                //if (!DBNull.Value.Equals(oReader["PrecioListaSinIva"]))
                                //    obj.PrecioListaSinIva = (decimal)(oReader["PrecioListaSinIva"] ?? 0);
                                
                                
                                obj.FechaAlta = (DateTime)(oReader["FechaAlta"]);
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;

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

        public async Task<int> spComprobanteInsertaDatosFiscales(ComprobantesDTO comprobantesDTO)
        {
            try
            {
                using (var oCnn = factoryConnection.GetConnection())
                {
                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        //asignamos la conexion de trabajo
                        oCmd.Connection = oCnn;

                        //utilizamos stored procedures
                        oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //el indicamos cual stored procedure utilizar
                        oCmd.CommandText = "ComprobantesInsertarDatosFiscales";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", comprobantesDTO.Id);
                        oCmd.Parameters.AddWithValue("@TipoComprobanteFiscal", comprobantesDTO.TipoComprobanteFiscal);
                        oCmd.Parameters.AddWithValue("@LetraFiscal", comprobantesDTO.LetraFiscal);
                        oCmd.Parameters.AddWithValue("@PtoVentaFiscal", comprobantesDTO.PtoVentaFiscal);
                        oCmd.Parameters.AddWithValue("@NumeroFiscal", comprobantesDTO.NumeroFiscal);
                        oCmd.Parameters.AddWithValue("@Usuario", comprobantesDTO.UsuarioAlta);

                        //Ejecutamos el comando y retornamos el id generado
                        await oCmd.ExecuteScalarAsync();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el registro: " + ex.Message);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
        }

        public async Task<int> spComprobanteInsertaCodigoAutorizacion(ComprobantesFormasPagosDTO comprobantesDTO)
        {
            try
            {
                using (var oCnn = factoryConnection.GetConnection())
                {
                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        //asignamos la conexion de trabajo
                        oCmd.Connection = oCnn;

                        //utilizamos stored procedures
                        oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //el indicamos cual stored procedure utilizar
                        oCmd.CommandText = "ComprobantesInsertarCodigoAutorizacion";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", comprobantesDTO.Id);
                        oCmd.Parameters.AddWithValue("@CodigoAutorizacion", comprobantesDTO.CodigoAutorizacion);
                        oCmd.Parameters.AddWithValue("@Usuario", comprobantesDTO.UsuarioAlta);

                        //Ejecutamos el comando y retornamos el id generado
                        await oCmd.ExecuteScalarAsync();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el registro: " + ex.Message);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
        }

        public async Task<List<ComprobantesDetalleIndicador>> spComprobanteDetalleEntregaIndicador()
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
                    oCmd.CommandText = "ComprobantesDetalleEntregarPendienteIndicador";

                    //le asignamos el parámetro para el stored procedure
                    //oCmd.Parameters.AddWithValue("@ComprobanteId", Id);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ComprobantesDetalleIndicador>();

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
                                var obj = new ComprobantesDetalleIndicador();
                                obj.Id = oReader["Id"] as string;
                                obj.ComprobanteId = oReader["ComprobanteId"] as string;
                                obj.DetalleId = oReader["ComprobanteId"] as string;
                                obj.ProductoId = oReader["ProductoId"] as string;
                                obj.ProductoNombre = oReader["ProductoNombre"] as string;
                                obj.ProductoCodigo = oReader["ProductoCodigo"] as string;

                                obj.Cantidad = (int)(oReader["Cantidad"] ?? 0);

                                //obj.PrecioUnitario = (decimal)(oReader["PrecioUnitario"] ?? 0);
                                obj.Precio = (decimal)(oReader["Precio"] ?? 0);

                                obj.Imputado = (decimal)(oReader["ImporteImputado"] ?? 0);
                                obj.ImputadoPorcentaje = (decimal)(oReader["PorcentajeImputado"] ?? 0);

                                obj.Estado = (bool)oReader["Estado"];
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.Usuario = oReader["Usuario"] as string;

                                if (!DBNull.Value.Equals(oReader["Entrega"]))
                                    obj.EntregaEstado = (bool)(oReader["Entrega"] ?? false);

                                if (!DBNull.Value.Equals(oReader["FechaEntrega"]))
                                    obj.EntregaFecha = (DateTime)(oReader["FechaEntrega"] ?? DateTime.Now);

                                obj.EntregaUsuario = oReader["UsuarioEntrega"] as string;

                                if (!DBNull.Value.Equals(oReader["AutorizaEntrega"]))
                                    obj.AutorizaEstado = (bool)(oReader["AutorizaEntrega"] ?? false);

                                if (!DBNull.Value.Equals(oReader["FechaAutoriza"]))
                                    obj.AutorizaFecha = (DateTime)(oReader["FechaAutoriza"] ?? DateTime.Now);

                                obj.AutorizaUsuario = oReader["UsuarioDespacha"] as string;


                                if (!DBNull.Value.Equals(oReader["Despacha"]))
                                    obj.DespachaEstado = (bool)oReader["Despacha"];

                                if (!DBNull.Value.Equals(oReader["FechaDespacha"]))
                                    obj.DespachaFecha = (DateTime)oReader["FechaDespacha"];

                                obj.DespachaUsuario = oReader["UsuarioDespacha"] as string;

                                if (!DBNull.Value.Equals(oReader["Devolucion"]))
                                    obj.DevolucionEstado = (bool)oReader["Devolucion"];

                                if (!DBNull.Value.Equals(oReader["FechaDevolucion"]))
                                    obj.DevolucionFecha = (DateTime)oReader["FechaDevolucion"];

                                obj.DevolucionUsuario = oReader["UsuarioDevolucion"] as string;
                                obj.DevolucionMotivo = oReader["MotivoDevolucion"] as string;


                                if (!DBNull.Value.Equals(oReader["FechaComprobante"]))
                                    obj.FechaComprobante = (DateTime)oReader["FechaComprobante"];

                                obj.CodigoComprobante = oReader["CodigoComprobante"] as string;
                                obj.UsuarioComprobante = oReader["UsuarioComprobante"] as string;
                                obj.ClienteCodigo = oReader["ClienteCodigo"] as string;
                                obj.RazonSocial = oReader["RazonSocial"] as string;
                                obj.CodigoPrespuesto = oReader["CodigoPrespuesto"] as string;


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

        public async Task<List<ComprobantesDTO>> spComprobantesInputaciones(string ComprobanteId)
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
                    oCmd.CommandText = "ComprobantesImputacionGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@ComprobanteId", ComprobanteId);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ComprobantesDTO>();

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
                                var obj = new ComprobantesDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.Codigo = oReader["Codigo"] as string;

                                obj.TipoComprobanteId = oReader["TipoComprobanteId"] as string;
                                obj.TipoComprobante = oReader["TipoComprobante"] as string;
                                obj.TipoComprobanteCodigo = oReader["TipoComprobanteCodigo"] as string;

                                obj.PresupuestoId = oReader["PresupuestoId"] as string;
                                obj.Letra = oReader["Letra"] as string;
                                obj.PtoVenta = (int)(oReader["PtoVenta"] ?? 0);
                                obj.Numero = (decimal)(oReader["Numero"] ?? 0);
                                obj.FechaComprobante = (DateTime)oReader["FechaComprobante"];
                                obj.ConceptoIncluidoId = oReader["ConceptoIncluidoId"] as string;
                                obj.ConceptoIncluidoCodigo = oReader["ConceptoIncluidoCodigo"] as string;
                                obj.ConceptoIncluido = oReader["ConceptoIncluido"] as string;

                                if (!DBNull.Value.Equals(oReader["PeriodoFacturadoDesde"]))
                                    obj.PeriodoFacturadoDesde = (DateTime)(oReader["PeriodoFacturadoDesde"] ?? DateTime.Now);
                                if (!DBNull.Value.Equals(oReader["PeriodoFacturadoDesde"]))
                                    obj.PeriodoFacturadoHasta = (DateTime)(oReader["PeriodoFacturadoDesde"] ?? DateTime.Now);
                                if (!DBNull.Value.Equals(oReader["FechaVencimiento"]))
                                    obj.FechaVencimiento = (DateTime)(oReader["FechaVencimiento"] ?? DateTime.Now);

                                obj.TipoResponsableId = oReader["TipoResponsableId"] as string;
                                obj.TipoResponsableCodigo = oReader["TipoResponsableCodigo"] as string;
                                obj.TipoResponsable = oReader["TipoResponsable"] as string;

                                obj.ClienteId = oReader["ClienteId"] as string;
                                obj.ClienteCodigo = oReader["ClienteCodigo"] as string;
                                obj.TipoDocumentoId = oReader["TipoDocumentoId"] as string;
                                obj.TipoDocumentoCodigo = oReader["TipoDocumentoCodigo"] as string;
                                obj.TipoDocumento = oReader["TipoDocumento"] as string;
                                obj.NroDocumento = oReader["NroDocumento"] as string;
                                obj.CuilCuit = oReader["CuilCuit"] as string;
                                obj.RazonSocial = oReader["RazonSocial"] as string;

                                obj.ProvinciaId = oReader["ProvinciaId"] as string;
                                obj.ProvinciaCodigo = oReader["ProvinciaCodigo"] as string;
                                obj.Provincia = oReader["Provincia"] as string;

                                obj.Localidad = oReader["Localidad"] as string;
                                obj.CodigoPostal = oReader["CodigoPostal"] as string;
                                obj.Calle = oReader["Calle"] as string;
                                obj.CalleNro = oReader["CalleNro"] as string;
                                obj.PisoDpto = oReader["PisoDpto"] as string;
                                obj.OtrasReferencias = oReader["OtrasReferencias"] as string;

                                obj.Email = oReader["Email"] as string;
                                obj.Telefono = oReader["Telefono"] as string;
                                obj.Celular = oReader["Celular"] as string;

                                obj.Total = (decimal)(oReader["Total"] ?? 0);
                                obj.TotalSinImpuesto = (decimal)(oReader["TotalSinImpuesto"] ?? 0);
                                obj.TotalSinDescuento = (decimal)(oReader["TotalSinDescuento"] ?? 0);
                                obj.TotalSinImpuestoSinDescuento = (decimal)(oReader["TotalSinImpuestoSinDescuento"] ?? 0);
                                obj.DescuentoPorcentaje = (decimal)(oReader["DescuentoPorcentaje"] ?? 0);
                                obj.DescuentoTotal = (decimal)(oReader["DescuentoTotal"] ?? 0);
                                obj.DescuentoSinImpuesto = (decimal)(oReader["DescuentoSinImpuesto"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["ImporteTributos"]))
                                    obj.ImporteTributos = (decimal)(oReader["ImporteTributos"] ?? 0);

                                obj.Observaciones = oReader["Observaciones"] as string;

                                obj.TipoComprobanteAnulaId = oReader["TipoComprobanteAnulaId"] as string;
                                obj.TipoComprobanteAnula = oReader["TipoComprobanteAnula"] as string;
                                obj.TipoComprobanteAnulaCodigo = oReader["TipoComprobanteAnulaCodigo"] as string;

                                obj.LetraAnula = oReader["LetraAnula"] as string;
                                if (!DBNull.Value.Equals(oReader["PtoVtaAnula"]))
                                    obj.PtoVtaAnula = (int)oReader["PtoVtaAnula"];

                                if (!DBNull.Value.Equals(oReader["NumeroAnula"]))
                                    obj.NumeroAnula = (decimal)(oReader["NumeroAnula"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["FechaAnulacion"]))
                                    obj.FechaAnulacion = (DateTime)oReader["FechaAnulacion"];

                                if (!DBNull.Value.Equals(oReader["Anulado"]))
                                    obj.Anulado = (bool)oReader["Anulado"];
                                obj.CodigoAnula = oReader["CodigoAnula"] as string;
                                obj.UsuarioAnula = oReader["UsuarioAnula"] as string;

                                if (!DBNull.Value.Equals(oReader["Saldo"]))
                                    obj.Saldo = (decimal)(oReader["Saldo"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["Saldo_Porcentaje"]))
                                    obj.Saldo_Porcentaje = (decimal)(oReader["Saldo_Porcentaje"] ?? 0);

                                obj.TipoComprobanteFiscal = oReader["TipoComprobanteFiscal"] as string;
                                obj.LetraFiscal = oReader["LetraFiscal"] as string;
                                if (!DBNull.Value.Equals(oReader["PtoVentaFiscal"]))
                                    obj.PtoVentaFiscal = (int)(oReader["PtoVentaFiscal"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["NumeroFiscal"]))
                                    obj.NumeroFiscal = (decimal)(oReader["NumeroFiscal"] ?? 0);

                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;
                                obj.CodigoPresupuesto = oReader["CodigoPresupuesto"] as string;

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

        public async Task<List<ComprobantesDTO>> spPresupuestosRemitos()
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
                    oCmd.CommandText = "ComprobantesRemitosGet";


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ComprobantesDTO>();

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
                                var obj = new ComprobantesDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.Codigo = oReader["Codigo"] as string;

                                obj.TipoComprobanteId = oReader["TipoComprobanteId"] as string;
                                obj.TipoComprobante = oReader["TipoComprobante"] as string;
                                obj.TipoComprobanteCodigo = oReader["TipoComprobanteCodigo"] as string;

                                obj.PresupuestoId = oReader["PresupuestoId"] as string;
                                obj.Letra = oReader["Letra"] as string;
                                obj.PtoVenta = (int)(oReader["PtoVenta"] ?? 0);
                                obj.Numero = (decimal)(oReader["Numero"] ?? 0);
                                obj.FechaComprobante = (DateTime)oReader["FechaComprobante"];
                                obj.ConceptoIncluidoId = oReader["ConceptoIncluidoId"] as string;
                                obj.ConceptoIncluidoCodigo = oReader["ConceptoIncluidoCodigo"] as string;
                                obj.ConceptoIncluido = oReader["ConceptoIncluido"] as string;

                                if (!DBNull.Value.Equals(oReader["PeriodoFacturadoDesde"]))
                                    obj.PeriodoFacturadoDesde = (DateTime)(oReader["PeriodoFacturadoDesde"] ?? DateTime.Now);
                                if (!DBNull.Value.Equals(oReader["PeriodoFacturadoDesde"]))
                                    obj.PeriodoFacturadoHasta = (DateTime)(oReader["PeriodoFacturadoDesde"] ?? DateTime.Now);
                                if (!DBNull.Value.Equals(oReader["FechaVencimiento"]))
                                    obj.FechaVencimiento = (DateTime)(oReader["FechaVencimiento"] ?? DateTime.Now);

                                obj.TipoResponsableId = oReader["TipoResponsableId"] as string;
                                obj.TipoResponsableCodigo = oReader["TipoResponsableCodigo"] as string;
                                obj.TipoResponsable = oReader["TipoResponsable"] as string;

                                obj.ClienteId = oReader["ClienteId"] as string;
                                obj.ClienteCodigo = oReader["ClienteCodigo"] as string;
                                obj.TipoDocumentoId = oReader["TipoDocumentoId"] as string;
                                obj.TipoDocumentoCodigo = oReader["TipoDocumentoCodigo"] as string;
                                obj.TipoDocumento = oReader["TipoDocumento"] as string;
                                obj.NroDocumento = oReader["NroDocumento"] as string;
                                obj.CuilCuit = oReader["CuilCuit"] as string;
                                obj.RazonSocial = oReader["RazonSocial"] as string;

                                obj.ProvinciaId = oReader["ProvinciaId"] as string;
                                obj.ProvinciaCodigo = oReader["ProvinciaCodigo"] as string;
                                obj.Provincia = oReader["Provincia"] as string;

                                obj.Localidad = oReader["Localidad"] as string;
                                obj.CodigoPostal = oReader["CodigoPostal"] as string;
                                obj.Calle = oReader["Calle"] as string;
                                obj.CalleNro = oReader["CalleNro"] as string;
                                obj.PisoDpto = oReader["PisoDpto"] as string;
                                obj.OtrasReferencias = oReader["OtrasReferencias"] as string;

                                obj.Email = oReader["Email"] as string;
                                obj.Telefono = oReader["Telefono"] as string;
                                obj.Celular = oReader["Celular"] as string;

                                obj.Total = (decimal)(oReader["Total"] ?? 0);
                                obj.TotalSinImpuesto = (decimal)(oReader["TotalSinImpuesto"] ?? 0);
                                obj.TotalSinDescuento = (decimal)(oReader["TotalSinDescuento"] ?? 0);
                                obj.TotalSinImpuestoSinDescuento = (decimal)(oReader["TotalSinImpuestoSinDescuento"] ?? 0);
                                obj.DescuentoPorcentaje = (decimal)(oReader["DescuentoPorcentaje"] ?? 0);
                                obj.DescuentoTotal = (decimal)(oReader["DescuentoTotal"] ?? 0);
                                obj.DescuentoSinImpuesto = (decimal)(oReader["DescuentoSinImpuesto"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["ImporteTributos"]))
                                    obj.ImporteTributos = (decimal)(oReader["ImporteTributos"] ?? 0);

                                obj.Observaciones = oReader["Observaciones"] as string;

                                obj.TipoComprobanteAnulaId = oReader["TipoComprobanteAnulaId"] as string;
                                obj.TipoComprobanteAnula = oReader["TipoComprobanteAnula"] as string;
                                obj.TipoComprobanteAnulaCodigo = oReader["TipoComprobanteAnulaCodigo"] as string;

                                obj.LetraAnula = oReader["LetraAnula"] as string;
                                if (!DBNull.Value.Equals(oReader["PtoVtaAnula"]))
                                    obj.PtoVtaAnula = (int)oReader["PtoVtaAnula"];

                                if (!DBNull.Value.Equals(oReader["NumeroAnula"]))
                                    obj.NumeroAnula = (decimal)(oReader["NumeroAnula"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["FechaAnulacion"]))
                                    obj.FechaAnulacion = (DateTime)oReader["FechaAnulacion"];

                                if (!DBNull.Value.Equals(oReader["Saldo"]))
                                    obj.Saldo = (decimal)(oReader["Saldo"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["Saldo_Porcentaje"]))
                                    obj.Saldo_Porcentaje = (decimal)(oReader["Saldo_Porcentaje"] ?? 0);

                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;
                                obj.CodigoPresupuesto = oReader["CodigoPresupuesto"] as string;

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

        public async Task<int> spDolar(ComprobantesDolarDTO formaPago)
        {
            try
            {
                using (var oCnn = factoryConnection.GetConnection())
                {
                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        //asignamos la conexion de trabajo
                        oCmd.Connection = oCnn;

                        //utilizamos stored procedures
                        oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //el indicamos cual stored procedure utilizar
                        oCmd.CommandText = "ComprobanteTmpInsertarDolar";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@ComprobanteId", formaPago.ComprobanteId);
                        oCmd.Parameters.AddWithValue("@FormaPagoId", formaPago.FormaPagoId);
                        oCmd.Parameters.AddWithValue("@Importe", formaPago.Importe);
                        oCmd.Parameters.AddWithValue("@DolarImporte", formaPago.DolarImporte);
                        oCmd.Parameters.AddWithValue("@DolarCotizacion", formaPago.DolarCotizacion);
                        oCmd.Parameters.AddWithValue("@TotalImporte", formaPago.Saldo);
                        oCmd.Parameters.AddWithValue("@DescuentoImporte", formaPago.SaldoConDescuento);
                        oCmd.Parameters.AddWithValue("@Observaciones", formaPago.Observaciones);
                        oCmd.Parameters.AddWithValue("@Usuario", formaPago.Usuario);

                        //Ejecutamos el comando y retornamos el id generado
                        await oCmd.ExecuteScalarAsync();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el registro: " + ex.Message);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
        }

    }
}
