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
        if (inputs.interactDown)
        {
            Physics2DHelper.GetAllNear(transform.position, 1f, -1, closeInteractables);
            foreach (var interactable in closeInteractables)
            {
                interactable.OnInteract(); //@TODO, figure out how to redesign this. IInteractable directly, probably
            }
        }
    }
}