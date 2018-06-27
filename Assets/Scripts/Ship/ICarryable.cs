public interface ICarryable
{
    void Use();
    bool TryPickUp(Carrier carrier);
    bool TryPutDown();

}