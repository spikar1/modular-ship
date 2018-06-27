using UnityEngine;
using System.Collections.Generic;

public class Attachment : MonoBehaviour
{
    private bool IsCarried => attachedTo == null;
    private IAttachable[] attachables;
    private Wall attachedTo;

    void Awake()
    {
        attachables = GetComponents<IAttachable>();
    }

    public bool CanBePickedUp()
    {
        return !IsCarried; //@TODO: Raycast maybe?
    }
    
    public void OnPickUp()
    {
        foreach (var attachable in attachables)
            attachable.OnDetach();
        attachedTo.attachedThing = null;
    }

    public bool TryAttachToNearest(List<Wall> walls)
    {
        if(walls.Count == 0)
        {
            return false;
        }

        Wall closest = null;
        var closestDistance = Mathf.Infinity;
        foreach (var wall in walls)
        {
            if(wall.attachedThing != null)
                continue;
                
            var distance = Vector2.Distance(wall.transform.position, transform.position);
            if (distance < closestDistance)
            {
                closest = wall;
                closestDistance = distance;
            }
        }

        if (closest == null)
        {
            Debug.LogWarning($"Couldn't find any free walls when attaching {name}", gameObject);
            return false;
        }

        closest.attachedThing = this;
        attachedTo = closest;
        transform.parent = closest.transform;
        transform.localPosition = closest.attachPoint;
        transform.localRotation = closest.orientation.ToLookRotation();
        foreach (var attachable in attachables)
            attachable.OnAttachedTo(closest);
        return true; 
    }
}

public interface IAttachable
{
    void OnAttachedTo(Wall wall);
    void OnDetach();
}