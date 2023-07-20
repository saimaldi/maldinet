using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace MaldiRabbit
{
    /// <summary>
    /// A collection describing the connection for the rabbitmq server
    /// </summary>
    public class RabbitMQConnectionDetails
    {
        private string virtualHost;
        private string host;
        private string username;
        private string password;
        private string exchangeName;
        private int port;

        /// <summary>
        /// Helper method to set the exchange name for clients wishing to see the informative 
        /// Messages fromt the Maldi Control System
        /// 
        /// This connection should use SubscribeToNewMessages(EventHandler<RabbitMQMessageEventArgs> CallMe)
        /// to gain access to the incoming messages
        /// </summary>
        public void SetReportingExchange()
        {
            ExchangeName = "LaserToF";
        }

        /// <summary>
        /// The control exchange is used by the MaldiControl system to request changes to the system
        /// eg. Open the Lock, set the high voltages
        /// 
        /// Messages are transmitted using the SendMessage
        /// </summary>
        public void SetControlExchange()
        {
            ExchangeName = "LaserToFCommand";
        }


        /// <summary>
        /// Class containing the rabbitmq virtualhost, hostname
        /// username password and exchange name
        /// 
        /// This class is used as the configuration for the connection 
        /// to the rabbitmq server
        /// </summary>
        public RabbitMQConnectionDetails()
        {
            VirtualHost = "lt2";
            Host = "10.1.234.1";
            Username = "SAI";
            Password = "LT2";
            ExchangeName = "LaserToF";
            Port = 5672;
        }

        /// <summary>
        /// Virtual host on the rabbitmqserver
        /// </summary>
        public string VirtualHost
        {
            get { return virtualHost;}
            set{ virtualHost = value; }
        }

        /// <summary>
        /// The hostname or ip address of the rabbitmqserver
        /// </summary>
        public string Host
        {
            get { return host; }
            set { host = value;}
        }

        /// <summary>
        /// Username used to create the connection to the rabbitmqserver
        /// </summary>
        public string Username
        {
            get {return username; }
            set {username = value;}
        }

        /// <summary>
        /// password used to create the connection to the rabbitmqserver
        /// </summary>
        public string Password
        {
            get{ return password;}
            set { password = value;}
        }

        /// <summary>
        /// The string representing the exchange on the rabbitmq server
        /// </summary>
        public string ExchangeName
        {
            get {  return exchangeName;}
            set  { exchangeName = value; }
        }

        public int Port
        {
            get { return port; }
            set { port = value; }
        }
    }



    /// <summary>
    /// Messageing class for use with the c# event
    /// </summary>
    public class RabbitMQMessageEventArgs : EventArgs
    {
        public RabbitMQMessageEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }

    /// <summary>
    /// An instance of a connection to the rabbitmq server
    /// seperate instances should be created for senders and receivers
    /// </summary>
    public class RabbitMQConnection
    {
        RabbitMQConnectionDetails serverDetails;
        IConnection connection;
        IModel model;
        readonly string queueName = "";

        /// <summary>
        /// Sends a message to the rabbitmq server
        /// </summary>
        /// <param name="ThisMessage">The text string containing the message</param>
        /// <returns>transmission status, false if the connection is not open</returns>
        public bool SendMessage(string ThisMessage)
        {
            if (!model.IsOpen)
                return false;

            byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(ThisMessage);
            model.BasicPublish(serverDetails.ExchangeName, "", null, messageBodyBytes);
            return true;
        }

        /// <summary>
        /// Use the connection parameters to connect to the rabbitmq server
        /// 
        /// start reading messages if consume is true
        /// </summary>
        /// <param name="serverDetails"></param>
        /// <param name="consume">Start Reading Messages</param>
        /// <returns>Connection status</returns>
        public bool Connect(RabbitMQConnectionDetails serverDetails, bool consume)
        {
            this.serverDetails = serverDetails;
            bool failed = false;

            ConnectionFactory factory = new ConnectionFactory();

            factory.UserName = serverDetails.Username;
            factory.Password = serverDetails.Password;
            factory.VirtualHost = serverDetails.VirtualHost;
            factory.HostName = serverDetails.Host;
             
            factory.Port = serverDetails.Port;

            try
            {
                connection = factory.CreateConnection();
            }
            catch 
            {
                 return false;
            }
            

            model = connection.CreateModel();
            model.ExchangeDeclare(serverDetails.ExchangeName, ExchangeType.Fanout);
            

            if (consume)
            {
                
                 model.QueueDeclare(queue: "",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
                

                model.QueueBind("", serverDetails.ExchangeName, "",null);

                EventingBasicConsumer consumer = new EventingBasicConsumer(model);
                consumer.Received += Consumer_Received;
 
                var result = model.BasicConsume(queueName, false, consumer);
            
            }
            return failed;
        }

        /// <summary>
        /// Query the current state of the connection
        /// </summary>
        /// <returns>True if connected, false if not</returns>
        public bool isConnected()
        {
            return connection.IsOpen;
        }

        public event EventHandler<RabbitMQMessageEventArgs> NewMessage;

        /// <summary>
        /// Internal method which accepts the incoming rabbitmq messages
        /// from the client library
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">Rabbigmq message details</param>
        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            byte[] bytes = e.Body.ToArray();
            String Message = Encoding.UTF8.GetString(bytes);
            // pass on the message to any subscribed observers
            NewMessage?.Invoke(this, new RabbitMQMessageEventArgs(Message));
            model.BasicAck(e.DeliveryTag, false);
        }

        /// <summary>
        /// Close the connection to the rabbitmq server
        /// </summary>
        public void Disconnect()
        {
            model?.Close();
            connection?.Close();
        }

        /// <summary>
        /// install a delegate methis which will be called whenever a message is received
        /// </summary>
        /// <param name="CallMe">A delegate to receive the message</param>
        public void SubscribeToNewMessages(EventHandler<RabbitMQMessageEventArgs> CallMe)
        {
            NewMessage += CallMe;
        }

        /// <summary>
        /// Remove the delegate which no longer wants to receive the messages
        /// </summary>
        /// <param name="CallMe"></param>
        public void UnSubscribeFromNewMessages(EventHandler<RabbitMQMessageEventArgs> CallMe)
        {
            NewMessage -= CallMe;
        }
    }
}
