﻿using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public interface ICajasRepository : IGenericRepository<ParamCajas>
    {
        IEnumerable<SelectListItem> GetCombo(string sucursalId);
        Task<List<CajasEstadoDTO>> spCajasEstadoImportesGet(string Id);
        Task<List<CajasEstadoDTO>> spCajasEstadoUsuariosGet(string Id);
        Task<List<CajasEstadoDTO>> spCajasEstadoFechaGet();
        Task<List<CajasEstadoDTO>> spCajasEstadoFechaGet(string sucursalId);
        Task<List<CajasEstadoFormaPagosDTO>> spCajasEstadoChequesGet(string Id);
        Task<List<CajasEstadoFormaPagosDTO>> spCajasEstadoEfectivoGet(string Id);
        Task<List<CajasEstadoFormaPagosDTO>> spCajasEstadoDolaresGet(string Id);
    }
}
