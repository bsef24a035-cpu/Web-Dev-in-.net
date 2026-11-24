using Microsoft.Data.SqlClient;
namespace Qistflow_Semester_Project.Data
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory()
        {
            _connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=Qistflow_Semester_Project;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30";
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
