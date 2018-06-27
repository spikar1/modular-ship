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
        foreach (var behaviour in behaviours)
        {
            var inputReceiver = behaviour.GetComponent<IInputReceiver>();
            if (inputReceiver != null) 
                inputReceivers.Add(inputReceiver);
            else
                Debug.LogWarning(
                    $"Object {behaviour.name} is attached as an inputreceiver to seat {name}, but it's not " +
                    $"got any inputReceivers attached!", behaviour);
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