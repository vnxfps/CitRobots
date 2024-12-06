using System;
using System.Drawing;
using System.Windows.Forms;
using CitRobots.Models;
using CitRobots.Services;

namespace CitRobots.Views
{
    public partial class RobotDetailsForm : UserControl
    {
        private RobotModel robot;
        private Panel mainPanel;
        private FlowLayoutPanel customizationPanel;
        private Label lblPrecoTotal;
        private decimal precoTotal;

        public RobotDetailsForm(RobotModel robot)
        {
            this.robot = robot;
            this.robot.Customization = new RobotCustomizationModel();
            InitializeComponent();
            SetupDetailsForm();
        }

        private void SetupDetailsForm()
        {
            this.BackColor = Color.FromArgb(45, 45, 45);
            this.Dock = DockStyle.Fill;

            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(40)
            };

            TableLayoutPanel headerPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 400,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.FromArgb(35, 35, 35),
                Padding = new Padding(20),
                Margin = new Padding(0, 0, 0, 20)
            };
            headerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            headerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            Panel imagePanel = new Panel { Dock = DockStyle.Fill };
            PictureBox robotImage = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                Margin = new Padding(20)
            };
            try
            {
                robotImage.Image = Image.FromFile($"Resources/Images/{robot.ImagemUrl}");
            }
            catch
            {
                Label lblNoImage = new Label
                {
                    Text = "Imagem não disponível",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    ForeColor = Color.White
                };
                imagePanel.Controls.Add(lblNoImage);
            }
            imagePanel.Controls.Add(robotImage);

            Panel infoPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 0, 0, 0)
            };

            Label lblTitle = new Label
            {
                Text = robot.Nome,
                Font = new Font("Segoe UI", 32, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true
            };

            Label lblDescricao = new Label
            {
                Text = robot.Descricao,
                Font = new Font("Segoe UI", 16),
                ForeColor = Color.FromArgb(200, 200, 200),
                AutoSize = true,
                Location = new Point(0, 60)
            };

            lblPrecoTotal = new Label
            {
                Text = $"R$ {robot.Preco:N2}",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(92, 184, 92),
                AutoSize = true,
                Location = new Point(0, 120)
            };

            infoPanel.Controls.AddRange(new Control[] { lblTitle, lblDescricao, lblPrecoTotal });

            headerPanel.Controls.Add(imagePanel, 0, 0);
            headerPanel.Controls.Add(infoPanel, 1, 0);

            customizationPanel = CreateCustomizationPanel();

            Button btnBack = new Button
            {
                Text = "Voltar",
                Dock = DockStyle.Bottom,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(57, 57, 57),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 0, 10)
            };
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.Click += (s, e) => this.Parent.Controls.Clear();

            Button btnAddToCart = new Button
            {
                Text = "Adicionar ao Carrinho",
                Dock = DockStyle.Bottom,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(57, 57, 57),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAddToCart.FlatAppearance.BorderSize = 0;
            btnAddToCart.Click += BtnAddToCart_Click;

            mainPanel.Controls.Add(btnBack);
            mainPanel.Controls.Add(btnAddToCart);
            mainPanel.Controls.Add(customizationPanel);
            mainPanel.Controls.Add(headerPanel);
            this.Controls.Add(mainPanel);
        }

        private FlowLayoutPanel CreateCustomizationPanel()
        {
            FlowLayoutPanel panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(20),
                BackColor = Color.FromArgb(35, 35, 35),
                WrapContents = false
            };

            Label lblCustomTitle = new Label
            {
                Text = "Personalize seu Robô",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 20)
            };
            panel.Controls.Add(lblCustomTitle);

            panel.Controls.Add(CreateColorSelection());
            panel.Controls.Add(CreateVoiceSelection());
            panel.Controls.Add(CreateSizeSelection());
            panel.Controls.Add(CreateWeightSelection());
            panel.Controls.Add(CreateAddonsSelection());

            return panel;
        }

        private Panel CreateColorSelection()
        {
            Panel colorPanel = new Panel
            {
                Width = 800,
                Height = 150,
                Margin = new Padding(0, 0, 0, 20)
            };

            Label lblTitle = new Label
            {
                Text = "Cor do Robô",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(0, 0)
            };

            FlowLayoutPanel colorsFlow = new FlowLayoutPanel
            {
                Location = new Point(0, 40),
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Width = colorPanel.Width - 40
            };

            var cores = new[]
            {
               ("Azul", Color.Blue),
               ("Vermelho", Color.Red),
               ("Verde", Color.Green),
               ("Preto", Color.Black),
               ("Roxo", Color.Purple),
               ("Cinza", Color.Gray)
           };

            foreach (var (nome, cor) in cores)
            {
                Panel colorContainer = new Panel
                {
                    Width = 120,
                    Height = 40,
                    Margin = new Padding(0, 0, 10, 10)
                };

                Panel colorBox = new Panel
                {
                    Width = 30,
                    Height = 30,
                    BackColor = cor,
                    Location = new Point(0, 5)
                };

                Label lblColor = new Label
                {
                    Text = nome,
                    ForeColor = Color.White,
                    AutoSize = true,
                    Location = new Point(40, 10)
                };

                colorContainer.Controls.AddRange(new Control[] { colorBox, lblColor });
                colorContainer.Cursor = Cursors.Hand;

                colorContainer.Click += (s, e) =>
                {
                    robot.Customization.Cor = nome;
                    foreach (Control c in colorsFlow.Controls)
                    {
                        c.BackColor = Color.Transparent;
                    }
                    colorContainer.BackColor = Color.FromArgb(70, 70, 70);
                };

                colorsFlow.Controls.Add(colorContainer);
            }

            colorPanel.Controls.Add(lblTitle);
            colorPanel.Controls.Add(colorsFlow);
            return colorPanel;
        }

        private Panel CreateVoiceSelection()
        {
            Panel voicePanel = CreateCustomSection("Voz do Robô", new[] { "Cristiano Ronaldo", "Marta" });
            voicePanel.Tag = "voice";
            return voicePanel;
        }

        private Panel CreateSizeSelection()
        {
            Panel sizePanel = CreateCustomSection("Tamanho", new[] { "1.80m", "1.90m", "2.00m" });
            sizePanel.Tag = "size";
            return sizePanel;
        }

        private Panel CreateWeightSelection()
        {
            Panel weightPanel = CreateCustomSection("Peso", new[] { "80kg", "90kg", "100kg" });
            weightPanel.Tag = "weight";
            return weightPanel;
        }

        private Panel CreateCustomSection(string title, string[] options)
        {
            Panel panel = new Panel
            {
                Width = 800,
                Height = 100,
                Margin = new Padding(0, 0, 0, 20)
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(0, 0)
            };

            ComboBox comboBox = new ComboBox
            {
                Location = new Point(0, 40),
                Width = 300,
                Font = new Font("Segoe UI", 12),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(57, 57, 57),
                ForeColor = Color.White
            };
            comboBox.Items.AddRange(options);

            comboBox.SelectedIndexChanged += (s, e) =>
            {
                string selected = comboBox.SelectedItem.ToString();
                switch (panel.Tag.ToString())
                {
                    case "voice":
                        robot.Customization.Voz = selected;
                        break;
                    case "size":
                        robot.Customization.Tamanho = selected;
                        break;
                    case "weight":
                        robot.Customization.Peso = selected;
                        break;
                }
            };

            panel.Controls.Add(lblTitle);
            panel.Controls.Add(comboBox);
            return panel;
        }

        private Panel CreateAddonsSelection()
        {
            Panel addonsPanel = new Panel
            {
                Width = 800,
                Height = 150,
                Margin = new Padding(0, 0, 0, 20)
            };

            Label lblTitle = new Label
            {
                Text = "Recursos Adicionais",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(0, 0)
            };

            CheckBox cbReplay = new CheckBox
            {
                Text = "Replay (+R$ 10.000,00)",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.White,
                Location = new Point(0, 40),
                AutoSize = true
            };
            cbReplay.CheckedChanged += (s, e) =>
            {
                robot.Customization.TemReplay = cbReplay.Checked;
                UpdatePrecoTotal();
            };

            CheckBox cbMonitoramento = new CheckBox
            {
                Text = "Monitoramento de Desempenho (+R$ 10.000,00)",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.White,
                Location = new Point(0, 70),
                AutoSize = true
            };
            cbMonitoramento.CheckedChanged += (s, e) =>
            {
                robot.Customization.TemMonitoramento = cbMonitoramento.Checked;
                UpdatePrecoTotal();
            };

            addonsPanel.Controls.Add(lblTitle);
            addonsPanel.Controls.Add(cbReplay);
            addonsPanel.Controls.Add(cbMonitoramento);
            return addonsPanel;
        }

        private void UpdatePrecoTotal()
        {
            precoTotal = robot.CalcularPrecoTotal();
            lblPrecoTotal.Text = $"R$ {precoTotal:N2}";
        }

        private void BtnAddToCart_Click(object sender, EventArgs e)
        {
            if (ValidateCustomizations())
            {
                try
                {
                    CartService.AddToCart(robot, robot.Customization);
                    MessageBox.Show("Produto adicionado ao carrinho!", "Sucesso",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Parent.Controls.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao adicionar ao carrinho: {ex.Message}",
                        "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateCustomizations()
        {
            if (string.IsNullOrEmpty(robot.Customization.Cor) ||
                string.IsNullOrEmpty(robot.Customization.Voz) ||
                string.IsNullOrEmpty(robot.Customization.Tamanho) ||
                string.IsNullOrEmpty(robot.Customization.Peso))
            {
                MessageBox.Show("Por favor, selecione todas as opções de personalização.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
    }
}