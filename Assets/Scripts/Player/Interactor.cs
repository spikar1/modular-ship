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

    private readonly List<IInteractable> interactableBuffer = new List<IInteractable>();
    public void OnUpdate(Inputs inputs)
    {
        if (!inputs.interactDown && !inputs.interactHeld)
            return;

        Physics2DHelper.GetAllNear(transform.position, 1f, -1, interactableBuffer);
        interactableBuffer.Sort(SortInteractables);
        if (interactableBuffer.Count == 0)
            return;

        var closest = interactableBuffer[0];
        if (inputs.interactDown)
            closest.OnInteractDown();
        if (inputs.interactHeld)
            closest.OnInteractHeld();
    }

    private int SortInteractables(IInteractable x, IInteractable y)
    {
        var myPos = transform.position;
        var xDist = Vector3.SqrMagnitude(((Component) x).transform.position - myPos);
        var yDist = Vector3.SqrMagnitude(((Component) y).transform.position - myPos);

        return xDist.CompareTo(yDist);
    }
}