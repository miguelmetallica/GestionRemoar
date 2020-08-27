using Gestion.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public interface IClientesRepository : IGenericRepository<Clientes>
    {
        List<Clientes> GetAllActivos();
        Clientes GetOne(string id);
        Task<bool> ExistCuitCuilAsync(string id, string cuitcuil);

        Task<bool> ExistNroDocAsync(string id, string tipo, string nro);

        Task<int> spInsertar(ClientesFisicoAdd item);

        Task<int> spEditar(ClientesFisicoEdit item);
    }
}
