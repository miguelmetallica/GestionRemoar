using System.Data;
using System.Data.SqlClient;

namespace Gestion.Web.Data
{
    public interface IFactoryConnection
    {
        void CloseConnection();
        SqlConnection GetConnection();

    }
}
