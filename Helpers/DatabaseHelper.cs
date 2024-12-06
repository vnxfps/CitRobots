using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace CitRobots.Helpers
{
    public class DatabaseHelper
    {
        // Configure sua string de conexão aqui
        private static readonly string ConnectionString = "Server=localhost;Database=CitRobots;Uid=root;Pwd=;";

        /// <summary>
        /// Testa a conexão com o banco de dados.
        /// </summary>
        public void TestConnection()
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
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

        /// <summary>
        /// Executa um comando SELECT e retorna os resultados como um DataTable.
        /// </summary>
        public DataTable ExecuteSelect(string query, Dictionary<string, object> parameters = null)
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        AddParameters(command, parameters);

                        var dt = new DataTable();
                        using (var adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(dt);
                        }
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao executar SELECT: {ex.Message}");
            }
        }

        /// <summary>
        /// Executa um comando INSERT e retorna o ID do último registro inserido.
        /// </summary>
        public int ExecuteInsert(string query, Dictionary<string, object> parameters)
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query + "; SELECT LAST_INSERT_ID();", connection))
                    {
                        AddParameters(command, parameters);
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao executar INSERT: {ex.Message}");
            }
        }

        /// <summary>
        /// Executa um comando UPDATE e retorna o número de linhas afetadas.
        /// </summary>
        public int ExecuteUpdate(string query, Dictionary<string, object> parameters)
        {
            return ExecuteNonQuery(query, parameters, "UPDATE");
        }

        /// <summary>
        /// Executa um comando DELETE e retorna o número de linhas afetadas.
        /// </summary>
        public int ExecuteDelete(string query, Dictionary<string, object> parameters)
        {
            return ExecuteNonQuery(query, parameters, "DELETE");
        }

        /// <summary>
        /// Método genérico para executar comandos como INSERT, UPDATE e DELETE.
        /// </summary>
        private int ExecuteNonQuery(string query, Dictionary<string, object> parameters, string commandType)
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        AddParameters(command, parameters);
                        return command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao executar {commandType}: {ex.Message}");
            }
        }

        /// <summary>
        /// Adiciona parâmetros ao comando MySQL.
        /// </summary>
        private void AddParameters(MySqlCommand command, Dictionary<string, object> parameters)
        {
            if (parameters == null) return;

            foreach (var param in parameters)
            {
                command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
            }
        }
    }
}