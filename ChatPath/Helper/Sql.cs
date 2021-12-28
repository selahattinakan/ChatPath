using System.Data.SqlClient;

namespace ChatPath
{
    public static class Sql
    {
        public static SqlConnection get()
        {
            var connection = new SqlConnection("Data Source=localhost;Integrated Security=True;Timeout=30;pooling=true;MultipleActiveResultSets=true;Min Pool Size=10;Max Pool Size=200;Connection Timeout=9000");
            connection.Open();
            connection.ChangeDatabase("ChatPath");

            return connection;
        }

    }

}
