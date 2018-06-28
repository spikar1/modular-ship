using System.Collections.Generic;
using UnityEngine;

public class AttachmentCarrier : MonoBehaviour, IInputReceiver
{
    private Attachment carriedThing;

    private readonly List<Attachment> carryBuffer = new List<Attachment>();

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