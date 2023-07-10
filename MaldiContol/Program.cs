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
        private BlockingCollection<RabbitMQMessageEventArgs> messages = new BlockingCollection<RabbitMQMessageEventArgs>();
        private RabbitMQConnection connection;
        /// <summary>
        /// This receives the messages from the rabbitmq client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="messageargument"></param>
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
        public MaldiReader()
        {

            RabbitMQConnectionDetails connectionParameters = new RabbitMQConnectionDetails();
            connectionParameters.Host = "elimaldidev";
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
        public MaldiController()
        {

            RabbitMQConnectionDetails connectionParameters = new RabbitMQConnectionDetails();
            connectionParameters.Host = "elimaldidev";
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
    /// windows text box.  This implementation uses the AsyncTextDisplay
    /// to hide the windows thread marshalling complexities
    /// </summary>
    public class WindowsFormMessageInterface : MessageInterface
    {
        AsyncTextDisplay TextDisplay;

        public WindowsFormMessageInterface(MainWindow tellme)
        {
            TextDisplay = new AsyncTextDisplay(tellme.LastMessageTextBox());
        }

        public void ProcessMessage(String Message)
        {
            TextDisplay.SetText(Message);
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
    /// field
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
            MaldiReader reader = new MaldiReader();
            MaldiController controller = new MaldiController();
            mymainwindow.setMaldiControlInterface(controller);
            reader.SetMessageInterface(new WindowsFormMessageInterface(mymainwindow));
            
            Application.Run(mymainwindow);
        }
    }
}
