using MaldiContol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaldiNet
{
    class ConsoleMaldiMessageInterface : MessageInterface
    {
        public void ProcessMessage(string Message)
        {
            Console.WriteLine(Message);
        }
    }
}
