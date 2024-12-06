using CitRobots.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitRobots.Services
{
    public class DiscountService
    {
        private readonly DatabaseHelper _db;

        public DiscountService()
        {
            _db = new DatabaseHelper();
        }

        public decimal AplicarDesconto(string codigo, decimal valorTotal)
        {
            string query = @"SELECT * FROM cupons_desconto 
                        WHERE codigo = @Codigo 
                        AND ativo = 1 
                        AND data_inicio <= NOW() 
                        AND data_validade >= NOW() 
                        AND (quantidade_maxima IS NULL OR quantidade_utilizada < quantidade_maxima)
                        AND (valor_minimo_pedido IS NULL OR @ValorTotal >= valor_minimo_pedido)";

            var parameters = new Dictionary<string, object>
        {
            { "@Codigo", codigo },
            { "@ValorTotal", valorTotal }
        };

            var result = _db.ExecuteSelect(query, parameters);

            if (result.Rows.Count > 0)
            {
                var row = result.Rows[0];
                string tipo = row["tipo"].ToString();
                decimal valor = Convert.ToDecimal(row["valor"]);
                int cupomId = Convert.ToInt32(row["id"]);

                IncrementarUso(cupomId);

                return tipo == "PORCENTAGEM"
                    ? valorTotal * (1 - valor / 100)
                    : valorTotal - valor;
            }

            return valorTotal;
        }

        private void IncrementarUso(int cupomId)
        {
            string query = @"UPDATE cupons_desconto 
                        SET quantidade_utilizada = quantidade_utilizada + 1 
                        WHERE id = @CupomId";

            var parameters = new Dictionary<string, object> { { "@CupomId", cupomId } };
            _db.ExecuteUpdate(query, parameters);
        }
    }
}
