using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CitRobots.Models;
using CitRobots.Services;

namespace CitRobots.Views
{
    public partial class ProfileForm : Form
    {
        private UserModel currentUser;
        private TabControl tabControl;
        private Panel profilePanel;

        public ProfileForm()
        {
            InitializeComponent();
            SetupProfileForm();
            LoadUserData();
            CenterToScreen();
        }

        private void SetupProfileForm()
        {
            this.BackColor = Color.FromArgb(45, 45, 45);
            this.Size = new Size(800, 600);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            Panel mainContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(45, 45, 45),
                Padding = new Padding(20),
                AutoScroll = true
            };

            PictureBox logoBox = new PictureBox
            {
                Size = new Size(200, 100),
                SizeMode = PictureBoxSizeMode.Zoom
            };
            logoBox.Location = new Point((this.ClientSize.Width - logoBox.Width) / 2, 20);

            try
            {
                logoBox.Image = Image.FromFile("Resources/Images/Logo.png");
            }
            catch
            {
                logoBox.Visible = false;
            }

            Label lblTitle = new Label
            {
                Text = "Meu Perfil",
                Font = new Font("Segoe UI", 32, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(400, 60)
            };
            lblTitle.Location = new Point((this.ClientSize.Width - lblTitle.Width) / 2, logoBox.Bottom + 20);

            profilePanel = new Panel
            {
                Width = 700,
                Height = 450,
                BackColor = Color.FromArgb(35, 35, 35)
            };
            profilePanel.Location = new Point((this.ClientSize.Width - profilePanel.Width) / 2, lblTitle.Bottom + 30);

            profilePanel.Paint += (s, e) =>
            {
                var rect = new Rectangle(0, 0, profilePanel.Width - 1, profilePanel.Height - 1);
                using (var pen = new Pen(Color.FromArgb(92, 184, 92), 2))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
            };

            SetupTabs();

            mainContainer.Controls.AddRange(new Control[] { logoBox, lblTitle, profilePanel });
            this.Controls.Add(mainContainer);

            this.Resize += (s, e) =>
            {
                logoBox.Location = new Point((this.ClientSize.Width - logoBox.Width) / 2, 20);
                lblTitle.Location = new Point((this.ClientSize.Width - lblTitle.Width) / 2, logoBox.Bottom + 20);
                profilePanel.Location = new Point((this.ClientSize.Width - profilePanel.Width) / 2, lblTitle.Bottom + 30);
            };
        }

        private void SetupTabs()
        {
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12F),
                Padding = new Point(10, 10)
            };

            TabPage tabDados = new TabPage("Dados Pessoais")
            {
                BackColor = Color.FromArgb(45, 45, 45)
            };

            TabPage tabPedidos = new TabPage("Meus Pedidos")
            {
                BackColor = Color.FromArgb(45, 45, 45)
            };

            TabPage tabEndereco = new TabPage("Endereços")
            {
                BackColor = Color.FromArgb(45, 45, 45)
            };

            SetupDadosPessoais(tabDados);
            SetupPedidos(tabPedidos);
            SetupEnderecos(tabEndereco);

            tabControl.TabPages.AddRange(new TabPage[] { tabDados, tabPedidos, tabEndereco });
            profilePanel.Controls.Add(tabControl);
        }

        private void SetupDadosPessoais(TabPage tab)
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = Color.FromArgb(35, 35, 35)
            };

            int yPos = 20;
            AddProfileField(panel, "Nome:", ref yPos);
            AddProfileField(panel, "Sobrenome:", ref yPos);
            AddProfileField(panel, "Email:", ref yPos);
            AddProfileField(panel, "CPF:", ref yPos);
            AddProfileField(panel, "Telefone:", ref yPos);

            Button btnSalvar = new Button
            {
                Text = "Salvar Alterações",
                Size = new Size(360, 45),
                Location = new Point(20, yPos + 15),
                BackColor = Color.FromArgb(92, 184, 92),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSalvar.FlatAppearance.BorderSize = 0;
            btnSalvar.Click += BtnSalvar_Click;

            panel.Controls.Add(btnSalvar);
            tab.Controls.Add(panel);
        }

        private void AddProfileField(Panel panel, string labelText, ref int yPos)
        {
            Label lbl = new Label
            {
                Text = labelText,
                Location = new Point(20, yPos),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.White
            };

            TextBox txt = new TextBox
            {
                Location = new Point(20, yPos + 25),
                Size = new Size(360, 35),
                Font = new Font("Segoe UI", 12),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            panel.Controls.AddRange(new Control[] { lbl, txt });
            yPos += 65;
        }

        private void SetupPedidos(TabPage tab)
        {
            ListView listPedidos = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };

            listPedidos.Columns.AddRange(new ColumnHeader[]
            {
                new ColumnHeader { Text = "Número", Width = 100 },
                new ColumnHeader { Text = "Data", Width = 100 },
                new ColumnHeader { Text = "Status", Width = 100 },
                new ColumnHeader { Text = "Total", Width = 100 }
            });

            tab.Controls.Add(listPedidos);
        }

        private void SetupEnderecos(TabPage tab)
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = Color.FromArgb(35, 35, 35)
            };

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
                Size = new Size(360, 100),
                Font = new Font("Segoe UI", 12),
                Multiline = true,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Button btnSalvarEndereco = new Button
            {
                Text = "Salvar Endereço",
                Size = new Size(360, 45),
                Location = new Point(20, 170),
                BackColor = Color.FromArgb(92, 184, 92),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSalvarEndereco.FlatAppearance.BorderSize = 0;

            panel.Controls.AddRange(new Control[] { lblEndereco, txtEndereco, btnSalvarEndereco });
            tab.Controls.Add(panel);
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            // Implementação para salvar alterações.
        }

        private void LoadUserData()
        {
            // Carregar dados do usuário atual.
        }
    }
}
