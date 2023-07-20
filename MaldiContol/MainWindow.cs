using System;
using System.Windows.Forms;

namespace MaldiContol
{
    public partial class MainWindow : Form
    {
        private MaldiController _maldiController;
        public void SetMaldiControlInterface(MaldiController UseThisInterface)
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
        public TextBox XMotorPositionDisplay()
        {
            return XMotorPositionTextBox;
        }
        public TextBox YMotorPositionDisplay()
        {
            return YMotorPositionTextBox;
        }
        public TextBox LockStatusDisplay()
        {
            return LockStatusTextBox;
        }

        public TextBox PumpingStateDisplay()
        {
            return PumpingStateTextBox;
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
