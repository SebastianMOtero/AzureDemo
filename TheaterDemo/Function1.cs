using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace TheaterDemo
{
    public static class Function1
    {
        [FunctionName("DeleteReservation")]
        public static async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            var str = ConfigurationManager.ConnectionStrings["sqldb_connection"].ConnectionString;

            using (SqlConnection oConn = new SqlConnection(str))
            {
                oConn.Open();
                var text = "DELETE b FROM Booking as b " +
                           "INNER JOIN Funcion as f ON b.id_function = f.id " +
                           "WHERE f.fecha = CONVERT(date, SYSDATETIME());";
                using (SqlCommand oCmd = new SqlCommand(text, oConn))
                {
                    var rows = await oCmd.ExecuteNonQueryAsync();
                    log.Info($"{rows} rows were updated");
                }
            }
        }
    }
}
