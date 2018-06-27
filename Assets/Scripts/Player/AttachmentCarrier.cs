using System.Collections.Generic;
using UnityEngine;

public class AttachmentCarrier : MonoBehaviour, IInputReceiver
{
    public int InputOrder => InputReceiverOrder.AttachmentCarrier;
    public bool ReceiveInput { get; set; }

    private Attachment carriedAttachment;
    private Sitter sitter;
    private Interactor interactor;

    private List<Wall> wallBuffer = new List<Wall>();
    private List<Attachment> attachmentBuffer = new List<Attachment>();

    private void Awake()
    {
        ReceiveInput = true;
        sitter = GetComponent<Sitter>();
        interactor = GetComponent<Interactor>();
    }
    
    public void OnUpdate(Inputs inputs)
    {
        if (!inputs.interactDown) 
            return;

        if (carriedAttachment != null)
        {
            Physics2DHelper.GetAllNear(transform.position, .5f, -1, wallBuffer);
            if (carriedAttachment.TryAttachToNearest(wallBuffer))
            {
                carriedAttachment = null;
                sitter.ReceiveInput = true;
                interactor.ReceiveInput = true;
            }
        }
        else
        {
            Physics2DHelper.GetAllNear(transform.position, .5f, -1, attachmentBuffer);
            foreach (var attachment in attachmentBuffer)
            {
                if (attachment.CanBePickedUp())
                {
                    carriedAttachment = attachment;
                    attachment.OnPickUp();
                    attachment.transform.parent = transform;
                    attachment.transform.localPosition = Vector3.zero;
                    sitter.ReceiveInput = false;
                    interactor.ReceiveInput = false;
                    break;
                }
            }
        }
    }
}