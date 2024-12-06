using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace CitRobots.Views
{
    public partial class SobreNosForm : Form
    {
        public SobreNosForm()
        {
            InitializeComponent();
            SetupSobreNosForm();
        }

        private void SetupSobreNosForm()
        {
            this.BackColor = Color.FromArgb(45, 45, 45);
            this.Dock = DockStyle.Fill;

            // Container principal com scroll
            Panel mainContainer = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(0, 0, 0, 30)
            };

            // Título Principal
            Label lblTitle = CreateHeaderLabel("Quem nós somos?", 42);
            lblTitle.Margin = new Padding(0, 50, 0, 20);

            // Descrição Principal
            Label lblMainDescription = CreateDescriptionLabel(
                "Somos a CT ROBOTS, uma startup dedicada a revolucionar o treinamento esportivo " +
                "de alto nível com robôs avançados. Fornecemos soluções de última geração para " +
                "equipes de futebol das principais divisões, otimizando seu desempenho e eficiência " +
                "nos treinos. Nossos robôs simulam movimentos precisos, aprimoram táticas e habilidades, " +
                "oferecendo uma abordagem tecnológica que supera as limitações convencionais de treino " +
                "e eleva a preparação esportiva a novos patamares."
            );

            // Slogan
            Label lblSlogan = new Label
            {
                Text = "\"CT ROBOTS, elevando seu jogo.\"",
                Font = new Font("Segoe UI", 24, FontStyle.Italic),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 30, 0, 30)
            };

            // Seção História
            Panel historySection = CreateSection(
                "Nossa História",
                "Ambição, Revolução e Inovação",
                "A história da CT ROBOTS começou com a paixão por esportes e tecnologia de seus " +
                "fundadores, que viram a oportunidade de unir essas áreas para criar uma solução " +
                "inovadora no futebol profissional. Identificando a necessidade de ferramentas mais " +
                "precisas e inteligentes para treinamentos, eles reuniram uma equipe especializada " +
                "para desenvolver robôs altamente especializados.",
                "team.jpg"
            );

            // Seção Protótipos
            Panel prototypesSection = CreateSection(
                "Protótipos",
                "Modernidade desde seu início",
                "Desde os primeiros protótipos, nos dedicamos a entender as demandas específicas " +
                "dos treinadores e atletas de alto rendimento, refinando continuamente seus produtos " +
                "para atender às exigências do futebol moderno. Hoje, nossos robôs são reconhecidos " +
                "por sua precisão e capacidade de simular situações reais de jogo, consolidando a " +
                "CT ROBOTS como uma parceira estratégica para times que buscam uma vantagem competitiva.",
                "prototype.jpg"
            );

            // Adicionar elementos ao container
            mainContainer.Controls.Add(prototypesSection);
            mainContainer.Controls.Add(historySection);
            mainContainer.Controls.Add(lblSlogan);
            mainContainer.Controls.Add(lblMainDescription);
            mainContainer.Controls.Add(lblTitle);

            this.Controls.Add(mainContainer);
        }

        private Label CreateHeaderLabel(string text, int fontSize)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", fontSize, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 80
            };
        }

        private Label CreateDescriptionLabel(string text)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 16),
                ForeColor = Color.FromArgb(230, 230, 230),
                TextAlign = ContentAlignment.TopCenter,
                Dock = DockStyle.Top,
                Padding = new Padding(100, 20, 100, 20),
                AutoSize = true
            };
        }

        private Panel CreateSection(string title, string subtitle, string description, string imagePath)
        {
            Panel section = new Panel
            {
                Dock = DockStyle.Top,
                Height = 400,
                Margin = new Padding(50),
                Padding = new Padding(20)
            };

            // Container do conteúdo
            TableLayoutPanel content = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.FromArgb(35, 35, 35),
                Padding = new Padding(30)
            };

            content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));

            // Painel de texto
            Panel textPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Top,
                Height = 50
            };

            Label lblSubtitle = new Label
            {
                Text = subtitle,
                Font = new Font("Segoe UI", 16, FontStyle.Italic),
                ForeColor = Color.FromArgb(92, 184, 92),
                Dock = DockStyle.Top,
                Height = 30
            };

            Label lblDescription = new Label
            {
                Text = description,
                Font = new Font("Segoe UI", 14),
                ForeColor = Color.FromArgb(200, 200, 200),
                Dock = DockStyle.Fill,
                AutoSize = true
            };

            textPanel.Controls.AddRange(new Control[] { lblDescription, lblSubtitle, lblTitle });

            // Imagem
            PictureBox pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                Margin = new Padding(20)
            };

            try
            {
                pictureBox.Image = Image.FromFile($"Resources/Images/{imagePath}");
            }
            catch
            {
                Label lblImagePlaceholder = new Label
                {
                    Text = "Imagem não disponível",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    ForeColor = Color.White
                };
                pictureBox.Controls.Add(lblImagePlaceholder);
            }

            // Adicionar elementos ao layout
            content.Controls.Add(textPanel, 0, 0);
            content.Controls.Add(pictureBox, 1, 0);
            section.Controls.Add(content);

            return section;
        }
    }
}