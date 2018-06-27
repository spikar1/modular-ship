using UnityEngine;

public class Mover : MonoBehaviour, IToggelableInputReceiver
{
    private Controller controller;
    [SerializeField]
    private LayerMask collidableMask;
    [SerializeField]
    private float speed;
    public int InputOrder => InputReceiverOrder.Mover;
    public bool ReceiveInput { get; set; }
    
    private void Awake()
    {
        controller = GetComponent<Controller>();
        ReceiveInput = true;
    }

    public void OnUpdate(Inputs inputs)
    {
        controller.Move(inputs.axis.normalized * speed * Time.deltaTime, collidableMask);
    }
}