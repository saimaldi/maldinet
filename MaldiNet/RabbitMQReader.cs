using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace MaldiNet
{
    public class RabbitMQConnectionDetails
    {
        private string virtualHost;
        private string host;
        private string username;
        private string password;
        private string exchangeName;
        private int port;

        private void NotifyChanges()
        {
            //OnPropertyChanged();
        }

        public RabbitMQConnectionDetails()
        {
            VirtualHost = "lt2";
            Host = "localhost";
            Username = "SAI";
            Password = "LT2";
            ExchangeName = "LaserToF";
            Port = 5672;
        }

        public string VirtualHost
        {
            get
            {
                return virtualHost;
            }
            set
            {
                if (value != virtualHost)
                {
                    virtualHost = value;
                    NotifyChanges();
                }
            }
        }

        public string Host
        {
            get
            {
                return host;
            }
            set
            {
                if (value != host)
                {
                    host = value;
                    NotifyChanges();
                }
            }
        }

        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                if (value != username)
                {
                    username = value;
                    NotifyChanges();
                }
            }
        }

        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                if (value != password)
                {
                    password = value;
                    NotifyChanges();
                }
            }
        }

        public string ExchangeName
        {
            get
            {
                return exchangeName;
            }
            set
            {
                if (value != exchangeName)
                {
                    exchangeName = value;
                    NotifyChanges();
                }
            }
        }

        public int Port
        {
            get
            {
                return port;
            }
            set
            {
                if (value != port)
                {
                    port = value;
                    NotifyChanges();
                }
            }
        }
    }



    public class RabbitMQMessageEventArgs : EventArgs
    {
        public RabbitMQMessageEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }

    public class RabbitMQConnection
    {
        RabbitMQConnectionDetails serverDetails;
        IConnection connection;
        IModel model;
        string queueName = "";


        public bool SendMessage(string ThisMessage)
        {
            if (!model.IsOpen)
                return false;

            byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(ThisMessage);
            model.BasicPublish(serverDetails.ExchangeName, "", null, messageBodyBytes);
            return true;
        }

        public void Connect(RabbitMQConnectionDetails serverDetails, bool consume)
        {
            this.serverDetails = serverDetails;

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
                bool failed = true;
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
        }

        public bool isConnected()
        {
            return connection.IsOpen;
        }

        public event EventHandler<RabbitMQMessageEventArgs> NewMessage;

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            model.BasicAck(e.DeliveryTag, false);

            NewMessage?.Invoke(this, new RabbitMQMessageEventArgs(Encoding.UTF8.GetString(e.Body.Span)));
        }

        public void Disconnect()
        {
            model?.Close();
            connection?.Close();
        }

        public void Publish(string message)
        {
            model.BasicPublish(serverDetails.ExchangeName, "", null, Encoding.UTF8.GetBytes(message));
        }

        public void SubscribeToNewMessages(EventHandler<RabbitMQMessageEventArgs> CallMe)
        {
            NewMessage += CallMe;
        }

        public void UnSubscribeFromNewMessages(EventHandler<RabbitMQMessageEventArgs> CallMe)
        {
            NewMessage -= CallMe;
        }
    }

    class RabbitMQReader
    {

    }
}
