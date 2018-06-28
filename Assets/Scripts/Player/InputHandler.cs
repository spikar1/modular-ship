using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private IPlayerInputReceiver[] inputReceivers;
    public int player;

    private void Start()
    {
        inputReceivers = GetComponents<IPlayerInputReceiver>();
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
                leftBumper  = Input.GetKey(KeyCode.Q),
                rightBumper = Input.GetKey(KeyCode.E),

                interactHeld = Input.GetKey(KeyCode.Space),
                interactDown = Input.GetKeyDown(KeyCode.Space),
                interactUp   = Input.GetKeyUp(KeyCode.Space),

                sitHeld = Input.GetKey(KeyCode.LeftShift),
                sitDown = Input.GetKeyDown(KeyCode.LeftShift),
                sitUp   = Input.GetKeyUp(KeyCode.LeftShift),
                
                cameraRotation = Input.GetMouseButton(2) ? Input.GetAxisRaw("Mouse X") : 0f,
                cameraZoom     = Input.mouseScrollDelta.y,
            };
        }
        if (player == 1)
        {
            return new Inputs
            {
                axis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")),

                leftBumper  = Input.GetButton("LeftBumper"),
                rightBumper = Input.GetButton("RightBumper"),

                interactHeld = Input.GetButton("Fire1"),
                interactDown = Input.GetButtonDown("Fire1"),
                interactUp   = Input.GetButtonUp("Fire1"),

                sitHeld = Input.GetButton("Fire2"),
                sitDown = Input.GetButtonDown("Fire2"),
                sitUp   = Input.GetButtonUp("Fire2"),
                
                cameraRotation = Input.GetAxisRaw("RightStick X") * .4f,
                cameraZoom     = -Input.GetAxisRaw("RightStick Y") * .1f,
            };
        }

        return new Inputs();
    }
}