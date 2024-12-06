using CitRobots.Services;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace CitRobots.Views
{
    public partial class MainForm : Form
    {
        private Panel topPanel;
        private Panel contentPanel;
        private Form currentForm;

        public MainForm()
        {
            InitializeComponent();
            SetupMainForm();
            LoadHomeForm();
        }

        private void SetupMainForm()
        {
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(45, 45, 45);

            topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(45, 45, 45)
            };

            try
            {
                PictureBox logo = new PictureBox
                {
                    Image = Image.FromFile("Resources/Images/Logo.png"),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Width = 150,
                    Height = 40,
                    Location = new Point(20, 10)
                };
                topPanel.Controls.Add(logo);
                logo.Cursor = Cursors.Hand;
                logo.Click += (s, e) => LoadForm(new AdminStockForm());
            }
            catch
            {
                Label lblLogo = new Label
                {
                    Text = "CT ROBOTS",
                    Font = new Font("Segoe UI", 16, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(20, 15),
                    AutoSize = true
                };
                topPanel.Controls.Add(lblLogo);
            }

            FlowLayoutPanel centerMenu = new FlowLayoutPanel
            {
                AutoSize = true,
                Location = new Point(200, 0),
                Height = 60,
                Padding = new Padding(10),
                BackColor = Color.Transparent
            };

            string[] menuItems = { "Home", "Produtos", "Carrinho", "Sobre Nós" };
            foreach (string item in menuItems)
            {
                Button btn = new Button
                {
                    Text = item,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.Transparent,
                    ForeColor = Color.White,
                    Size = new Size(100, 40),
                    Margin = new Padding(5, 0, 5, 0),
                    Font = new Font("Segoe UI", 10),
                    Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += MenuButton_Click;
                centerMenu.Controls.Add(btn);
            }

            Button btnCadastro = new Button
            {
                Text = "Cadastro",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(92, 184, 92),
                ForeColor = Color.White,
                Size = new Size(100, 35),
                Location = new Point(this.Width - 320, 12),
                Font = new Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };
            btnCadastro.FlatAppearance.BorderSize = 0;
            btnCadastro.Click += (s, e) => LoadForm(new RegisterForm());

            Button btnLogin = new Button
            {
                Text = "Login",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                Size = new Size(100, 35),
                Location = new Point(this.Width - 215, 12),
                Font = new Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += (s, e) => LoadForm(new LoginForm());

            Button btnPerfil = new Button
            {
                Text = "Perfil",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                Size = new Size(100, 35),
                Location = new Point(this.Width - 110, 12),
                Font = new Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };
            btnPerfil.FlatAppearance.BorderSize = 0;
            btnPerfil.Click += (s, e) => LoadForm(new ProfileForm());

            topPanel.Controls.Add(btnCadastro);
            topPanel.Controls.Add(btnLogin);
            topPanel.Controls.Add(btnPerfil);
            topPanel.Controls.Add(centerMenu);

            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(45, 45, 45)
            };

            this.Controls.Add(contentPanel);
            this.Controls.Add(topPanel);
            this.Resize += (s, e) =>
            {
                btnCadastro.Location = new Point(this.Width - 320, 12);
                btnLogin.Location = new Point(this.Width - 215, 12);
                btnPerfil.Location = new Point(this.Width - 110, 12);
            };
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Text)
            {
                case "Home":
                    LoadForm(new HomeForm());
                    break;
                case "Produtos":
                    LoadForm(new ProductsForm());
                    break;
                case "Carrinho":
                    LoadForm(new CartForm());
                    break;
                case "Sobre Nós":
                    LoadForm(new SobreNosForm());
                    break;
            }
        }

        private void LoadForm(Form form)
        {
            if (currentForm != null)
            {
                currentForm.Close();
            }

            currentForm = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(form);
            form.Show();
        }

        private void LoadHomeForm()
        {
            LoadForm(new HomeForm());
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit();
        }
    }
}