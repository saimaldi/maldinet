using MaldiContol;
using System.Threading;
using MaldiRabbit;

namespace MaldiNet
{

    class Program
    {
        static void Main(string[] args)
        {
            MaldiReader maldiReader = new MaldiReader( "rabbitmq-server");
            ConsoleMaldiMessageInterface messagePrinter = new ConsoleMaldiMessageInterface();
            maldiReader.SetMessageInterface(messagePrinter);

            while (true)
                Thread.Sleep(10000);
                
        }

    }
}

