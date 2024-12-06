using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitRobots.Models;

namespace CitRobots.Models
{
    public class CartItemModel
    {
        public RobotModel Robot { get; set; }
        public RobotCustomizationModel Customization { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal PrecoTotal { get; set; }
    }
}