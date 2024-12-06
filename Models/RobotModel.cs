using System;
using Newtonsoft.Json;

namespace CitRobots.Models
{
    public class RobotModel
    {
        public int Id { get; set; }
        public int CategoriaId { get; set; }
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public string CodigoModelo { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }
        public string ImagemUrl { get; set; }
        public string Especificacoes { get; set; }
        public string Slogan { get; set; }
        public bool Ativo { get; set; }
        public RobotCustomizationModel Customization { get; set; }

        public RobotModel()
        {
            Customization = new RobotCustomizationModel();
            Codigo = CodigoModelo;
        }

        public decimal CalcularPrecoTotal()
        {
            return Preco + (Customization?.CalcularPrecoAdicionais() ?? 0);
        }

        public string GetEspecificacoesFormatadas()
        {
            try
            {
                var specs = JsonConvert.DeserializeObject<dynamic>(Especificacoes);
                return specs.ToString();
            }
            catch
            {
                return Especificacoes;
            }
        }
    }
}