using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CitRobots.CustomControls
{
    public class LoadingOverlay : Form
    {
        private Label lblMessage;
        private PictureBox pictureBox;

        public LoadingOverlay(string message = "Carregando...")
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(0, 0, 0, 150);
            this.Opacity = 0.8;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(200, 100);

            lblMessage = new Label
            {
                Text = message,
                ForeColor = Color.White,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            lblMessage.Location = new Point(
                (this.Width - lblMessage.Width) / 2,
                (this.Height - lblMessage.Height) / 2
            );

            this.Controls.Add(lblMessage);
        }

        public static void Show(Form parent, string message = "Carregando...")
        {
            LoadingOverlay overlay = new LoadingOverlay(message);
            overlay.Owner = parent;
            overlay.Show();
        }
    }
}