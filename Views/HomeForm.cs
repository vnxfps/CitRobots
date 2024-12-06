using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace CitRobots.Views
{
    public partial class HomeForm : Form
    {
        private Point originalSloganPosition;
        private Point originalSubtitlePosition;
        private Label lblSlogan;
        private Label lblSubtitle;

        public HomeForm()
        {
            InitializeComponent();
            SetupHomeForm();
        }

        private void SetupHomeForm()
        {
            this.BackColor = Color.FromArgb(45, 45, 45);
            this.Dock = DockStyle.Fill;

            // Container principal com scroll
            Panel mainContainer = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            // Painel do Banner
            Panel bannerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 400,
                BackColor = Color.FromArgb(40, 40, 40)
            };

            // Gradiente de fundo para o banner
            bannerPanel.Paint += (s, e) =>
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    bannerPanel.ClientRectangle,
                    Color.FromArgb(40, 40, 40),
                    Color.FromArgb(30, 30, 30),
                    45F))
                {
                    e.Graphics.FillRectangle(brush, bannerPanel.ClientRectangle);
                }
            };

            // Slogan
            lblSlogan = new Label
            {
                Text = "Transforme cada treino em uma vitória!",
                Font = new Font("Segoe UI", 42, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(50, 120),
                BackColor = Color.Transparent
            };
            originalSloganPosition = lblSlogan.Location;

            // Subtítulo
            lblSubtitle = new Label
            {
                Text = "Liberte o potencial máximo do seu time com CitRobots — \nmais precisão, mais inteligência, mais vitórias.",
                Font = new Font("Segoe UI", 20),
                ForeColor = Color.FromArgb(200, 200, 200),
                AutoSize = true,
                Location = new Point(50, 220),
                BackColor = Color.Transparent
            };
            originalSubtitlePosition = lblSubtitle.Location;

            bannerPanel.Controls.AddRange(new Control[] { lblSlogan, lblSubtitle });

            // Cards Panel
            TableLayoutPanel cardsPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 350,
                Padding = new Padding(50, 30, 50, 30),
                BackColor = Color.FromArgb(45, 45, 45),
                ColumnCount = 3,
                RowCount = 1
            };

            // Configurar colunas dos cards
            for (int i = 0; i < 3; i++)
            {
                cardsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            }

            // Adicionar cards
            string[] titles = { "Inovação", "Performance", "Durabilidade" };
            string[] descriptions = {
                "Projeto para treinar.\nE revolucionar.",
                "Potencialize seu\ndesempenho.",
                "Preparado para todo\ntreino."
            };
            string[] images = { "robot.png", "performance.png", "durability.png" };

            for (int i = 0; i < 3; i++)
            {
                cardsPanel.Controls.Add(CreateCard(titles[i], descriptions[i], images[i]), i, 0);
            }

            mainContainer.Controls.Add(cardsPanel);
            mainContainer.Controls.Add(bannerPanel);

            this.Controls.Add(mainContainer);

            this.MouseMove += HomeForm_MouseMove;
        }

        private Panel CreateCard(string title, string description, string imagePath)
        {
            Panel card = new Panel
            {
                BackColor = Color.White,
                Margin = new Padding(10),
                Padding = new Padding(20),
                Dock = DockStyle.Fill
            };

            card.Paint += (s, e) =>
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    int radius = 10;
                    Rectangle rect = card.ClientRectangle;
                    path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
                    path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
                    path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
                    path.CloseFigure();

                    card.Region = new Region(path);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                }
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 45, 45),
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblDescription = new Label
            {
                Text = description,
                Font = new Font("Segoe UI", 14),
                ForeColor = Color.FromArgb(70, 70, 70),
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter
            };

            PictureBox pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };

            try
            {
                string resourcePath = $"Resources.Images.{imagePath}";
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                using (var stream = assembly.GetManifestResourceStream(resourcePath))
                {
                    if (stream != null)
                    {
                        pictureBox.Image = Image.FromStream(stream);
                    }
                    else
                    {
                        pictureBox.Image = Image.FromFile($"Resources/Images/{imagePath}");
                    }
                }
            }
            catch
            {
                Label lblImage = new Label
                {
                    Text = title,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 16),
                    ForeColor = Color.FromArgb(100, 100, 100)
                };
                pictureBox.Controls.Add(lblImage);
            }

            card.MouseEnter += (s, e) => {
                card.BackColor = Color.FromArgb(245, 245, 245);
                card.Padding = new Padding(15);
            };

            card.MouseLeave += (s, e) => {
                card.BackColor = Color.White;
                card.Padding = new Padding(20);
            };

            card.Controls.Add(pictureBox);
            card.Controls.Add(lblDescription);
            card.Controls.Add(lblTitle);

            return card;
        }

        private void HomeForm_MouseMove(object sender, MouseEventArgs e)
        {
            int offsetX = (e.X - this.Width / 2) / 30;
            int offsetY = (e.Y - this.Height / 2) / 30;

            lblSlogan.Location = new Point(
                originalSloganPosition.X + offsetX,
                originalSloganPosition.Y + offsetY
            );

            lblSubtitle.Location = new Point(
                originalSubtitlePosition.X - offsetX / 2,
                originalSubtitlePosition.Y - offsetY / 2
            );
        }
    }
}