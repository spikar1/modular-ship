public interface IInputReceiver {
    void OnUpdate(Inputs inputs);
}

public interface ISeatInputReceiver : IInputReceiver {
    void OnSeated(Sitter sitter);
}

public interface IPlayerInputReceiver : IInputReceiver {
    int InputOrder { get; }
    bool ReceiveInput { get; }
}

public static class InputReceiverOrder {
    public const int Sitter = 0;
    public const int AttachmentCarrier = 1;
    public const int Interactor = 2;
    public const int Mover = 3;
    public const int Camera = 4;
}