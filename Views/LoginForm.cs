using System;
using System.Drawing;
using System.Windows.Forms;
using CitRobots.Services;

namespace CitRobots.Views
{
    public partial class LoginForm : Form
    {
        private Panel loginPanel;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private Button btnLogin;
        private LinkLabel lnkRegister;

        public LoginForm()
        {
            InitializeComponent();
            SetupLoginForm();
            CenterToScreen();
        }

        private void SetupLoginForm()
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
                Text = "Login",
                Font = new Font("Segoe UI", 32, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(400, 60)
            };
            lblTitle.Location = new Point((this.ClientSize.Width - lblTitle.Width) / 2, logoBox.Bottom + 20);

            loginPanel = new Panel
            {
                Width = 500,
                Height = 350,
                BackColor = Color.FromArgb(35, 35, 35)
            };
            loginPanel.Location = new Point((this.ClientSize.Width - loginPanel.Width) / 2, lblTitle.Bottom + 30);

            loginPanel.Paint += (s, e) =>
            {
                var rect = new Rectangle(0, 0, loginPanel.Width - 1, loginPanel.Height - 1);
                using (var pen = new Pen(Color.FromArgb(92, 184, 92), 2))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
            };

            SetupFormControls();

            mainContainer.Controls.AddRange(new Control[] {
                logoBox, lblTitle, loginPanel
            });

            this.Controls.Add(mainContainer);

            this.Resize += (s, e) =>
            {
                logoBox.Location = new Point((this.ClientSize.Width - logoBox.Width) / 2, 20);
                lblTitle.Location = new Point((this.ClientSize.Width - lblTitle.Width) / 2, logoBox.Bottom + 20);
                loginPanel.Location = new Point((this.ClientSize.Width - loginPanel.Width) / 2, lblTitle.Bottom + 30);
            };
        }

        private void SetupFormControls()
        {
            Label CreateLabel(string text, Point location)
            {
                return new Label
                {
                    Text = text,
                    Location = location,
                    Size = new Size(200, 25),
                    Font = new Font("Segoe UI", 12),
                    ForeColor = Color.White
                };
            }

            TextBox CreateTextBox(Point location, bool isPassword = false)
            {
                return new TextBox
                {
                    Location = location,
                    Size = new Size(360, 35),
                    Font = new Font("Segoe UI", 12),
                    BackColor = Color.FromArgb(45, 45, 45),
                    ForeColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    UseSystemPasswordChar = isPassword
                };
            }

            int yPos = 40;
            int spacing = 65;

            loginPanel.Controls.Add(CreateLabel("Email:", new Point(70, yPos)));
            txtEmail = CreateTextBox(new Point(70, yPos + 25));
            loginPanel.Controls.Add(txtEmail);

            yPos += spacing;
            loginPanel.Controls.Add(CreateLabel("Senha:", new Point(70, yPos)));
            txtPassword = CreateTextBox(new Point(70, yPos + 25), true);
            loginPanel.Controls.Add(txtPassword);

            yPos += spacing;
            btnLogin = new Button
            {
                Text = "Entrar",
                Location = new Point(70, yPos + 15),
                Size = new Size(360, 45),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(92, 184, 92),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;
            loginPanel.Controls.Add(btnLogin);

            yPos += spacing + 10;
            lnkRegister = new LinkLabel
            {
                Text = "Não tem uma conta? Registre-se",
                Location = new Point(70, yPos + 15),
                Font = new Font("Segoe UI", 10),
                LinkColor = Color.White,
                ActiveLinkColor = Color.LightGray,
                AutoSize = true
            };
            lnkRegister.Click += LnkRegister_Click;
            loginPanel.Controls.Add(lnkRegister);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            // Validação básica
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Por favor, preencha todos os campos.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DatabaseService db = new DatabaseService();
                var user = db.ValidateLogin(txtEmail.Text, txtPassword.Text);

                if (user != null)
                {
                    MessageBox.Show("Login realizado com sucesso!", "Sucesso",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    new MainForm().Show();
                }
                else
                {
                    MessageBox.Show("Email ou senha incorretos.", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao realizar login: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LnkRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            new RegisterForm().Show();
        }
    }
}
