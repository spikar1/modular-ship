public interface ITool
{
    bool IsCarried { get; }
    void Use();
    void OnPickUp(ToolCarrier carrier);
    void OnDropped(ToolCarrier carrier);
}