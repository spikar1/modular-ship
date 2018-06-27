using System.Collections.Generic;
using UnityEngine;

public class Carrier : MonoBehaviour, IToggelableInputReceiver
{
    public int InputOrder => InputReceiverOrder.AttachmentCarrier;
    public bool ReceiveInput { get; set; }

    private ICarryable carriedThing;
    private Sitter sitter;
    private Interactor interactor;

    private readonly List<ICarryable> carryBuffer = new List<ICarryable>();

    private void Awake()
    {
        ReceiveInput = true;
        sitter = GetComponent<Sitter>();
        interactor = GetComponent<Interactor>();
    }
    
    public void OnUpdate(Inputs inputs)
    {
        if (!inputs.interactDown && !inputs.sitDown) 
            return;

        if (carriedThing != null)
        {
            if (inputs.interactDown)
            {
                if (carriedThing.TryPutDown())
                {
                    carriedThing = null;
                    sitter.ReceiveInput = true;
                    interactor.ReceiveInput = true;
                }
            }
            else if (inputs.sitHeld)
            {
                carriedThing.Use();
            }
        }
        else
        {
            Physics2DHelper.GetAllNear(transform.position, .5f, -1, carryBuffer);
            foreach (var carryable in carryBuffer)
            {
                if (carryable.TryPickUp(this))
                {
                    carriedThing = carryable;
                    sitter.ReceiveInput = false;
                    interactor.ReceiveInput = false;
                    break;
                }
            }
        }
    }
}