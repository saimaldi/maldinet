using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaldiContol;
using MaldiRabbit;

namespace WPFExample
{
    /// <summary>
    /// Create a class to hide windows crap UI architecture
    /// </summary>
    class AsyncTextDisplay
    {
        private readonly TextBox TextElement;

        public AsyncTextDisplay(TextBox thisText)
        {
            TextElement = thisText;
        }
        private delegate void InvokeDelegate(string message);

        private void update_text(string message)
        {
            TextElement.Text = message;
        }

        public void SetText(string val)
        {
            TextElement.Dispatcher.Invoke(new InvokeDelegate(update_text), val);

        }
    }

    /// <summary>
    /// Create an instance of the Message interface to interact with the WPF gui
    /// </summary>

    public class WPFMessageInterface : MessageInterface
    {
        readonly AsyncTextDisplay TextDisplay;
        readonly AsyncTextDisplay XMotorPositionDisplay;
        readonly AsyncTextDisplay YMotorPositionDisplay;
        readonly AsyncTextDisplay LockStatusDisplay;
        readonly AsyncTextDisplay PumpingStateDisplay;

        /// <summary>
        /// The constructor connects the dialog components 
        /// to our internal asynctextdisplay objects.  This dialog must 
        /// be loaded and the components must exist before it is passed to this consturctor
        /// </summary>
        /// <param name="tellme">The windows dialog object which contains the ui elements
        /// </param>
        public WPFMessageInterface(MainWindow tellme)
        {
            TextDisplay = new AsyncTextDisplay(tellme.LastMessageTextBox);
            XMotorPositionDisplay = new AsyncTextDisplay(tellme.XPosition);
            YMotorPositionDisplay = new AsyncTextDisplay(tellme.YPosition);
            LockStatusDisplay = new AsyncTextDisplay(tellme.LockStatusDisplay);
            PumpingStateDisplay = new AsyncTextDisplay(tellme.PumpingStateDisplay);
        }

        /// <summary>
        /// Internal processing method for stage messages
        /// </summary>
        /// <param name="words">the array of words containing the message</param>
        private void ProcessStageMessage(string[] words)
        {
            if (words[3].Equals("POSITION"))
            {
                if (words[2].Equals("X"))
                {
                    XMotorPositionDisplay.SetText(words[4]);
                }
                else
                {
                    if (words[2].Equals("Y"))
                    {
                        YMotorPositionDisplay.SetText(words[4]);
                    }
                }


            }
        }

        /// <summary>
        /// Internal processiong of lock status messages
        /// </summary>
        /// <param name="words">the array of words containing the message</param>
        private void ProcessLockStatusMessage(String[] words)
        {
            LockStatusDisplay.SetText(words[2]);
        }

        /// <summary>
        /// Internal processiong of pumping state messages
        /// </summary>
        /// <param name="words">the array of words containing the message</param>
        private void ProcessPumpingStateMessage(String[] words)
        {
            PumpingStateDisplay.SetText(words[2]);
        }

        /// <summary>
        /// The target of the subscription to incoming messages from the Maldi Instrument
        /// 
        /// The message is converted to a consistent case and then split into the individual words
        /// these words are then parsed and the relevant messages are passed onto methods which 
        /// update the ui
        /// </summary>
        /// <param name="Message"></param>
        public void ProcessMessage(String Message)
        {
            TextDisplay.SetText(Message);
            var words = Message.ToUpper().Split();
            // we look for messages of the form Stage Motor X Position interpreted_position actual_position
            if (words.Length >= 6)
            {
                if (words[0].Equals("STAGE"))
                {
                    ProcessStageMessage(words);
                }
            }
            if (words.Length > 2)
            {
                // LOCK STATUS STOPPED_CLOSED
                // LOCK STATUS STOPPED_OPEN
                if (words[0].Equals("LOCK") && words[1].Equals("STATUS"))
                {
                    ProcessLockStatusMessage(words);
                }
                //PUMPING STATE state
                if (words[0].Equals("PUMPING") && words[1].Equals("STATE"))
                {
                    ProcessPumpingStateMessage(words);
                }
            }
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MaldiController controller;
        private readonly MaldiReader reader;

        public MainWindow()
        {
            InitializeComponent();

            // create the maldi interface objects after the main window is constructed so that we can connec them up
            controller = new MaldiController("rabbitmq-server");

            //Create a maldi reader to receive messages from the Maldi instrument
            reader = new MaldiReader("rabbitmq-server");

            // connect the reader to the UI interface
            reader.SetMessageInterface(new WPFMessageInterface(this));

        }

        private void OpenLockButtonClicked(object sender, RoutedEventArgs e)
        {
            controller.SampleOut();
        }

        private void CloseLockButtonClicked(object sender, RoutedEventArgs e)
        {
            controller.SampleIn();
        }
    }
}
