namespace LGW.MessageDistributor.MessageBus.Core.Consts
{
    public static class MessageBusConsts
    {
        

        public struct Events
        {
            public const string LoadingInstructions = "LoadingInstructions";
            public const string LoadingInstructionCreated = "LoadingInstructionCreated";
            public const string LoadingInstructionCreatedError = $"{LoadingInstructionCreated}_error";
            public const string LoadingInstructionApplied = "LoadingInstructionApplied";
            public const string LoadingInstructionAppliedError = $"{LoadingInstructionApplied}_error";
            public const string LoadingInstructionCancelled = "LoadingInstructionCancelled";
            public const string LoadingInstructionCancelledError = $"{LoadingInstructionCancelled}_error";
            public const string LoadingInstructionThresholdExceeded = "LoadingInstructionThresholdExceeded";
            public const string LoadingInstructionAppliedThresholdExceeded = "LoadingInstructionAppliedThresholdExceeded";
        }

        public struct CachePrefix
        {
            public const string CreatedLoadingInstructions = "LoadingInstructions.Created";
            public const string AppliedLoadingInstructions = "LoadingInstructions.Applied";
            public const string CancelledLoadingInstructions = "LoadingInstructions.Cancelled";
        }


        public struct RoutingKeys
        {
            public const string LoadingInstructionCreatedExceeded = "loadingInstructionCreatedExceeded";
            public const string LoadingInstructionAppliedExceeded = "loadingInstructionAppliedExceeded";
            public const string LoadingInstructionCancelledExceeded = "loadingInstructionCancelledExceeded";
        }

        public struct CacheHashTableName
        {
            public const string CreatedLoadingInstructionQueueState = "LoadingInstruction.CreatedQueueState";
            public const string AppliedLoadingInstructionQueueState = "LoadingInstruction.AppliedQueueState";
            public const string CancelledLoadingInstructionQueueState = "LoadingInstruction.CancelledQueueState";
        }

    }
}
