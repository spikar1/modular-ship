using UnityEngine;
using System.Collections.Generic;

public class Attachment : MonoBehaviour, ICarryable
{
    private bool IsCarried => attachedTo == null;
    private IAttachable[] attachables;
    private Wall attachedTo;

    void Awake()
    {
        attachables = GetComponents<IAttachable>();
    }


    public void Use() { }

    public bool TryPickUp(Carrier carrier)
    {
        if (IsCarried)
            return false;

        foreach (var attachable in attachables)
            attachable.OnDetach();

        attachedTo.attachedThing = null;
        attachedTo = null;
        transform.parent = carrier.transform;
        transform.localPosition = Vector3.zero;

        return true;
    }

    private readonly List<Wall> wallBuffer = new List<Wall>();

    public bool TryPutDown()
    {
        Physics2DHelper.GetAllNear(transform.position, .5f, -1, wallBuffer);
        if (wallBuffer.Count == 0)
        {
            return false;
        }

        Wall closest = null;
        var closestDistance = Mathf.Infinity;
        foreach (var wall in wallBuffer)
        {
            if (wall.attachedThing != null)
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