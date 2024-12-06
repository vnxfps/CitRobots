using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;

namespace CitRobots.Helpers
{
    public class DatabaseHelper
    {
        private readonly string connectionString = "Server=localhost;Database=CitRobots;Uid=root;Pwd=;";

        public DataTable ExecuteSelect(string query, Dictionary<string, object> parameters = null)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

                    var dataTable = new DataTable();
                    using (var adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                    return dataTable;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao executar select: {ex.Message}");
                }
            }
        }

        public int ExecuteInsert(string query, Dictionary<string, object> parameters)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }

                    command.ExecuteNonQuery();
                    return (int)command.LastInsertedId;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao executar insert: {ex.Message}");
                }
            }
        }

        public int ExecuteUpdate(string query, Dictionary<string, object> parameters)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }
                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao executar update: {ex.Message}");
                }
            }
        }

        public int ExecuteDelete(string query, Dictionary<string, object> parameters)
        {
            return ExecuteUpdate(query, parameters);
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public void BeginTransaction(Action<MySqlTransaction> action)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        action(transaction);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}