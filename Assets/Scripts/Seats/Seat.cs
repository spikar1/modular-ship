using UnityEngine;

public sealed class Seat : Interactable
{
    public Entity entity;
    public Transform ejectTransform;
    public Inputs inputs;

    public Vector3 ejectPoint;

    private bool entityFirstFrame = true; 

    public GameObject[] behaviours;
    
    public void Start()
    {
        ejectPoint = transform.position + Vector3.up;
        if (ejectTransform)
            ejectPoint = ejectTransform.position;
    }

    public void Update()
    {
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

    private void OnEject(Entity _entity)
    {
        for (int i = 0; i < behaviours.Length; i++) {
            IInputReciever behaviour = behaviours[i].GetComponent<IInputReciever>();
            if (behaviour == null) {
                Debug.LogError("GameObject should contain IBehaviour!");
            }
        }
    }

    private void OnSeated(Entity _entity)
    {
        for (int i = 0; i < behaviours.Length; i++) {
            IInputReciever behaviour = behaviours[i].GetComponent<IInputReciever>();
            if (behaviour == null) {
                Debug.LogError("GameObject should contain IBehaviour!");
            }
        }
    }

    private void WhileSeated(Entity _entity)
    {
        for (int i = 0; i < behaviours.Length; i++) {
            IInputReciever behaviour = behaviours[i].GetComponent<IInputReciever>();
            if (behaviour == null) {
                Debug.LogError("GameObject should contain IBehaviour!");
            }
            else
            {
                behaviour.ReceiveInput(inputs);
            }
        }
    }

    public override void OnInteract(Entity entity)
    {
        base.OnInteract(entity);
        
        switch (entity.state)
        {
            
            case Entity.State.normal:
                this.entity = entity;
                this.entity.seat = this;
                this.entity.transform.position = transform.position;
                this.entity.transform.parent = transform;
                this.entity.state = Entity.State.seated;
                break;
            case Entity.State.seated:
                OnEject(entity);
                this.entity = null;
                entity.seat = null;
                entity.transform.position = ejectPoint;
                entity.transform.parent = entity.directionParent;
                entity.state = Entity.State.normal;
                break;
        }
    }
}