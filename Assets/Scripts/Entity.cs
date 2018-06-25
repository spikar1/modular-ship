using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//[RequireComponent(typeof(Controller))]
public class Entity : NetworkBehaviour
{
    public enum State { normal, seated, dead};
    public State state = State.normal;

    #region Stats
    public float maxhp, hp;
    #endregion

    public float maxSpeed = 10;
    public float acceleration = 10;
    public Seat seat;
    public LayerMask interactableMask;
    public LayerMask collidableMask;
    public Transform directionParent;

    [HideInInspector]
    public Vector2 dir, moveInput;
    [HideInInspector]
    public bool leftBumper, rightBumper;
    [HideInInspector]
    public Controller controller;

    public virtual void Start()
    {
        controller = GetComponent<Controller>();
    }

    virtual public void SeatedStateUpdate()
    {
        if (!seat)
            Debug.LogError(name + "is trying to act seated when there is no seat registered");

        seat.inputs.axis = moveInput;
        float _strafe = 0;
        _strafe = leftBumper ? _strafe - 1 : _strafe;
        _strafe = rightBumper ? _strafe + 1 : _strafe;
        seat.inputs.strafe = _strafe;
        seat.inputs.action1Down = Input.GetButtonDown("Fire1");
        seat.inputs.action1Up = Input.GetButtonUp("Fire1");
        seat.inputs.action1 = Input.GetButton("Fire1");
    }
    public virtual void NormalStateUpdate() {
    }

    public void AttemptSeat()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, 2, interactableMask);
        if (col)
        {
            seat = col.GetComponent<Seat>();
        }

        if (!seat)
            return;
        seat.entity = this;
        state = State.seated;

        //controller.rb.isKinematic = true;
        //controller.col.enabled = false;
        transform.position = seat.seatPoint;
        //transform.parent = seat.transform;
    }

    [Command]
    public virtual void CmdInteract()
    {

        Debug.Log(name + " triedInteracting");
        Collider2D col = Physics2D.OverlapCircle(transform.position, .4f, interactableMask);

        if (!col)
            return;

        if (!col.GetComponent<Interactable>())
            throw new System.Exception("Only GameObjects with an Interactable() class can have the Interactable mask");

        Interactable _inter = col.GetComponent<Interactable>();
        //print(_inter.transform.name);
        _inter.OnInteract(this);
        
    }
    public void SetDirectionalParent()
    {
        
    }
}
