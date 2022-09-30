using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace E_Badge
{
    public partial class Form1 : Form
    {
        private RadioButton selectedSpeed;
        public Form1()
        {
            InitializeComponent();
            this.radioButton1.CheckedChanged += new EventHandler(speedButton_CheckedChanged);
            this.radioButton2.CheckedChanged += new EventHandler(speedButton_CheckedChanged);
            this.radioButton3.CheckedChanged += new EventHandler(speedButton_CheckedChanged);
            this.radioButton4.CheckedChanged += new EventHandler(speedButton_CheckedChanged);
            this.radioButton5.CheckedChanged += new EventHandler(speedButton_CheckedChanged);
            this.selectedSpeed = this.radioButton3;
        }

        void speedButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb == null)
            {
                MessageBox.Show("Sender is not a RadioButton");
                return;
            }

            // Ensure that the RadioButton.Checked property
            // changed to true.
            if (rb.Checked)
            {
                // Keep track of the selected RadioButton by saving a reference
                // to it.
                selectedSpeed = rb;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int page = (int)this.page.Value;
            int repeat = (int)this.repeat.Value;
            int speed = int.Parse(selectedSpeed.Text);
            String message = this.textBox1.Text.Substring(0, Math.Min(this.textBox1.Text.Length,150));
            SerialPort _serialPort = new SerialPort("COM3", 1200, Parity.None, 8, StopBits.One);
            _serialPort.RtsEnable = true;
            _serialPort.Handshake = Handshake.XOnXOff;
            _serialPort.Open();
            String str = String.Format("{0:X2}{1:X2}{2:X2}{3:X2}{4}", page, repeat, (4 - speed) * 16 + 1, message.Length, message);
            byte[] bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(str);
            //_serialPort.Write(bytes, 0, bytes.Length);
            for (int i=0; i<bytes.Length; i++)
            {
                _serialPort.Write(bytes, i, 1);
                Thread.Sleep(50);
            }
            _serialPort.Close();
        }
    }
}
