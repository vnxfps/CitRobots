using System;
using System.Drawing;
using System.Windows.Forms;

namespace CitRobots.Views
{
    public partial class ProfileForm : Form
    {
        private Panel menuPanel;
        private Panel contentPanel;
        private Label lblTitle;

        public ProfileForm()
        {
            InitializeComponent();
            SetupProfileForm();
        }

        private void SetupProfileForm()
        {
            this.Text = "Perfil do Usuário";
            this.BackColor = Color.FromArgb(35, 35, 35);
            this.Size = new Size(850, 600);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;

            menuPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 200,
                BackColor = Color.FromArgb(45, 45, 45),
                Padding = new Padding(10)
            };

            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(45, 45, 45),
                Padding = new Padding(20)
            };

            lblTitle = new Label
            {
                Text = "Meu Perfil",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter
            };

            this.Controls.Add(contentPanel);
            this.Controls.Add(menuPanel);
            contentPanel.Controls.Add(lblTitle);

            SetupMenu();
            ShowProfileData(null, null);
        }

        private Button CreateMenuButton(string text, EventHandler onClick)
        {
            Button btn = new Button
            {
                Text = text,
                Dock = DockStyle.Top,
                Height = 50,
                Font = new Font("Segoe UI", 12),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += onClick;
            return btn;
        }

        private void SetupMenu()
        {
            Label lblMenu = new Label
            {
                Text = "Menu",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter
            };
            menuPanel.Controls.Add(lblMenu);

            menuPanel.Controls.Add(CreateMenuButton("🛒 Meus Pedidos", ShowOrders));
            menuPanel.Controls.Add(CreateMenuButton("📍 Alterar Endereço", ShowAddress));
            menuPanel.Controls.Add(CreateMenuButton("👤 Alterar Informações", ShowProfileData));
        }

        private void ShowProfileData(object sender, EventArgs e)
        {
            contentPanel.Controls.Clear();
            lblTitle.Text = "👤 Alterar Informações do Perfil";

            Panel profilePanel = CreateContentPanel();

            int yPos = 20;
            AddProfileField(profilePanel, "Nome:", ref yPos);
            AddProfileField(profilePanel, "Sobrenome:", ref yPos);
            AddProfileField(profilePanel, "Email:", ref yPos);
            AddProfileField(profilePanel, "CPF:", ref yPos);
            AddProfileField(profilePanel, "Telefone:", ref yPos);

            Button btnSave = CreateActionButton("Salvar Alterações", "💾", yPos + 20);
            profilePanel.Controls.Add(btnSave);

            contentPanel.Controls.Add(profilePanel);
        }

        private void ShowAddress(object sender, EventArgs e)
        {
            contentPanel.Controls.Clear();
            lblTitle.Text = "📍 Alterar Endereço Principal";

            Panel addressPanel = CreateContentPanel();

            Label lblEndereco = new Label
            {
                Text = "Endereço Principal:",
                Location = new Point(20, 20),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.White
            };

            TextBox txtEndereco = new TextBox
            {
                Location = new Point(20, 50),
                Size = new Size(400, 100),
                Font = new Font("Segoe UI", 12),
                Multiline = true,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Button btnSaveAddress = CreateActionButton("Salvar Endereço", "📥", 170);
            addressPanel.Controls.Add(lblEndereco);
            addressPanel.Controls.Add(txtEndereco);
            addressPanel.Controls.Add(btnSaveAddress);

            contentPanel.Controls.Add(addressPanel);
        }

        private void ShowOrders(object sender, EventArgs e)
        {
            contentPanel.Controls.Clear();
            lblTitle.Text = "🛒 Meus Pedidos";

            Panel ordersPanel = CreateContentPanel();

            Label lblPedidos = new Label
            {
                Text = "Seus Pedidos Recentes:",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Top,
                Height = 30
            };

            ordersPanel.Controls.Add(lblPedidos);

            for (int i = 1; i <= 5; i++)
            {
                Label lblOrder = new Label
                {
                    Text = $"Pedido #{i} - {DateTime.Now.AddDays(-i):dd/MM/yyyy} - Status: Enviado - Total: R$ {100 + i * 10}",
                    Font = new Font("Segoe UI", 12),
                    ForeColor = Color.White,
                    Dock = DockStyle.Top,
                    Padding = new Padding(5),
                    BackColor = Color.FromArgb(50, 50, 50)
                };
                ordersPanel.Controls.Add(lblOrder);
            }

            contentPanel.Controls.Add(ordersPanel);
        }

        private Panel CreateContentPanel()
        {
            return new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = Color.FromArgb(35, 35, 35),
                AutoScroll = true
            };
        }

        private Button CreateActionButton(string text, string emoji, int yPos)
        {
            return new Button
            {
                Text = $"{emoji} {text}",
                Size = new Size(200, 40),
                Location = new Point((contentPanel.Width - 200) / 2, yPos),
                BackColor = Color.FromArgb(92, 184, 92),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            }.Apply(b => b.FlatAppearance.BorderSize = 0);
        }

        private void AddProfileField(Panel panel, string labelText, ref int yPos)
        {
            Label lbl = new Label
            {
                Text = labelText,
                Location = new Point(20, yPos),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.White
            };

            TextBox txt = new TextBox
            {
                Location = new Point(20, yPos + 30),
                Size = new Size(400, 30),
                Font = new Font("Segoe UI", 12),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            panel.Controls.Add(lbl);
            panel.Controls.Add(txt);
            yPos += 80;
        }
    }

    public static class Extensions
    {
        public static T Apply<T>(this T target, Action<T> action)
        {
            action(target);
            return target;
        }
    }
}