using UnityEngine;

public abstract class Carryable : MonoBehaviour {
    private bool IsCarried;
    
    public abstract void Use();

    public virtual bool TryPickUp(AttachmentCarrier attachmentCarrier) {
        if (IsCarried)
            return false;

        IsCarried = true;
        transform.parent = attachmentCarrier.transform;
        transform.localPosition = Vector3.zero;
        return true;
    }
    
    public abstract bool TryPutDown(AttachmentCarrier attachmentCarrier);
}