using MaldiNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaldiContol
{
    /// <summary>
    /// This class connects to a Maldi Instrument and reads the instrument 
    /// status messages.  It calls the 
    /// ProcessMessage method of the supplied messageinterface whenever a 
    /// new message is received
    /// </summary>
    public class MaldiReader
    {
        private MessageInterface messageInterface;
        private RabbitMQConnection connection;

        /// <summary>
        /// This receives the messages from the Maldireaderclass
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="messageargument">The message</param>
        private void MessageReceiver(object sender, RabbitMQMessageEventArgs messageargument)
        {
            messageInterface.ProcessMessage(messageargument.Message);
        }

        /// <summary>
        /// Creates and instance of a maldireader and connects it to the 
        /// reporting exchange of the maldi instrument.
        /// 
        /// Call SetMessageInterface to receive updates as the messages are
        /// processed.
        /// </summary>
        public MaldiReader(string hostname = "10.1.234.1")
        {

            RabbitMQConnectionDetails connectionParameters = new RabbitMQConnectionDetails();
            connectionParameters.Host = hostname;
            connectionParameters.SetReportingExchange();
            connection = new RabbitMQConnection();
            connection.Connect(connectionParameters, true);
        }

        /// <summary>
        /// Installs the instance of the message interface which will
        /// receive calls to ProcessMessage for each message the MaldiReader Receives
        /// </summary>
        /// <param name="myMessageInterface"></param>
        public void SetMessageInterface(MessageInterface myMessageInterface)
        {
            messageInterface = myMessageInterface;
            connection.SubscribeToNewMessages(MessageReceiver);
        }

    }

    /// <summary>
    /// Provides and interface for applications to set commands to the MALDI Instrument
    /// </summary>
    public class MaldiController
    {
        private RabbitMQConnection connection;
        public MaldiController(string hostname="10.1.234.1")
        {

            RabbitMQConnectionDetails connectionParameters = new RabbitMQConnectionDetails();
            connectionParameters.Host = hostname;
            connectionParameters.SetControlExchange();
            connection = new RabbitMQConnection();
            connection.Connect(connectionParameters, false);
        }

        /// <summary>
        /// Request the instrument moves the sample to the sample
        /// inlet allows the user to change the sample plate
        /// </summary>
        public void SampleOut()
        {
            connection.SendMessage("ADVISE FRONTPANEL BUTTON_PRESSED");
        }

        /// <summary>
        /// Request the instrument tries to pump the sample inlet
        /// and take the sample into the instrument if sucessful
        /// </summary>
        public void SampleIn()
        {
            connection.SendMessage("SET PUMPINGSTATE LOCK_PUMPDOWN");
        }
    }

    /// <summary>
    /// A class which provides the ProcessMessage method to receive message
    /// updates from the maldiserver
    /// </summary>
    public interface MessageInterface
    {
        void ProcessMessage(String Message);
    }

    /// <summary>
    /// An implementation of a MessageInterface which updates a
    /// windows text boxes.  This implementation uses the AsyncTextDisplay
    /// to hide the windows thread marshalling complexities
    /// </summary>
    public class WindowsFormMessageInterface : MessageInterface
    {
        AsyncTextDisplay TextDisplay;
        AsyncTextDisplay XMotorPositionDisplay;
        AsyncTextDisplay YMotorPositionDisplay;
        AsyncTextDisplay LockStatusDisplay;
        AsyncTextDisplay PumpingStateDisplay;

        /// <summary>
        /// The constructor connects the dialog components 
        /// to our internal asynctextdisplay objects.  This dialog must 
        /// be loaded and the components must exist before it is passed to this consturctor
        /// </summary>
        /// <param name="tellme">The windows dialog object which contains the ui elements
        /// </param>
        public WindowsFormMessageInterface(MainWindow tellme)
        {
            TextDisplay = new AsyncTextDisplay(tellme.LastMessageTextBox());
            XMotorPositionDisplay = new AsyncTextDisplay(tellme.XMotorPositionDisplay());
            YMotorPositionDisplay = new AsyncTextDisplay(tellme.YMotorPositionDisplay());
            LockStatusDisplay = new  AsyncTextDisplay(tellme.LockStatusDisplay());
            PumpingStateDisplay = new AsyncTextDisplay(tellme.PumpingStateDisplay());
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
    /// A class to deal with the marshalling to gui updates from non gui thread
    /// updates.  It just hides the windows complexity
    /// </summary>
    class AsyncTextDisplay
    {
        private TextBox TextElement;

        public AsyncTextDisplay(TextBox thisText)
        {
            TextElement = thisText;
        }
        private delegate void InvokeDelegate(string message);

        public void SetText(string val)
        {
            if (TextElement.InvokeRequired)
            {
                TextElement.BeginInvoke(new InvokeDelegate(SetText), val);
            }
            else
            {
                TextElement.Text = val;
            }
        }
    }

    /// <summary>
    /// An example application which displays the maldi messages in a single line text
    /// field, the lock state, pumping state and stage position
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainWindow mymainwindow = new MainWindow();

            //Create a maldi reader to receive messages from the Maldi instrument
            MaldiReader reader = new MaldiReader("elimaldidev");

            //Create a maldi reader to send control messages to the Maldi instrument
            MaldiController controller = new MaldiController("elimaldidev");

            // install the controller into the main window to enable ui object events to 
            // send messages to the instrument
            mymainwindow.setMaldiControlInterface(controller);

            //install the messaging interface into the reader, the reader will use this 
            // interface to provide the ui with updates
            //
            // in this instance we create an instance of a WindowsFormMessageInterface which
            // understands how to update the ui components of the application main window
            reader.SetMessageInterface(new WindowsFormMessageInterface(mymainwindow));

            // run the application
            Application.Run(mymainwindow);
        }
    }
}
