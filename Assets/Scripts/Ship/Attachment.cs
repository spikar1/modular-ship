﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class Attachment : MonoBehaviour
{
    private bool isCarried;
    private IAttachable[] attachables;
    private Wall attachedTo;

    void Awake()
    {
        attachables = GetComponents<IAttachable>();
    }

    //Called by UnityEvent
    public void TryPickUp(Entity picker)
    {
        if (isCarried)
            return;

        picker.StartCarrying(this);
        isCarried = true;
        attachedTo.attachedThing = null;
        foreach (var attachable in attachables)
            attachable.OnDetach();
    }

    public bool TryAttachToNearest(List<Wall> walls)
    {
        if(walls.Count == 0)
        {
            Debug.LogError($"Asked to attach{name} to nearest of no walls!", gameObject);
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

        if (closestDistance > 1f)
            Debug.LogWarning($"The closest wall to {name} is more than 1f away!", gameObject);

        closest.attachedThing = this;
        attachedTo = closest;
        transform.parent = closest.transform;
        transform.localPosition = closest.attachPoint;
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