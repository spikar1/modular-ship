using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class Seat : MonoBehaviour
{
    [SerializeField] private Transform ejectTransform;
    [SerializeField] private GameObject[] behaviours;
    private List<IInputReceiver> inputReceivers; 

    [NonSerialized] public Sitter sitter;
    public Vector3 ejectPoint { get; private set; }

    private void Awake()
    {
        if (ejectTransform)
            ejectPoint = ejectTransform.position;
        else
            ejectPoint = transform.position + Vector3.up;

        inputReceivers = new List<IInputReceiver>();
        foreach (var b in behaviours)
        {
            var ir = b.GetComponent<IInputReceiver>();
            if (ir != null) 
                inputReceivers.Add(ir);
            else
                Debug.LogWarning(
                    $"Object {b.name} is attached as an inputreceiver to seat {name}, but it's not " +
                    $"got any inputReceivers attached!", b);
        }
    }

    public void OnUpdate(Inputs inputs)
    {
        foreach (var receiver in inputReceivers)
        {
            receiver.OnUpdate(inputs);
        }
    }
}