using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CitRobots.Models;

namespace CitRobots.Views
{
    public partial class ProductsForm : Form
    {
        private List<RobotModel> robots;
        private Panel contentPanel;

        public ProductsForm()
        {
            InitializeComponent();
            LoadRobotsData();
            SetupProductsForm();
        }

        private void LoadRobotsData()
        {
            robots = new List<RobotModel>
           {
               new RobotModel
               {
                   Id = 1,
                   Nome = "Striker 1.8",
                   Codigo = "1339255",
                   Descricao = "Conheça o Striker 1.8, o robô projetado para revolucionar o treinamento de futebol. " +
                              "Com precisão avançada e inteligência artificial, ele foi desenvolvido para aperfeiçoar a arte de fazer gols, " +
                              "simulando jogadas complexas e ajudando jogadores a aprimorar suas habilidades.",
                   Slogan = "Ataque e precisão",
                   Preco = 174999.99m,
                   ImagemUrl = "striker.jpg"
               },
               new RobotModel
               {
                   Id = 2,
                   Nome = "DefenderX 1.9",
                   Codigo = "1339256",
                   Descricao = "Sistema de defesa inteligente e tempos de reação ultrarrápidos.",
                   Slogan = "Consistência e Confiança",
                   Preco = 174999.99m,
                   ImagemUrl = "defender.jpg"
               },
               new RobotModel
               {
                   Id = 3,
                   Nome = "KeeperZ 1.3",
                   Codigo = "1339257",
                   Descricao = "Preparado para defender qualquer jogada com agilidade excepcional.",
                   Slogan = "Segurança e Agilidade",
                   Preco = 174999.99m,
                   ImagemUrl = "keeper.jpg"
               }
           };
        }

        private void SetupProductsForm()
        {
            this.BackColor = Color.FromArgb(45, 45, 45);
            this.Dock = DockStyle.Fill;

            Panel mainContainer = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30)
            };

            FlowLayoutPanel cardsContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                Padding = new Padding(10)
            };

            foreach (var robot in robots)
            {
                cardsContainer.Controls.Add(CreateProductCard(robot));
            }

            contentPanel = new Panel
            {
                Name = "contentPanel",
                Dock = DockStyle.Fill,
                Visible = false
            };

            cardsContainer.Dock = DockStyle.Fill;
            mainContainer.Controls.Add(contentPanel);
            mainContainer.Controls.Add(cardsContainer);
            this.Controls.Add(mainContainer);
        }

        private Panel CreateProductCard(RobotModel robot)
        {
            Panel card = new Panel
            {
                BackColor = Color.White,
                Size = new Size(450, 550),
                Margin = new Padding(20),
                Padding = new Padding(30)
            };

            Label lblName = new Label
            {
                Text = robot.Nome,
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 45, 45),
                Location = new Point(25, 25),
                AutoSize = true
            };

            Label lblSlogan = new Label
            {
                Text = robot.Slogan,
                Font = new Font("Segoe UI", 16),
                ForeColor = Color.FromArgb(100, 100, 100),
                Location = new Point(25, 85),
                AutoSize = true
            };

            PictureBox productImage = new PictureBox
            {
                Size = new Size(350, 300),
                Location = new Point(50, 130),
                SizeMode = PictureBoxSizeMode.Zoom
            };

            try
            {
                productImage.Image = Image.FromFile($"Resources/Images/{robot.ImagemUrl}");
            }
            catch
            {
                Label lblNoImage = new Label
                {
                    Text = "Imagem não disponível",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    ForeColor = Color.FromArgb(150, 150, 150)
                };
                productImage.Controls.Add(lblNoImage);
            }

            Button btnMore = new Button
            {
                Text = "Mais informações",
                Size = new Size(250, 45),
                Location = new Point(100, 460),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(57, 57, 57),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14),
                Cursor = Cursors.Hand
            };
            btnMore.FlatAppearance.BorderSize = 0;

            btnMore.Click += (s, e) =>
            {
                var detailsForm = new RobotDetailsForm(robot);
                contentPanel.Controls.Clear();
                contentPanel.Controls.Add(detailsForm);
                detailsForm.Dock = DockStyle.Fill;
                contentPanel.Visible = true;
            };

            card.Controls.AddRange(new Control[] { lblName, lblSlogan, productImage, btnMore });
            return card;
        }
    }
}