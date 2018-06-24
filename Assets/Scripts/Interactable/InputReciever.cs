using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInputReciever {
    void ReceiveInput(bool action1Down, bool action1Up, Vector2 axis);
}
