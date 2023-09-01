using MaldiRabbit;

namespace MaldiContol
{
    /// <summary>
    /// Provides and interface for applications to set commands to the MALDI Instrument
    /// </summary>
    public class MaldiController
    {
        private readonly RabbitMQConnection connection;
        private readonly RabbitMQConnectionDetails connectionParameters;

        public void Connect()
        {
            if (IsConnected()) return;

            connection.Connect(connectionParameters, false);
        }

        public MaldiController(string hostname = "10.1.234.1")
        {

            connectionParameters = new RabbitMQConnectionDetails();
            connectionParameters.Host = hostname;
            connectionParameters.SetControlExchange();
            connection = new RabbitMQConnection();
            Connect();
            
        }

        /// <summary>
        /// Request the instrument moves the sample to the sample
        /// inlet allows the user to change the sample plate
        /// </summary>
        public void SampleOut()
        {
            connection.SendMessage("ADVISE FRONTPANEL BUTTON_PRESSED");
        }

        /// <summary>
        /// Request the instrument tries to pump the sample inlet
        /// and take the sample into the instrument if sucessful
        /// </summary>
        public void SampleIn()
        {
            connection.SendMessage("SET PUMPINGSTATE LOCK_PUMPDOWN");
        }

        public bool IsConnected()
        {
            if (connection == null) return false;
            return connection.isConnected();
        }
    }
}
