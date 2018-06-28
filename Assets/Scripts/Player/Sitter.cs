using System.Collections.Generic;
using UnityEngine;

public class Sitter : MonoBehaviour, IPlayerInputReceiver {
    private Mover mover;
    private Interactor interactor;

    private Seat currentSeat;

    public int InputOrder => InputReceiverOrder.Sitter;
    public bool ReceiveInput { get; set; }

    GameObject selectionHologram;
    MeshFilter hologramMeshFilter;
    public Material hologramMaterial;
    public Color hologramColor;

    private void Awake() {
        mover = GetComponent<Mover>();
        interactor = GetComponent<Interactor>();

        ReceiveInput = true;

        selectionHologram = new GameObject("Selection Hologram");
        Material mat = selectionHologram.AddComponent<MeshRenderer>().material = Instantiate(hologramMaterial);
        mat.color = hologramColor;
        hologramMeshFilter = selectionHologram.AddComponent<MeshFilter>();
    }

    public void OnUpdate(Inputs inputs) {
        if (!currentSeat) {
            var closestSeat = FindClosestSeat();
            if (closestSeat) {
                ShowHologram(closestSeat);
                if (inputs.sitDown) {
                    SitIn(closestSeat);
                }
            }
            else
                HideHologram();
        }
        else {
            if (inputs.sitDown) {
                GetUp();
            }
            else {
                currentSeat.OnUpdate(inputs);
            }
        }
    }

    private readonly List<Seat> findSeatBuffer = new List<Seat>();

    private Seat FindClosestSeat() {
        Physics2DHelper.GetAllNear(transform.position, .4f, -1, findSeatBuffer);
        for (int i = findSeatBuffer.Count - 1; i >= 0; i--) {
            if (findSeatBuffer[i].sitter == null)
                continue;
            findSeatBuffer.RemoveAt(i);
        }

        if (findSeatBuffer.Count == 0)
            return null;

        findSeatBuffer.Sort(CompareSeatDistances);
        return findSeatBuffer[0];
    }

    private int CompareSeatDistances(Seat s1, Seat s2) {
        var s1Dist = Vector2.SqrMagnitude(s1.transform.position - transform.position);
        var s2Dist = Vector2.SqrMagnitude(s2.transform.position - transform.position);
        return s1Dist.CompareTo(s2Dist);
    }

    private void SitIn(Seat seat) {
        SetOtherReceiversReceiveInput(false);
        currentSeat = seat;
        seat.OnSeated(this);
        transform.position = seat.transform.position;
        HideHologram();
    }

    private void GetUp() {
        if (currentSeat != null) {
            currentSeat.SitterGotUp();
            transform.position = currentSeat.transform.position + Vector3.forward * .4f;
            GetComponent<CameraController>().target = transform;
        }
        else {
            Debug.LogError("Trying to get up with no seat!");
        }

        currentSeat = null;
        SetOtherReceiversReceiveInput(true);
    }

    private void SetOtherReceiversReceiveInput(bool receiveInput) {
        mover.ReceiveInput = receiveInput;
        interactor.ReceiveInput = receiveInput;
    }

    private void ShowHologram(Seat seat) {
        selectionHologram.SetActive(true);
        hologramMeshFilter.mesh = seat.GetComponentInChildren<MeshFilter>().mesh;
        selectionHologram.transform.position = seat.transform.position;
        selectionHologram.transform.localScale = seat.transform.localScale;
        selectionHologram.transform.rotation = seat.transform.rotation;
    }

    void HideHologram() {
        selectionHologram.SetActive(false);
    }
}