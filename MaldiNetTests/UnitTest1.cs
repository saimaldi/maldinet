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
            connectionParameters.Host = "192.168.57.237";
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
            connectionParameters.Host = "192.168.57.237";
            connectionParameters.Username = "SAI";
            connectionParameters.Password = "LT2";
            connectionParameters.ExchangeName = "LaserToF";

            RabbitMQConnection connection = new RabbitMQConnection();
            connection.Connect(connectionParameters, true);
            connection.SubscribeToNewMessages((object sender, RabbitMQMessageEventArgs x) => { messages.Add(x); }) ;
            Thread.Sleep(10000);
            Assert.IsTrue(connection.isConnected());
            Assert.IsTrue(messages.Count > 0);
        }
    }
}