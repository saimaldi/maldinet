using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MaldiNet
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            BlockingCollection<RabbitMQMessageEventArgs> messages = new BlockingCollection<RabbitMQMessageEventArgs>();

            RabbitMQConnectionDetails connectionParameters = new RabbitMQConnectionDetails();
            connectionParameters.VirtualHost = "lt2";
            connectionParameters.Host = "elimaldidev";
            connectionParameters.Username = "SAI";
            connectionParameters.Password = "LT2";
            connectionParameters.ExchangeName = "LaserToF";
            connectionParameters.VirtualHost = "lt2";

            RabbitMQConnection connection = new RabbitMQConnection();

            connection.Connect(connectionParameters, true);

            connection.SubscribeToNewMessages((object sender, RabbitMQMessageEventArgs x) => { messages.Add(x); });
            while (true)
                if (messages.Count > 0)
                {
                    var message = messages.Take();                   
                    Console.WriteLine(message.Message);
                }
        }

    }
}

