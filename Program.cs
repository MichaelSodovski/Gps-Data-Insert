using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Diagnostics;

namespace GpsDataInsert
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            var connectionString = builder.Configuration.GetConnectionString("IRAILMETA");

            var app = builder.Build();

            Random random = new Random();

            Task.Run(() =>
            {
                while (true)
                {
                    using (OracleConnection conn = new OracleConnection(connectionString))
                    {
                        using (OracleCommand cmd = conn.CreateCommand())
                        {
                            try
                            {
                                conn.Open();
                                cmd.BindByName = true;

                                Stopwatch stopwatch = new Stopwatch();
                                stopwatch.Start();

                                // Get the current max OBJECT_ID from the GPS_DATA table
                                int lastid = 0;
                                cmd.CommandText = "SELECT MAX(OBJECT_ID) FROM GPS_DATA";
                                using (OracleDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read() && !reader.IsDBNull(0))
                                    {
                                        lastid = reader.GetInt32(0);
                                    }
                                }

                                double minX = 35.003854811137394;
                                double maxX = 35.003854811244004;
                                double minY = 32.81489071147241;
                                double maxY = 32.814890711544706;

                                for (var i = 0; i < 65; i++)
                                {
                                    double latitude = random.NextDouble() * (maxY - minY) + minY;
                                    double longitude = random.NextDouble() * (maxX - minX) + minX;


                                    cmd.Parameters.Clear();
                                    cmd.CommandText = $"INSERT INTO GPS_DATA (OBJECT_ID, X, Y, TIMESTAMP) VALUES (:OBJECT_ID, :X, :Y, :TIMESTAMP)";
                                    OracleParameter OBJECT_ID = new OracleParameter("OBJECT_ID", OracleDbType.Int32)
                                    {
                                        Value = ++lastid 
                                    };
                                    OracleParameter X = new OracleParameter("X", longitude);
                                    OracleParameter Y = new OracleParameter("Y", latitude);
                                    OracleParameter TIMESTAMP = new OracleParameter("TIMESTAMP", OracleDbType.TimeStamp)
                                    {
                                        Value = DateTime.Now
                                    };
                                    cmd.Parameters.Add(OBJECT_ID);
                                    cmd.Parameters.Add(X);
                                    cmd.Parameters.Add(Y);
                                    cmd.Parameters.Add(TIMESTAMP);
                                    cmd.ExecuteNonQuery();
                                }

                                stopwatch.Stop();
                                Console.WriteLine($"Total operation time: {stopwatch.Elapsed.TotalSeconds} seconds");
                                Thread.Sleep(5000); // Pause for 5 seconds.
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                    }
                }

            });
                
                // Configure the HTTP request pipeline.
                if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}