namespace DDDS.Test.WebAPI.Constants
{
    public static class RabbitMQConstants
    {
        public const string Uri = "amqp://localhost";
        public const string Username = "guest";
        public const string Password = "guest";

        public struct Events
        {
            public const string LoadingInstructionCreated = "LoadingInstructionCreated";
            public const string LoadingInstructionThresholdExceeded = "LoadingInstructionThresholdExceeded";
        }

    }
}
