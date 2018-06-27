using System.Collections.Generic;
using UnityEngine;

public class Sitter : MonoBehaviour, IToggelableInputReceiver
{
    private Mover mover;
    private Interactor interactor;
    private AttachmentCarrier attachmentCarrier;

    private Seat currentSeat;

    public int InputOrder => InputReceiverOrder.Sitter;
    public bool ReceiveInput { get; set; }

    private void Awake()
    {
        mover = GetComponent<Mover>();
        interactor = GetComponent<Interactor>();
        attachmentCarrier = GetComponent<AttachmentCarrier>();
        ReceiveInput = true;
    }

    public void OnUpdate(Inputs inputs)
    {
        if (!currentSeat)
        {
            if (inputs.sitDown)
            {
                var closestSeat = FindClosestSeat();
                if (closestSeat)
                {
                    SitIn(closestSeat);
                }
            }
        }
        else
        {
            if (inputs.sitDown)
            {
                GetUp();
            }
            else
            {
                currentSeat.OnUpdate(inputs);
            }
        }
    }

    private readonly List<Seat> findSeatBuffer = new List<Seat>();
    private Seat FindClosestSeat()
    {
        Physics2DHelper.GetAllNear(transform.position, .4f, -1, findSeatBuffer);
        for (int i = findSeatBuffer.Count - 1; i >= 0; i--)
        {
            if(findSeatBuffer[i].sitter != null)
                findSeatBuffer.RemoveAt(i);
        }

        if (findSeatBuffer.Count == 0)
            return null;

        findSeatBuffer.Sort(CompareSeatDistances);
        return findSeatBuffer[0];
    }

    private int CompareSeatDistances(Seat s1, Seat s2)
    {
        var s1Dist = Vector2.SqrMagnitude(s1.transform.position - transform.position);
        var s2Dist = Vector2.SqrMagnitude(s2.transform.position - transform.position);
        return s1Dist.CompareTo(s2Dist);
    }

    private void SitIn(Seat seat)
    {
        SetOtherReceiversReceiveInput(false);
        currentSeat = seat;
        seat.sitter = this;
        transform.position = seat.transform.position;
    }

    private void GetUp()
    {
        if (currentSeat != null)
        {
            currentSeat.sitter = null;
            transform.position = currentSeat.ejectPoint;
        }
        else
        {
            Debug.LogError("Trying to get up with no seat!");
        }

        currentSeat = null;
        SetOtherReceiversReceiveInput(true);
    }
    
    private void SetOtherReceiversReceiveInput(bool receiveInput)
    {
        mover.ReceiveInput = receiveInput;
        interactor.ReceiveInput = receiveInput;
        attachmentCarrier.ReceiveInput = receiveInput;
    }
}