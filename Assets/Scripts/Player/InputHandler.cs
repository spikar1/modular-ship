using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private IInputReceiver[] inputReceivers;
    public int player;

    private void Start()
    {
        inputReceivers = GetComponents<IInputReceiver>();
        Array.Sort(inputReceivers, (ir1, ir2) => ir1.InputOrder.CompareTo(ir2.InputOrder));
    }

    private void Update()
    {
        var inputs = ReadInputs();
        foreach (var inputReceiver in inputReceivers)
        {
            if(inputReceiver.ReceiveInput)
                inputReceiver.OnUpdate(inputs);
        }
    }

    private Inputs ReadInputs()
    {
        if (player == 0)
        {
            return new Inputs
            {
                axis =
                {
                    x = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0),
                    y = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0)
                },
                leftBumper = Input.GetKey(KeyCode.Q),
                rightBumper = Input.GetKey(KeyCode.E),

                interactHeld = Input.GetKey(KeyCode.Space),
                interactDown = Input.GetKeyDown(KeyCode.Space),
                interactUp = Input.GetKeyUp(KeyCode.Space),

                sitHeld = Input.GetKey(KeyCode.LeftShift),
                sitDown = Input.GetKeyDown(KeyCode.LeftShift),
                sitUp = Input.GetKeyUp(KeyCode.LeftShift)
            };
        }
        else
        {
            return new Inputs();
        }
    }
}