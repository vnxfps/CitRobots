using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CitRobots.Models;
using CitRobots.Services;

namespace CitRobots.Views
{
    public partial class CartForm : Form
    {
        private Panel mainPanel;
        private FlowLayoutPanel cartItemsPanel;
        private Panel summaryPanel;
        private TextBox txtCupom;
        private Label lblTotal;
        private decimal totalComDesconto;

        public CartForm()
        {
            InitializeComponent();
            SetupCartForm();
            LoadCartItems();
        }

        private void SetupCartForm()
        {
            this.BackColor = Color.FromArgb(45, 45, 45);
            this.Dock = DockStyle.Fill;

            TableLayoutPanel mainContainer = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(30),
                BackColor = Color.FromArgb(45, 45, 45)
            };

            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));

            Panel leftPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 0, 20, 0)
            };

            Label lblTitle = new Label
            {
                Text = "Seu Carrinho",
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Top,
                Height = 60
            };

            cartItemsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(0, 20, 0, 0)
            };

            leftPanel.Controls.Add(cartItemsPanel);
            leftPanel.Controls.Add(lblTitle);

            summaryPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = Color.FromArgb(35, 35, 35)
            };

            SetupSummaryPanel();

            mainContainer.Controls.Add(leftPanel, 0, 0);
            mainContainer.Controls.Add(summaryPanel, 1, 0);

            this.Controls.Add(mainContainer);
        }

        private void SetupSummaryPanel()
        {
            Label lblSummaryTitle = new Label
            {
                Text = "Resumo do Pedido",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Top,
                Height = 40
            };

            Panel couponPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                Padding = new Padding(0, 20, 0, 20)
            };

            Label lblCupom = new Label
            {
                Text = "Cupom de Desconto",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.White,
                Location = new Point(0, 0),
                AutoSize = true
            };

            txtCupom = new TextBox
            {
                Location = new Point(0, 30),
                Size = new Size(150, 35),
                Font = new Font("Segoe UI", 12),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Button btnAplicarCupom = new Button
            {
                Text = "Aplicar",
                Location = new Point(160, 30),
                Size = new Size(100, 35),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(92, 184, 92),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11),
                Cursor = Cursors.Hand
            };
            btnAplicarCupom.FlatAppearance.BorderSize = 0;
            btnAplicarCupom.Click += BtnAplicarCupom_Click;

            couponPanel.Controls.AddRange(new Control[] { lblCupom, txtCupom, btnAplicarCupom });

            Panel valuesPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                Padding = new Padding(0, 20, 0, 20)
            };

            lblTotal = new Label
            {
                Text = $"Total: R$ {CartService.GetTotal():N2}",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(0, 40),
                AutoSize = true
            };

            valuesPanel.Controls.Add(lblTotal);

            Button btnCheckout = new Button
            {
                Text = "Finalizar Compra",
                Dock = DockStyle.Bottom,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(57, 57, 57),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCheckout.FlatAppearance.BorderSize = 0;
            btnCheckout.Click += BtnCheckout_Click;

            summaryPanel.Controls.Add(btnCheckout);
            summaryPanel.Controls.Add(valuesPanel);
            summaryPanel.Controls.Add(couponPanel);
            summaryPanel.Controls.Add(lblSummaryTitle);
        }

        private void AddCartItemPanel(CartItemModel item)
        {
            Panel itemPanel = new Panel
            {
                Width = cartItemsPanel.Width - 40,
                Height = 150,
                Margin = new Padding(0, 0, 0, 15),
                BackColor = Color.FromArgb(35, 35, 35),
                Padding = new Padding(20)
            };

            PictureBox productImage = new PictureBox
            {
                Size = new Size(100, 100),
                Location = new Point(20, 25),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };

            try
            {
                string resourcePath = $"{Application.StartupPath}\\Resources\\Images\\{item.Robot.Nome.ToLower()}.jpg";
                if (File.Exists(resourcePath))
                {
                    productImage.Image = Image.FromFile(resourcePath);
                }
                else
                {
                    throw new FileNotFoundException();
                }
            }
            catch
            {
                Label lblNoImage = new Label
                {
                    Text = "Imagem não\ndisponível",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    ForeColor = Color.FromArgb(150, 150, 150)
                };
                productImage.Controls.Add(lblNoImage);
            }

            Label lblRobotName = new Label
            {
                Text = item.Robot.Nome,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(140, 20),
                AutoSize = true
            };

            string customizations = $"Personalizações: {item.Customization.Cor}, {item.Customization.Voz}, " +
                                  $"{item.Customization.Tamanho}, {item.Customization.Peso}";
            if (item.Customization.TemReplay) customizations += ", Replay";
            if (item.Customization.TemMonitoramento) customizations += ", Monitoramento";

            Label lblCustomizations = new Label
            {
                Text = customizations,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(200, 200, 200),
                Location = new Point(140, 50),
                AutoSize = true
            };

            Label lblPrice = new Label
            {
                Text = $"R$ {item.PrecoTotal:N2}",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(itemPanel.Width - 150, 20),
                AutoSize = true
            };

            Button btnRemove = new Button
            {
                Text = "Remover",
                Size = new Size(100, 35),
                Location = new Point(itemPanel.Width - 150, 90),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(220, 53, 69),
                Font = new Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };
            btnRemove.FlatAppearance.BorderColor = Color.FromArgb(220, 53, 69);

            btnRemove.Click += (s, e) =>
            {
                CartService.RemoveFromCart(cartItemsPanel.Controls.GetChildIndex(itemPanel));
                LoadCartItems();
            };

            itemPanel.Controls.AddRange(new Control[] {
            productImage, lblRobotName, lblCustomizations,
            lblPrice, btnRemove
            });

            cartItemsPanel.Controls.Add(itemPanel);
        }

        private void LoadCartItems()
        {
            cartItemsPanel.Controls.Clear();
            var items = CartService.GetCartItems();

            foreach (var item in items)
            {
                AddCartItemPanel(item);
            }

            UpdateTotal();
        }

        private void BtnAplicarCupom_Click(object sender, EventArgs e)
        {
            string cupom = txtCupom.Text.Trim();
            if (string.IsNullOrEmpty(cupom))
            {
                MessageBox.Show("Por favor, insira um cupom.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            totalComDesconto = CartService.AplicarDesconto(cupom);
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            decimal total = totalComDesconto > 0 ? totalComDesconto : CartService.GetTotal();
            lblTotal.Text = $"Total: R$ {total:N2}";
        }

        private void BtnCheckout_Click(object sender, EventArgs e)
        {
            if (CartService.GetCartItems().Count == 0)
            {
                MessageBox.Show("Carrinho vazio.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var stockService = new StockService();
                var items = CartService.GetCartItems();

                foreach (var item in items)
                {
                    if (!stockService.VerificarEstoque(item.Robot.Id, 1))
                    {
                        MessageBox.Show($"Produto {item.Robot.Nome} sem estoque.", "Erro",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                DatabaseService db = new DatabaseService();
                decimal total = totalComDesconto > 0 ? totalComDesconto : CartService.GetTotal();
                int clienteId = 1;

                int pedidoId = db.SalvarPedido(items, clienteId, total, txtCupom.Text);

                if (pedidoId > 0)
                {
                    foreach (var item in items)
                    {
                        stockService.DiminuirEstoque(item.Robot.Id, 1);
                    }

                    var paymentForm = new PaymentForm(total, pedidoId);
                    if (paymentForm.ShowDialog() == DialogResult.OK)
                    {
                        MessageBox.Show("Pedido realizado com sucesso!", "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CartService.ClearCart();
                        LoadCartItems();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao finalizar pedido: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}