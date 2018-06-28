public interface ICarryable
{
    bool TryPickUp(AttachmentCarrier attachmentCarrier);
    bool TryPutDown();

}