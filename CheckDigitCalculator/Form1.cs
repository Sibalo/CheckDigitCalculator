using CheckDigitCalculator.App.Services.Interfaces;
using CheckDigitCalculator.Application.Services.Implementations;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CheckDigitCalculator
{
    public partial class Form1 : Form
    {
        private TextBox[] serialNumberBoxes = new TextBox[8];
        private TextBox txtSum;
        private readonly ICheckDigitService _checkDigitService;
        private readonly InputNavigationService _inputService;
        public Form1(ICheckDigitService checkDigitService, InputNavigationService inputService)
        {
            InitializeComponent();
            _checkDigitService = checkDigitService;
            _inputService = inputService;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Label lblSerialNumberPrompt = new Label();
            lblSerialNumberPrompt.Text = "Please enter the 8-digit serial number:";
            lblSerialNumberPrompt.Font = new Font("Segoe UI", 12);
            lblSerialNumberPrompt.AutoSize = true;
            lblSerialNumberPrompt.Location = new Point(20, 9);
            this.Controls.Add(lblSerialNumberPrompt);

            for (int i = 0; i < serialNumberBoxes.Length; i++)
            {
                serialNumberBoxes[i] = new TextBox();
                serialNumberBoxes[i].Width = 45;
                serialNumberBoxes[i].Height = 50;
                serialNumberBoxes[i].Font = new Font("Segoe UI", 16);
                serialNumberBoxes[i].MaxLength = 1;
                serialNumberBoxes[i].Location = new Point(20 + (i * 50), 46);
                serialNumberBoxes[i].TextAlign = HorizontalAlignment.Center;

                serialNumberBoxes[i].KeyPress += TextBox_KeyPress;
                serialNumberBoxes[i].Enter += (s, a) => MoveFocusToFirstEmpty();
                this.Controls.Add(serialNumberBoxes[i]);
            }

            serialNumberBoxes[0].Focus();
            int startX = serialNumberBoxes[0].Left;
            int endX = serialNumberBoxes[serialNumberBoxes.Length - 1].Right;

            Label lblSerialNumber = new Label();
            lblSerialNumber.Text = "Serial\nNo";
            lblSerialNumber.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblSerialNumber.AutoSize = true;
            lblSerialNumber.TextAlign = ContentAlignment.TopCenter;
            lblSerialNumber.Left = startX + ((endX - startX - lblSerialNumber.Width) / 2) + (9 * 3);
            lblSerialNumber.Top = serialNumberBoxes[0].Bottom + 9;
            this.Controls.Add(lblSerialNumber);

            Panel serialNumberLine = new Panel();
            serialNumberLine.Height = 1;
            serialNumberLine.Width = endX - startX;
            serialNumberLine.Left = startX;
            serialNumberLine.Top = serialNumberBoxes[0].Bottom + 25 + 30;
            serialNumberLine.BackColor = Color.Black;
            this.Controls.Add(serialNumberLine);

            txtSum = new TextBox();
            txtSum.Width = 45;
            txtSum.Height = 50;
            txtSum.Text = "?";
            txtSum.BackColor = Color.LightGreen;
            txtSum.Font = new Font("Segoe UI", 16);
            txtSum.Location = new Point(20 + (8 * 50), 46);
            txtSum.ReadOnly = true;
            txtSum.TextAlign = HorizontalAlignment.Center;
            this.Controls.Add(txtSum);

            Label lblCheckDigit = new Label();
            lblCheckDigit.Text = "Check\nDigit";
            lblCheckDigit.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblCheckDigit.AutoSize = true;
            lblCheckDigit.Left = txtSum.Left;
            lblCheckDigit.Top = txtSum.Bottom + 9;
            lblCheckDigit.TextAlign = ContentAlignment.TopCenter;
            this.Controls.Add(lblCheckDigit);

            Panel checkDigitLine = new Panel();
            checkDigitLine.Height = 1;
            checkDigitLine.Width = txtSum.Width;
            checkDigitLine.Left = txtSum.Left;
            checkDigitLine.Top = txtSum.Bottom + 25 + 30;
            checkDigitLine.BackColor = Color.Black;
            this.Controls.Add(checkDigitLine);
        }
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
                return;
            }
            if (e.KeyChar == (char)Keys.Back)
            {
                if (box.Text == "")
                {
                    MoveBack(box);
                    txtSum.Text = "?";
                }
                else
                {
                    box.Clear();
                    txtSum.Text = "?";
                }
            }
            if (char.IsDigit(e.KeyChar))
            {
                box.Text = e.KeyChar.ToString();
                MoveNext(box);
                e.Handled = true;
            }
        }
        private void MoveNext(TextBox current)
        {
            var nextIndex = _inputService.GetNextIndex(serialNumberBoxes, current);
            if (nextIndex.HasValue) { serialNumberBoxes[nextIndex.Value].Focus(); }
            else { if (_inputService.AllFilled(serialNumberBoxes)) Calculate(); }
        }
        private void Calculate()
        {
            txtSum.Text = "?";
            var values = _inputService.ExtractValues(serialNumberBoxes);
            var result = _checkDigitService.CalculateCheckDigit(values);
            txtSum.Text = result.ToString();
        }
        private void MoveBack(TextBox current)
        {
            var previousIndex = _inputService.GetPreviousIndex(serialNumberBoxes, current);
            if (previousIndex.HasValue)
            {
                serialNumberBoxes[previousIndex.Value].Clear();
                serialNumberBoxes[previousIndex.Value].Focus();
            }
            txtSum.Text = "?";
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            MoveFocusToFirstEmpty();
        }
        private void MoveFocusToFirstEmpty()
        {
            foreach (var box in serialNumberBoxes)
            {
                if (string.IsNullOrEmpty(box.Text))
                {
                    box.Focus();
                    return;
                }
            }
        }
    }
}
