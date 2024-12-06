using CitRobots.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CitRobots.Services
{
    public class CustomizationService
    {
        private readonly DatabaseHelper _db;

        public CustomizationService() => _db = new DatabaseHelper();

        public List<(int id, string nome, string codigo)> GetCores()
        {
            var dt = _db.ExecuteSelect("SELECT id, nome, codigo_hex FROM opcoes_cores WHERE ativo = 1");
            return dt.AsEnumerable()
                    .Select(row => (
                        Convert.ToInt32(row["id"]),
                        row["nome"].ToString(),
                        row["codigo_hex"].ToString()
                    )).ToList();
        }

        public List<(int id, string nome)> GetVozes()
        {
            var dt = _db.ExecuteSelect("SELECT id, nome FROM opcoes_vozes WHERE ativo = 1");
            return dt.AsEnumerable()
                    .Select(row => (
                        Convert.ToInt32(row["id"]),
                        row["nome"].ToString()
                    )).ToList();
        }

        public List<(int id, decimal altura, int peso)> GetDimensoes()
        {
            var dt = _db.ExecuteSelect("SELECT id, altura, peso FROM opcoes_dimensoes WHERE ativo = 1");
            return dt.AsEnumerable()
                    .Select(row => (
                        Convert.ToInt32(row["id"]),
                        Convert.ToDecimal(row["altura"]),
                        Convert.ToInt32(row["peso"])
                    )).ToList();
        }
    }
}