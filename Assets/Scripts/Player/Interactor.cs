using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour, IToggelableInputReceiver
{
    public int InputOrder => InputReceiverOrder.Interactor;
    public bool ReceiveInput { get; set; }

    private void Start()
    {
        ReceiveInput = true;
    }

    private readonly List<IInteractable> closeInteractables = new List<IInteractable>();
    public void OnUpdate(Inputs inputs)
    {
        if (!inputs.interactDown && !inputs.interactHeld)
            return;

        Physics2DHelper.GetAllNear(transform.position, 1f, -1, closeInteractables);

        foreach (var interactable in closeInteractables)
        {
            if (inputs.interactDown)
                interactable.OnInteractDown();
            if (inputs.interactHeld)
                interactable.OnInteractHeld();
        }
    }
}