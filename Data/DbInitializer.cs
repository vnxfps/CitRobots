using CitRobots.Models;

namespace CitRobots.Data
{
    public static class DbInitializer
    {
        public static void Initialize(CitRobotsContext context)
        {
            context.Database.EnsureCreated();

            // Verifica se já existem robôs
            if (context.Robots.Any())
            {
                return;   // DB já foi populado
            }

            var robots = new Robot[]
            {
                new Robot
                {
                    Nome = "Keeper 2024",
                    Preco = 174999.00M,
                    Quantidade = 10,
                    Slogan = "Aprimore cada defesa. Leva os goleiros ao próximo nível."
                },
                // Adicione os outros robôs
            };

            foreach (Robot r in robots)
            {
                context.Robots.Add(r);
            }
            context.SaveChanges();
        }
    }
}