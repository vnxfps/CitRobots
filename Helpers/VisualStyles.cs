using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace CitRobots.Helpers
{
    public static class VisualStyles
    {
        public static Color PrimaryColor = Color.FromArgb(57, 57, 57);
        public static Color SecondaryColor = Color.FromArgb(45, 45, 45);
        public static Color AccentColor = Color.FromArgb(70, 70, 70);
        public static Color TextColor = Color.White;

        public static Font TitleFont = new Font("Segoe UI", 24F, FontStyle.Bold);
        public static Font SubtitleFont = new Font("Segoe UI", 16F, FontStyle.Regular);
        public static Font ButtonFont = new Font("Segoe UI", 10F, FontStyle.Regular);
        public static Font TextFont = new Font("Segoe UI", 10F, FontStyle.Regular);

        public static void ApplyButtonStyle(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = AccentColor;
            button.ForeColor = TextColor;
            button.Font = ButtonFont;
            button.Cursor = Cursors.Hand;
            button.Padding = new Padding(10);

            button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(80, 80, 80);
            button.MouseLeave += (s, e) => button.BackColor = AccentColor;
        }

        public static void ApplyTextBoxStyle(TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Font = TextFont;
            textBox.BackColor = SecondaryColor;
            textBox.ForeColor = TextColor;
        }

        public static void CreateRoundedPanel(Panel panel, int radius)
        {
            panel.Paint += (s, e) =>
            {
                using (var brush = new SolidBrush(panel.BackColor))
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    var rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
                    var path = Drawing2D.RoundedRectangle(rect, radius);
                    e.Graphics.FillPath(brush, path);
                }
            };
        }
    }

    public static class Drawing2D
    {
        public static System.Drawing.Drawing2D.GraphicsPath RoundedRectangle(Rectangle rect, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}