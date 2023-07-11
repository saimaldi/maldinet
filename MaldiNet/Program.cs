using MaldiContol;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace MaldiNet
{

    class Program
    {
        static void Main(string[] args)
        {
            MaldiReader maldiReader = new MaldiReader( "elimaldidev");
            ConsoleMaldiMessageInterface messagePrinter = new ConsoleMaldiMessageInterface();
            maldiReader.SetMessageInterface(messagePrinter);

            while (true)
                Thread.Sleep(10000);
                
        }

    }
}

