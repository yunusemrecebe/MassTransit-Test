namespace DDDS.Test.WebAPI.Constants
{
    public static class RabbitMQConstants
    {
        //public const string Uri = "amqp://10.240.1.102:30672";
        //public const string Username = "admin";
        //public const string Password = "Asd12345!";
        public const string Uri = "http://127.0.0.1:5672";
        public const string Host = "127.0.0.1";
        public const int Port = 5672;
        public const string Username = "guest";
        public const string Password = "guest";

        public struct Events
        {
            public const string LoadingInstructionCreated = "LoadingInstructionCreated";
            public const string LoadingInstructionApplied = "LoadingInstructionApplied";
            public const string LoadingInstructionThresholdExceeded = "LoadingInstructionThresholdExceeded";
        }

    }
}
