using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CitRobots.Models;
using CitRobots.Services;

namespace CitRobots.Views
{
    public partial class RegisterForm : Form
    {
        private Panel registerPanel;
        private TextBox txtNome;
        private TextBox txtSobrenome;
        private TextBox txtEmail;
        private MaskedTextBox txtCPF;
        private DateTimePicker dtpNascimento;
        private MaskedTextBox txtTelefone;
        private TextBox txtEndereco;
        private TextBox txtSenha;
        private TextBox txtConfirmaSenha;
        private Button btnRegistrar;
        private Button btnVoltar;

        public RegisterForm()
        {
            InitializeComponent();
            SetupRegisterForm();
            CenterToScreen();
        }

        private void SetupRegisterForm()
        {
            this.BackColor = Color.FromArgb(45, 45, 45);
            this.Size = new Size(800, 900);
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
                Text = "Registre-se",
                Font = new Font("Segoe UI", 32, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(400, 60)
            };
            lblTitle.Location = new Point((this.ClientSize.Width - lblTitle.Width) / 2, logoBox.Bottom + 20);

            registerPanel = new Panel
            {
                Width = 500,
                Height = 700, // Aumentei altura
                BackColor = Color.FromArgb(35, 35, 35)
            };
            registerPanel.Location = new Point((this.ClientSize.Width - registerPanel.Width) / 2, lblTitle.Bottom + 30);

            registerPanel.Paint += (s, e) => {
                var rect = new Rectangle(0, 0, registerPanel.Width - 1, registerPanel.Height - 1);
                using (var pen = new Pen(Color.FromArgb(92, 184, 92), 2))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
            };

            SetupFormControls();

            mainContainer.Controls.AddRange(new Control[] {
               logoBox, lblTitle, registerPanel
           });

            this.Controls.Add(mainContainer);

            this.Resize += (s, e) =>
            {
                logoBox.Location = new Point((this.ClientSize.Width - logoBox.Width) / 2, 20);
                lblTitle.Location = new Point((this.ClientSize.Width - lblTitle.Width) / 2, logoBox.Bottom + 20);
                registerPanel.Location = new Point((this.ClientSize.Width - registerPanel.Width) / 2, lblTitle.Bottom + 30);
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

            TextBox CreateTextBox(Point location)
            {
                return new TextBox
                {
                    Location = location,
                    Size = new Size(360, 35),
                    Font = new Font("Segoe UI", 12),
                    BackColor = Color.FromArgb(45, 45, 45),
                    ForeColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle
                };
            }

            int yPos = 40;
            int spacing = 65; // Aumentei espaçamento

            registerPanel.Controls.Add(CreateLabel("Nome:", new Point(70, yPos)));
            txtNome = CreateTextBox(new Point(70, yPos + 25));
            registerPanel.Controls.Add(txtNome);

            yPos += spacing;
            registerPanel.Controls.Add(CreateLabel("Sobrenome:", new Point(70, yPos)));
            txtSobrenome = CreateTextBox(new Point(70, yPos + 25));
            registerPanel.Controls.Add(txtSobrenome);

            yPos += spacing;
            registerPanel.Controls.Add(CreateLabel("Email:", new Point(70, yPos)));
            txtEmail = CreateTextBox(new Point(70, yPos + 25));
            registerPanel.Controls.Add(txtEmail);

            yPos += spacing;
            registerPanel.Controls.Add(CreateLabel("CPF:", new Point(70, yPos)));
            txtCPF = new MaskedTextBox
            {
                Mask = "000,000,000-00",
                Location = new Point(70, yPos + 25),
                Size = new Size(360, 35),
                Font = new Font("Segoe UI", 12),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            registerPanel.Controls.Add(txtCPF);

            yPos += spacing;
            registerPanel.Controls.Add(CreateLabel("Data de Nascimento:", new Point(70, yPos)));
            dtpNascimento = new DateTimePicker
            {
                Location = new Point(70, yPos + 25),
                Size = new Size(360, 35),
                Font = new Font("Segoe UI", 12),
                Format = DateTimePickerFormat.Short
            };
            registerPanel.Controls.Add(dtpNascimento);

            yPos += spacing;
            registerPanel.Controls.Add(CreateLabel("Telefone:", new Point(70, yPos)));
            txtTelefone = new MaskedTextBox
            {
                Mask = "(00) 00000-0000",
                Location = new Point(70, yPos + 25),
                Size = new Size(360, 35),
                Font = new Font("Segoe UI", 12),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            registerPanel.Controls.Add(txtTelefone);

            yPos += spacing;
            registerPanel.Controls.Add(CreateLabel("Endereço:", new Point(70, yPos)));
            txtEndereco = CreateTextBox(new Point(70, yPos + 25));
            registerPanel.Controls.Add(txtEndereco);

            yPos += spacing;
            registerPanel.Controls.Add(CreateLabel("Senha:", new Point(70, yPos)));
            txtSenha = CreateTextBox(new Point(70, yPos + 25));
            txtSenha.UseSystemPasswordChar = true;
            registerPanel.Controls.Add(txtSenha);

            yPos += spacing;
            registerPanel.Controls.Add(CreateLabel("Confirmar Senha:", new Point(70, yPos)));
            txtConfirmaSenha = CreateTextBox(new Point(70, yPos + 25));
            txtConfirmaSenha.UseSystemPasswordChar = true;
            registerPanel.Controls.Add(txtConfirmaSenha);

            yPos += spacing;
            btnRegistrar = new Button
            {
                Text = "Criar Conta",
                Location = new Point(70, yPos + 15),
                Size = new Size(360, 45),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(92, 184, 92),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRegistrar.FlatAppearance.BorderSize = 0;
            btnRegistrar.Click += BtnRegistrar_Click;
            registerPanel.Controls.Add(btnRegistrar);

            yPos += spacing + 10;
            btnVoltar = new Button
            {
                Text = "Voltar",
                Location = new Point(70, yPos + 15),
                Size = new Size(360, 35),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(57, 57, 57),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12),
                Cursor = Cursors.Hand
            };
            btnVoltar.FlatAppearance.BorderSize = 0;
            btnVoltar.Click += BtnVoltar_Click;
            registerPanel.Controls.Add(btnVoltar);
        }

        private void BtnRegistrar_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                var user = new UserModel
                {
                    Nome = txtNome.Text,
                    Sobrenome = txtSobrenome.Text,
                    Email = txtEmail.Text,
                    CPF = txtCPF.Text.Replace(".", "").Replace("-", "").Trim(),
                    DataNascimento = dtpNascimento.Value.ToString("yyyy-MM-dd"),
                    Telefone = txtTelefone.Text,
                    Endereco = txtEndereco.Text,
                    Senha = txtSenha.Text
                };

                DatabaseService db = new DatabaseService();
                int userId = db.RegisterUser(user);

                if (userId > 0)
                {
                    MessageBox.Show("Conta criada com sucesso!", "Sucesso",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    new LoginForm().Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao criar conta: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnVoltar_Click(object sender, EventArgs e)
        {
            this.Close();
            new LoginForm().Show();
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                string.IsNullOrWhiteSpace(txtSobrenome.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtEndereco.Text) ||
                string.IsNullOrWhiteSpace(txtSenha.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmaSenha.Text))
            {
                MessageBox.Show("Todos os campos são obrigatórios.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!Regex.IsMatch(txtEmail.Text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                MessageBox.Show("Email inválido.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            string cpf = txtCPF.Text.Replace(".", "").Replace("-", "").Trim();
            if (cpf.Length < 11)
            {
                MessageBox.Show("CPF inválido. Digite todos os números.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtSenha.Text != txtConfirmaSenha.Text)
            {
                MessageBox.Show("As senhas não coincidem.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dtpNascimento.Value.AddYears(18) > DateTime.Now)
            {
                MessageBox.Show("É necessário ter pelo menos 18 anos.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}