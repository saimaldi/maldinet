using MaldiNet;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace MaldiNetTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {

            Assert.Pass();
        }

        [Test]
        public void CanCreateConnectionToMaldiNotificationQueue()
        {
            RabbitMQConnectionDetails connectionParameters = new RabbitMQConnectionDetails();
            connectionParameters.VirtualHost = "lt2";
            connectionParameters.Host = "elimaldidev";
            connectionParameters.Username = "SAI";
            connectionParameters.Password = "LT2";
            connectionParameters.ExchangeName = "LaserToF";

            RabbitMQConnection connection = new RabbitMQConnection();
            connection.Connect(connectionParameters, true);

            Assert.IsTrue(connection.isConnected());
        }


        [Test]
        public void CanSubscribeToMessages()
        {
            List<RabbitMQMessageEventArgs> messages = new List<RabbitMQMessageEventArgs>();

            RabbitMQConnectionDetails connectionParameters = new RabbitMQConnectionDetails();
            connectionParameters.VirtualHost = "lt2";
            connectionParameters.Host = "elimaldidev";
            connectionParameters.Username = "SAI";
            connectionParameters.Password = "LT2";
            connectionParameters.ExchangeName = "LaserToF";
            connectionParameters.VirtualHost = "lt2";

            RabbitMQConnection connection = new RabbitMQConnection();
           
            connection.Connect(connectionParameters, true);
           
            connection.SubscribeToNewMessages((object sender, RabbitMQMessageEventArgs x) => { messages.Add(x); }) ;
            Thread.Sleep(10000);
            Assert.IsTrue(connection.isConnected());
            Assert.IsTrue(messages.Count > 0);
        }

        [Test]
        public void CanSendConmmandMessages()
        {
            List<RabbitMQMessageEventArgs> messages = new List<RabbitMQMessageEventArgs>();

            RabbitMQConnectionDetails connectionParameters = new RabbitMQConnectionDetails();
            connectionParameters.VirtualHost = "lt2";
            connectionParameters.Host = "elimaldidev";
            connectionParameters.Username = "SAI";
            connectionParameters.Password = "LT2";
            connectionParameters.ExchangeName = "LaserToF";
            connectionParameters.VirtualHost = "lt2";

            

            RabbitMQConnection Receiverconnection = new RabbitMQConnection();

            Receiverconnection.Connect(connectionParameters, true);

            Receiverconnection.SubscribeToNewMessages((object sender, RabbitMQMessageEventArgs x) => { messages.Add(x); });

            
            RabbitMQConnection Senderconnection = new RabbitMQConnection();
            connectionParameters.ExchangeName = "LaserToFCommand";
            Senderconnection.Connect(connectionParameters, false);

            Assert.IsTrue( Senderconnection.SendMessage("SET TIMING GATE DELAY 2000 ns"));

            Assert.IsTrue(Receiverconnection.isConnected());
            Assert.IsTrue(messages.Count > 0);
            Receiverconnection.Disconnect();
            bool message_found = false;
            foreach (var x in messages)
            {
                string update = x.Message.ToUpper();
                if (update.Equals("TIMING GATE DELAY 2000 NS"))
                {
                    message_found = true;
                }
            }
            Assert.IsTrue(message_found);
        }
    }
}