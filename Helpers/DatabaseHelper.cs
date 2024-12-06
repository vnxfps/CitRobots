using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace CitRobots.Helpers
{
    public class DatabaseHelper
    {

        private static string ConnectionString = "Server=localhost;Database=CitRobots;Uid=root;Pwd=;";

        public void TestConnection()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    Console.WriteLine("Conexão com banco de dados estabelecida com sucesso!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao conectar ao banco de dados: {ex.Message}");
            }
        }

        public DataTable ExecuteSelect(string query, Dictionary<string, object> parameters = null)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

                    DataTable dt = new DataTable();
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                    return dt;
                }
            }
        }

        public int ExecuteInsert(string query, Dictionary<string, object> parameters)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }

                    query += "; SELECT LAST_INSERT_ID();";
                    command.CommandText = query;

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public int ExecuteUpdate(string query, Dictionary<string, object> parameters)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }

                    return command.ExecuteNonQuery();
                }
            }
        }

        public int ExecuteDelete(string query, Dictionary<string, object> parameters)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }

                    return command.ExecuteNonQuery();
                }
            }
        }
    }   
}