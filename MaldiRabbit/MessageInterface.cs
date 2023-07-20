using System;

namespace MaldiRabbit
{
    /// <summary>
    /// A class which provides the ProcessMessage method to receive message
    /// updates from the maldiserver
    /// </summary>
    public interface MessageInterface
    {
        void ProcessMessage(String Message);
    }
}
