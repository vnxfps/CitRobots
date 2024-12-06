using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitRobots.Models;

namespace CitRobots.Services
{
    public static class CartService
    {
        private static List<CartItemModel> _cartItems = new List<CartItemModel>();

        public static void AddToCart(RobotModel robot, RobotCustomizationModel customization)
        {
            var item = new CartItemModel
            {
                Robot = robot,
                Customization = customization,
                PrecoUnitario = robot.Preco,
                PrecoTotal = robot.CalcularPrecoTotal()
            };

            _cartItems.Add(item);
        }

        public static List<CartItemModel> GetCartItems()
        {
            return _cartItems;
        }

        public static void RemoveFromCart(int index)
        {
            if (index >= 0 && index < _cartItems.Count)
                _cartItems.RemoveAt(index);
        }

        public static void ClearCart()
        {
            _cartItems.Clear();
        }

        public static decimal GetTotal()
        {
            return _cartItems.Sum(item => item.PrecoTotal);
        }

        private static decimal CalcularPrecoPersonalizacoes(RobotCustomizationModel customization)
        {
            decimal total = 0;

            if (customization.TemReplay) total += 5000;
            if (customization.TemMonitoramento) total += 7500;

            return total;
        }

        public static decimal AplicarDesconto(string cupom)
        {
            var descontos = new Dictionary<string, decimal>
            {
                { "CIT5", 0.05m },
                { "CIT10", 0.10m },
                { "CIT15", 0.15m }
            };

            if (descontos.TryGetValue(cupom.ToUpper(), out decimal percentual))
            {
                return GetTotal() * (1 - percentual);
            }

            return GetTotal();
        }
    }
}