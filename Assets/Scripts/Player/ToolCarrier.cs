using System.Collections.Generic;
using UnityEngine;

public class ToolCarrier : MonoBehaviour, IPlayerInputReceiver {
    
    public int InputOrder => InputReceiverOrder.ItemCarrier;
    public bool ReceiveInput { get; set; }

    private readonly List<ITool> toolBuffer = new List<ITool>();
    private ITool carriedThing;

    private void Awake() {
        ReceiveInput = true;
    }

    public void OnUpdate(Inputs inputs) {
        if (carriedThing != null) {
            if (inputs.interactDown) {
                DropCarriedThing();
            }
            else if (inputs.sitHeld) {
                carriedThing.Use();
            }
        }
        else {
            if (inputs.interactDown) {
                var closestCarriable = Physics2DHelper.GetClosest(transform.position, 1f, -1, toolBuffer, CanBeCarried);
                if (closestCarriable != null) {
                    carriedThing = closestCarriable;
                    carriedThing.OnPickUp(this);
                }
            }
        }
    }

    private static bool CanBeCarried(ITool tool) => !tool.IsCarried;

    private void DropCarriedThing() {
        carriedThing.OnDropped(this);
        carriedThing = null;
    }
}