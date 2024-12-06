using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CitRobots.Models;
using CitRobots.Services;

namespace CitRobots.Views
{
    public partial class PaymentForm : Form
    {
        private decimal totalPedido;
        private int pedidoId;
        private Panel mainPanel;
        private ComboBox cmbTipoPagamento;
        private Panel pnlDadosPagamento;

        public PaymentForm(decimal total, int pedidoId)
        {
            InitializeComponent();
            this.totalPedido = total;
            this.pedidoId = pedidoId;
            SetupPaymentForm();
        }

        private void SetupPaymentForm()
        {
            this.Text = "Pagamento";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(57, 57, 57);
            this.ForeColor = Color.White;

            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            Label lblTitle = new Label
            {
                Text = "Finalizar Pagamento",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Top,
                Height = 50
            };

            Label lblTotal = new Label
            {
                Text = $"Total a Pagar: R$ {totalPedido:N2}",
                Font = new Font("Segoe UI", 14F),
                ForeColor = Color.White,
                Location = new Point(20, 70),
                AutoSize = true
            };

            Label lblTipoPagamento = new Label
            {
                Text = "Forma de Pagamento:",
                Font = new Font("Segoe UI", 12F),
                ForeColor = Color.White,
                Location = new Point(20, 120),
                AutoSize = true
            };

            cmbTipoPagamento = new ComboBox
            {
                Location = new Point(20, 150),
                Size = new Size(200, 30),
                Font = new Font("Segoe UI", 12F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbTipoPagamento.Items.AddRange(new object[] { "Cartão", "PIX", "Boleto" });
            cmbTipoPagamento.SelectedIndexChanged += CmbTipoPagamento_SelectedIndexChanged;

            pnlDadosPagamento = new Panel
            {
                Location = new Point(20, 200),
                Size = new Size(540, 200),
                BackColor = Color.FromArgb(45, 45, 45)
            };

            Button btnConfirmar = new Button
            {
                Text = "Confirmar Pagamento",
                Size = new Size(200, 40),
                Location = new Point(20, 420),
                BackColor = Color.FromArgb(70, 70, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnConfirmar.Click += BtnConfirmar_Click;

            mainPanel.Controls.AddRange(new Control[] {
                lblTitle,
                lblTotal,
                lblTipoPagamento,
                cmbTipoPagamento,
                pnlDadosPagamento,
                btnConfirmar
            });

            this.Controls.Add(mainPanel);
        }

        private void CmbTipoPagamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlDadosPagamento.Controls.Clear();
            string tipoPagamento = cmbTipoPagamento.SelectedItem.ToString();

            switch (tipoPagamento)
            {
                case "Cartão":
                    SetupCartaoForm();
                    break;
                case "PIX":
                    SetupPixForm();
                    break;
                case "Boleto":
                    SetupBoletoForm();
                    break;
            }
        }

        private void SetupCartaoForm()
        {
            AddField("Número do Cartão:", new MaskedTextBox { Mask = "0000 0000 0000 0000" }, 20);
            AddField("Nome no Cartão:", new TextBox(), 80);
            AddField("Validade:", new MaskedTextBox { Mask = "00/00" }, 140);
            AddField("CVV:", new MaskedTextBox { Mask = "000" }, 200);
        }

        private void SetupPixForm()
        {
            Label lblPix = new Label
            {
                Text = "Escaneie o QR Code para pagar:",
                Location = new Point(20, 20),
                AutoSize = true,
                ForeColor = Color.White
            };

            PictureBox qrCode = new PictureBox
            {
                Size = new Size(150, 150),
                Location = new Point(20, 50),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            pnlDadosPagamento.Controls.AddRange(new Control[] { lblPix, qrCode });
        }

        private void SetupBoletoForm()
        {
            Label lblBoleto = new Label
            {
                Text = "Clique para gerar o boleto:",
                Location = new Point(20, 20),
                AutoSize = true,
                ForeColor = Color.White
            };

            Button btnGerar = new Button
            {
                Text = "Gerar Boleto",
                Location = new Point(20, 50),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(70, 70, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            pnlDadosPagamento.Controls.AddRange(new Control[] { lblBoleto, btnGerar });
        }

        private void AddField(string label, Control control, int yPosition)
        {
            Label lbl = new Label
            {
                Text = label,
                Location = new Point(20, yPosition),
                AutoSize = true,
                ForeColor = Color.White
            };

            control.Location = new Point(20, yPosition + 25);
            control.Size = new Size(250, 25);
            control.Font = new Font("Segoe UI", 10F);

            pnlDadosPagamento.Controls.AddRange(new Control[] { lbl, control });
        }

        private void BtnConfirmar_Click(object sender, EventArgs e)
        {
            if (cmbTipoPagamento.SelectedIndex < 0)
            {
                MessageBox.Show("Selecione uma forma de pagamento.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var payment = new PaymentModel
                {
                    PedidoId = this.pedidoId,
                    TipoPagamento = cmbTipoPagamento.SelectedItem.ToString(),
                    Valor = this.totalPedido,
                    Status = "PENDENTE"
                };

                MessageBox.Show("Pagamento processado com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao processar pagamento: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}