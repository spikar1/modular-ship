using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Controller))]
public class Entity : MonoBehaviour
{
    public enum State { normal, seated};
    public State state = State.normal;

    #region Stats
    public float maxhp, hp;
    #endregion

    public float maxSpeed = 10;
    public Seat seat;
    public LayerMask interactableMask;
    public LayerMask collidableMask;
    public Transform directionParent;

    private Attachment carriedAttachment;

    [HideInInspector]
    public Vector2 dir, moveInput;
    [HideInInspector]
    public bool leftBumper, rightBumper;
    [HideInInspector]
    public bool interactButton, interactButtonDown, interactButtonUp;
    [HideInInspector]
    public bool action, actionDown, actionUp;
    [HideInInspector]
    public Controller controller;

    public virtual void Start()
    {
        controller = GetComponent<Controller>();
    }
    
    public virtual void Update()
    {
        switch (state)
        {
            case State.normal:
                NormalStateUpdate();
                break;
            case State.seated:
                SeatedUpdate();
                break;
        }
    }

    protected virtual void SeatedUpdate()
    {
        if (!seat)
            Debug.LogError(name + "is trying to act seated when there is no seat registered");

        seat.inputs.axis = moveInput;
        float _strafe = 0;
        _strafe = leftBumper ? _strafe - 1 : _strafe;
        _strafe = rightBumper ? _strafe + 1 : _strafe;
        seat.inputs.strafe = _strafe;
        seat.inputs.action1Down = actionDown;
        seat.inputs.action1Up = actionUp;
        seat.inputs.action1 = action;

        if (interactButtonDown)
            Interact();

        if (!directionParent)
            directionParent = seat.transform;
    }

    protected virtual void NormalStateUpdate() {
        if (interactButtonDown)
            Interact();

        if (directionParent)
            dir = directionParent.right * moveInput.x + directionParent.up * moveInput.y;
        else
            dir = moveInput;

        Debug.DrawRay((Vector2)transform.position, dir * 3, Color.blue);
        Vector2 velocity = new Vector3(dir.x, dir.y) * maxSpeed;
        Debug.DrawRay((Vector2)transform.position, velocity * 2, Color.red);
        controller.Move(velocity * Time.deltaTime, collidableMask);
    }

    protected void Interact()
    {
        if (carriedAttachment != null)
            CarryingInteract();
        else
            NormalInteract();
    }

    private Collider2D[] overlapCircleBuffer = new Collider2D[10];
    private List<Wall> hitWalls = new List<Wall>();
    private void CarryingInteract()
    {
        int numHits = Physics2D.OverlapCircleNonAlloc(transform.position, 2f, overlapCircleBuffer, -1);
        hitWalls.Clear();
        for (int i = 0; i < numHits; i++)
        {
            var wall = overlapCircleBuffer[i].GetComponent<Wall>();
            if (wall != null && wall.attachedThing == null)
                hitWalls.Add(wall);
        }
        
        if (hitWalls.Count > 0)
        {
            if (carriedAttachment.TryAttachToNearest(hitWalls))
                carriedAttachment = null;
        }
    }

    private void NormalInteract()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, .4f, interactableMask);

        if (!col)
            return;

        if (!col.GetComponent<Interactable>())
            throw new System.Exception("Only GameObjects with an Interactable() class can have the Interactable mask");

        Interactable _inter = col.GetComponent<Interactable>();
        _inter.OnInteract(this);
    }

    public void StartCarrying(Attachment attachment)
    {
        attachment.transform.parent = transform;
        attachment.transform.localPosition = Vector2.zero;
        carriedAttachment = attachment;
    }
}
