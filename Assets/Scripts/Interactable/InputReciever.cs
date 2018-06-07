using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInputReciever {
    void IsInteracting(bool action1Down, bool action1Up, Vector2 axis);
    void OnSeated();
    void OnEjected();

    /*void Action1Down();
    void Action2Down();
    void Action3Down();

    void Action1();
    void Action2();
    void Action3();

    void InputAxis(Vector2 axis);*/
}

/*public class InputReciever : MonoBehaviour {
}*/
