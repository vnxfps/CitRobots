using System;
using System.Drawing;
using System.Windows.Forms;
using CitRobots.Helpers;
using System.Collections.Generic;

namespace CitRobots.Views
{
    public partial class AdminStockForm : Form
    {
        private Panel loginPanel;
        private Panel stockPanel;
        private DataGridView gridStock;
        private DatabaseHelper _db;

        private readonly Dictionary<string, string> adminCredentials = new Dictionary<string, string>
       {
           {"CitNoah", "NoahADM1"},
           {"CitGirata", "GirataADM1"},
           {"CitGuilherme", "GuilhermeADM1"},
           {"CitKadu", "KaduADM1"},
           {"CitGiovanni", "GiovanniADM1"},
           {"CitLuan", "LuanADM1"}
       };

        public AdminStockForm()
        {
            InitializeComponent();
            _db = new DatabaseHelper();
            SetupLoginPanel();
            CenterToScreen();
        }

        private void SetupLoginPanel()
        {
            this.BackColor = Color.FromArgb(45, 45, 45);
            this.Size = new Size(800, 700);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            Panel mainContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(45, 45, 45),
                Padding = new Padding(20)
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
                Text = "Administradores",
                Font = new Font("Segoe UI", 32, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(400, 60)
            };
            lblTitle.Location = new Point((this.ClientSize.Width - lblTitle.Width) / 2, logoBox.Bottom + 20);

            loginPanel = new Panel
            {
                Width = 500,
                Height = 400,
                BackColor = Color.FromArgb(35, 35, 35)
            };
            loginPanel.Location = new Point((this.ClientSize.Width - loginPanel.Width) / 2, lblTitle.Bottom + 30);

            loginPanel.Paint += (s, e) => {
                var rect = new Rectangle(0, 0, loginPanel.Width - 1, loginPanel.Height - 1);
                using (var pen = new Pen(Color.FromArgb(92, 184, 92), 2))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
            };

            Label lblLogin = new Label
            {
                Text = "Login:",
                ForeColor = Color.White,
                Location = new Point(70, 60),
                Size = new Size(100, 30),
                Font = new Font("Segoe UI", 16)
            };

            TextBox txtLogin = new TextBox
            {
                Location = new Point(70, 100),
                Size = new Size(360, 40),
                Font = new Font("Segoe UI", 16),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblSenha = new Label
            {
                Text = "Senha:",
                ForeColor = Color.White,
                Location = new Point(70, 170),
                Size = new Size(100, 30),
                Font = new Font("Segoe UI", 16)
            };

            TextBox txtSenha = new TextBox
            {
                Location = new Point(70, 210),
                Size = new Size(360, 40),
                Font = new Font("Segoe UI", 16),
                PasswordChar = '•',
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Button btnLogin = new Button
            {
                Text = "Entrar",
                Location = new Point(70, 290),
                Size = new Size(360, 50),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(92, 184, 92),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += (s, e) => ValidateLogin(txtLogin.Text, txtSenha.Text);

            loginPanel.Controls.AddRange(new Control[] {
               lblLogin, txtLogin, lblSenha, txtSenha, btnLogin
           });

            mainContainer.Controls.AddRange(new Control[] {
               logoBox, lblTitle, loginPanel
           });

            this.Controls.Add(mainContainer);

            // Ajusta posições dos elementos quando o formulário é redimensionado
            this.Resize += (s, e) =>
            {
                logoBox.Location = new Point((this.ClientSize.Width - logoBox.Width) / 2, 20);
                lblTitle.Location = new Point((this.ClientSize.Width - lblTitle.Width) / 2, logoBox.Bottom + 20);
                loginPanel.Location = new Point((this.ClientSize.Width - loginPanel.Width) / 2, lblTitle.Bottom + 30);
            };
        }

        private void ValidateLogin(string login, string senha)
        {
            if (adminCredentials.ContainsKey(login) && adminCredentials[login] == senha)
            {
                loginPanel.Parent.Visible = false;
                this.Size = new Size(800, 600);
                this.CenterToScreen();
                SetupStockPanel();
                LoadStockData();
            }
            else
            {
                MessageBox.Show("Login ou senha incorretos.", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupStockPanel()
        {
            stockPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(45, 45, 45),
                Padding = new Padding(20)
            };

            Label lblStockTitle = new Label
            {
                Text = "Gerenciamento de Estoque",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Button btnRefresh = new Button
            {
                Text = "Atualizar",
                BackColor = Color.FromArgb(92, 184, 92),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(120, 40),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 80),
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => LoadStockData();

            gridStock = new DataGridView
            {
                Location = new Point(20, 140),
                Size = new Size(Width - 60, Height - 180),
                BackgroundColor = Color.FromArgb(57, 57, 57),
                ForeColor = Color.White,
                GridColor = Color.FromArgb(70, 70, 70),
                BorderStyle = BorderStyle.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                EditMode = DataGridViewEditMode.EditOnEnter,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                Font = new Font("Segoe UI", 12)
            };

            gridStock.DefaultCellStyle.BackColor = Color.FromArgb(57, 57, 57);
            gridStock.DefaultCellStyle.SelectionBackColor = Color.FromArgb(92, 184, 92);
            gridStock.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45);
            gridStock.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            gridStock.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            gridStock.EnableHeadersVisualStyles = false;

            gridStock.CellValidating += GridStock_CellValidating;
            gridStock.CellEndEdit += GridStock_CellEndEdit;

            stockPanel.Controls.AddRange(new Control[] { lblStockTitle, btnRefresh, gridStock });
            this.Controls.Add(stockPanel);
        }

        private void LoadStockData()
        {
            string query = @"
               SELECT 
                   id as 'ID',
                   nome as 'Nome do Robô',
                   codigo_modelo as 'Código',
                   CONCAT('R$ ', FORMAT(preco, 2, 'pt-BR')) as 'Preço',
                   quantidade_estoque as 'Estoque'
               FROM citrobots.robos
               ORDER BY nome";

            var dt = _db.ExecuteSelect(query);

            if (dt != null && dt.Rows.Count > 0)
            {
                gridStock.DataSource = dt;
                gridStock.Columns["ID"].Visible = false;
                gridStock.Columns["Estoque"].ReadOnly = false;
                gridStock.Columns["Nome do Robô"].ReadOnly = true;
                gridStock.Columns["Código"].ReadOnly = true;
                gridStock.Columns["Preço"].ReadOnly = true;

                foreach (DataGridViewColumn col in gridStock.Columns)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            else
            {
                MessageBox.Show("Nenhum dado encontrado.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void GridStock_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (gridStock.Columns[e.ColumnIndex].Name == "Estoque")
            {
                if (!int.TryParse(e.FormattedValue.ToString(), out int newValue) || newValue < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Digite um número inteiro maior ou igual a 0.",
                        "Valor Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void GridStock_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (gridStock.Columns[e.ColumnIndex].Name != "Estoque") return;

            var row = gridStock.Rows[e.RowIndex];
            int robotId = Convert.ToInt32(row.Cells["ID"].Value);
            int newQuantity = Convert.ToInt32(row.Cells["Estoque"].Value);

            string query = "UPDATE citrobots.robos SET quantidade_estoque = @Quantity WHERE id = @RobotId";
            var parameters = new Dictionary<string, object>
           {
               { "@RobotId", robotId },
               { "@Quantity", newQuantity }
           };

            try
            {
                _db.ExecuteUpdate(query, parameters);
                LoadStockData();
            }
            catch
            {
                MessageBox.Show("Erro ao atualizar estoque. Tente novamente.",
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadStockData();
            }
        }
    }
}