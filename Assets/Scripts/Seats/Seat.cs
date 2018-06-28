using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class Seat : MonoBehaviour, ISeatInputReceiver {
    [SerializeField] private Transform ejectTransform;
    [SerializeField] private GameObject[] behaviours;
    private List<ISeatInputReceiver> seatInputReceivers;
    private List<IInputReceiver> inputReceivers; //all objects in seatInputReceivers are also here

    public Vector3 ejectPoint { get; private set; }
    public Sitter sitter { get; private set; }

    private void Awake() {
        if (ejectTransform)
            ejectPoint = ejectTransform.position;
        else
            ejectPoint = transform.position + Vector3.up;

        inputReceivers = new List<IInputReceiver>();
        seatInputReceivers = new List<ISeatInputReceiver>();
        foreach (var behaviour in behaviours) {
            var receiversOnObject = behaviour.GetComponents<IInputReceiver>();
            foreach (var inputReceiver in receiversOnObject) {
                if (inputReceiver != null) {
                    inputReceivers.Add(inputReceiver);
                    var asSeatReceiver = inputReceiver as ISeatInputReceiver;
                    if (asSeatReceiver != null)
                        seatInputReceivers.Add(asSeatReceiver);
                }
            }

            if (receiversOnObject.Length == 0) {
                Debug.LogWarning(
                    $"Object {behaviour.name} is attached as an inputreceiver to seat {name}, but it's not " +
                    $"got any inputReceivers attached!", behaviour);
            }
        }
    }

    public void OnUpdate(Inputs inputs) {
        foreach (var receiver in inputReceivers) {
            receiver.OnUpdate(inputs);
        }
    }

    public void OnSeated(Sitter sitter) {
        this.sitter = sitter;
        foreach (var receiver in seatInputReceivers) {
            receiver.OnSeated(sitter);

        }
    }

    public void SitterGotUp() {
        sitter = null;
    }
}