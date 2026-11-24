using Microsoft.Data.SqlClient;

namespace Qistflow_Semester_Project.Data
{
    public interface IDbConnectionFactory
    {
        SqlConnection CreateConnection();
    }
}
