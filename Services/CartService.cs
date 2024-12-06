using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitRobots.Helpers;
using CitRobots.Models;

namespace CitRobots.Services
{
    public static class CartService
    {
        private static List<CartItemModel> _cartItems = new List<CartItemModel>();
        private readonly static DatabaseHelper _db = new DatabaseHelper();

        public static void AddToCart(RobotModel robot, RobotCustomizationModel customization)
        {
            var item = new CartItemModel
            {
                Robot = robot,
                Customization = customization,
                PrecoUnitario = robot.Preco,
                PrecoTotal = robot.CalcularPrecoTotal(),
                Quantidade = 1
            };
            _cartItems.Add(item);
        }

        public static List<CartItemModel> GetCartItems() => _cartItems;

        public static void RemoveFromCart(int index)
        {
            if (index >= 0 && index < _cartItems.Count)
                _cartItems.RemoveAt(index);
        }

        public static void ClearCart() => _cartItems.Clear();

        public static decimal GetTotal() => _cartItems.Sum(item => item.PrecoTotal);

        public static decimal AplicarDesconto(string cupom)
        {
            string query = @"SELECT tipo, valor FROM cupons_desconto 
                        WHERE codigo = @Codigo AND ativo = 1 
                        AND data_inicio <= NOW() AND data_validade >= NOW()";

            var parameters = new Dictionary<string, object> { { "@Codigo", cupom } };
            var result = _db.ExecuteSelect(query, parameters);

            if (result.Rows.Count > 0)
            {
                var row = result.Rows[0];
                string tipo = row["tipo"].ToString();
                decimal valor = Convert.ToDecimal(row["valor"]);

                decimal total = GetTotal();
                return tipo == "PORCENTAGEM" ? total * (1 - valor / 100) : total - valor;
            }
            return GetTotal();
        }
    }
}