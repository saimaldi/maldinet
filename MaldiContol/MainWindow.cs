using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaldiContol
{
    public partial class MainWindow : Form
    {
        private MaldiController _maldiController;
        public void setMaldiControlInterface(MaldiController UseThisInterface)
        {
            _maldiController = UseThisInterface;
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        public TextBox LastMessageTextBox()
        {
            return LastMessage;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ExchangeSampleClicked(object sender, EventArgs e)
        {
            if (_maldiController != null)
                _maldiController.SampleOut();
        }

        private void PumpSampleInletButton_Click(object sender, EventArgs e)
        {
            if (_maldiController != null)
                _maldiController.SampleIn();
        }
    }
}
