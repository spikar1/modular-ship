using UnityEngine;
using System.Collections;

public class Seat : Interactable
{
    //public Vector2 directionalInput;
    public Entity entity;
    public Transform ejectTransform;
    public Inputs inputs;

    public Vector3 seatPoint, ejectPoint;

    bool entityFirstFrame = true;

    public GameObject[] behaviours;

    public void Start()
    {
        seatPoint = transform.position;
        ejectPoint = transform.position + Vector3.up;
        if (ejectTransform)
            ejectPoint = ejectTransform.position;
    }

    public virtual void Update()
    {
        seatPoint = transform.position;
        ejectPoint = ejectTransform.position;
        if (entity)
        {
            if (entityFirstFrame)
            {
                OnSeated(entity);
                entityFirstFrame = false;
            }
            WhileSeated(entity);
        }
        else
            entityFirstFrame = true;
    }

    public virtual void OnEject(Entity _entity)
    {
        //print(entity.name + " got up from " + name);
        for (int i = 0; i < behaviours.Length; i++) {
            IInputReciever behaviour = behaviours[i].GetComponent<IInputReciever>();
            if (behaviour == null) {
                Debug.LogError("GameObject should contain IBehaviour!");
            }
            behaviour.OnEjected();
        }
    }

    public virtual void OnSeated(Entity _entity)
    {
        //print(entity.name + " sat down on " + name);
        for (int i = 0; i < behaviours.Length; i++) {
            IInputReciever behaviour = behaviours[i].GetComponent<IInputReciever>();
            if (behaviour == null) {
                Debug.LogError("GameObject should contain IBehaviour!");
            }
            behaviour.OnSeated();
        }
    }

    public virtual void WhileSeated(Entity _entity)
    {
        for (int i = 0; i < behaviours.Length; i++) {
            IInputReciever behaviour = behaviours[i].GetComponent<IInputReciever>();
            if (behaviour == null) {
                Debug.LogError("GameObject should contain IBehaviour!");
            }
            behaviour.IsInteracting(
                inputs.action1Down,
                inputs.action1Up,
                inputs.axis);
        }
    }

    public override void OnInteract(Entity _entity)
    {
        base.OnInteract(_entity);
        
        switch (_entity.state)
        {
            
            case Entity.State.normal:
                entity = _entity;
                entity.seat = this;
                entity.transform.position = transform.position;
                entity.transform.parent = transform;
                entity.state = Entity.State.seated;
                break;
            case Entity.State.seated:
                OnEject(_entity);
                entity = null;
                _entity.seat = null;
                _entity.transform.position = ejectPoint;
                _entity.transform.parent = _entity.directionParent;
                _entity.state = Entity.State.normal;
                break;
            case Entity.State.dead:
                break;
            default:
                break;
        }
    }

    public void ResetInputs()
    {
        inputs.axis = Vector2.zero;
        inputs.strafe = 0;
        inputs.action1Down = false;
        inputs.action1Up = false;
        inputs.action1 = false;
    }

    public struct Inputs
    {
        public Vector2 axis;
        public float strafe;
        public bool action1Down;
        public bool action1Up;
        public bool action1;
    }
}
