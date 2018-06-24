using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Interactable : MonoBehaviour {

    public UnityEvent actionsOnInteract;

    public virtual void OnInteract(Entity _entity)
    {
        print(_entity.transform.name + " interacted with " + transform.name);
        actionsOnInteract.Invoke();
    }
    
}
