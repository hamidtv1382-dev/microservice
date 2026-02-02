namespace Messaging_Service.src._01_Domain.Core.Enums
{
    public enum MessageStatus
    {
        Pending = 1,
        PendingApproval = 2,
        Approved = 3,
        Sent = 4,
        Failed = 5,
        Rejected = 6
    }
}
