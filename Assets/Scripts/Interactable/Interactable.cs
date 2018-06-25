using System;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {

    public UnityEvent actionsOnInteract;
    public EntityEvent actionsOnInteractWithEntity;

    public virtual void OnInteract(Entity entity)
    {
        actionsOnInteract.Invoke();
        actionsOnInteractWithEntity.Invoke(entity);
    }
}

[Serializable]
public class EntityEvent : UnityEvent<Entity> {}