using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.Networking;

public class Interactable : NetworkBehaviour {

    public UnityEvent actionsOnInteract;

    public virtual void OnInteract(Entity _entity)
    {
        if (!isServer)
            return;
        print(_entity.transform.name + " interacted with " + transform.name);
        actionsOnInteract.Invoke();
    }


}
