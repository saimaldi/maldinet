using MaldiRabbit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;


namespace MaldiRabbitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CanCreateConnectionToMaldiNotificationQueue()
        {
            MaldiReader connection = new MaldiReader("elimaldidev");
 
            Assert.IsTrue(connection.isConnected());
        }

        [TestMethod]
        public void CanSubscribeToMessages()
        {
            List<RabbitMQMessageEventArgs> messages = new List<RabbitMQMessageEventArgs>();

            RabbitMQConnectionDetails connectionParameters = new RabbitMQConnectionDetails();
            connectionParameters.Host = "elimaldidev";

            RabbitMQConnection connection = new RabbitMQConnection();

            connection.Connect(connectionParameters, true);

            connection.SubscribeToNewMessages((object sender, RabbitMQMessageEventArgs x) => { messages.Add(x); });
            Thread.Sleep(10000);
            Assert.IsTrue(connection.isConnected());
            Assert.IsTrue(messages.Count > 0);
        }

        [TestMethod]
        public void CanSendConmmandMessages()
        {
            List<RabbitMQMessageEventArgs> messages = new List<RabbitMQMessageEventArgs>();

            RabbitMQConnectionDetails connectionParameters = new RabbitMQConnectionDetails();
            connectionParameters.Host = "elimaldidev";

            RabbitMQConnection Receiverconnection = new RabbitMQConnection();

            Receiverconnection.Connect(connectionParameters, true);

            Receiverconnection.SubscribeToNewMessages((object sender, RabbitMQMessageEventArgs x) => { messages.Add(x); });


            RabbitMQConnection Senderconnection = new RabbitMQConnection();
            connectionParameters.SetControlExchange();
            Senderconnection.Connect(connectionParameters, false);

            Assert.IsTrue(Senderconnection.SendMessage("SET TIMING GATE DELAY 2000 ns"));
            Thread.Sleep(1000);
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
