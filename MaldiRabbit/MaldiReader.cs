using MaldiContol;
using MaldiRabbit;

namespace MaldiRabbit
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
        private readonly RabbitMQConnection connection;

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

        public bool isConnected()
        {
            return connection.isConnected();
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
}
