using System.Collections.Generic;
using UnityEngine;

public class Carrier : MonoBehaviour, IPlayerInputReceiver
{
    public int InputOrder => InputReceiverOrder.AttachmentCarrier;
    public bool ReceiveInput { get; set; }

    private ICarryable carriedThing;

    private readonly List<ICarryable> carryBuffer = new List<ICarryable>();

    private void Awake()
    {
        ReceiveInput = true;
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
                    break;
                }
            }
        }
    }
}