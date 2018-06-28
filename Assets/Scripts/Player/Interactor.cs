using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour, PlayerInputReceiver
{
    public int InputOrder => InputReceiverOrder.Interactor;
    public bool ReceiveInput { get; set; }

    GameObject selectionHologram;
    MeshFilter hologramMeshFilter;
    public Material hologramMaterial;
    public Color hologramColor;

    private void Start()
    {
        ReceiveInput = true;
        selectionHologram = new GameObject(name + "_Selection Hologram");
        selectionHologram.AddComponent<MeshRenderer>().material = new Material(hologramMaterial)
        {
            color = hologramColor,
        };
        hologramMeshFilter = selectionHologram.AddComponent<MeshFilter>();
    }

    private readonly List<IInteractable> closeInteractables = new List<IInteractable>();

    public void OnUpdate(Inputs inputs)
    {
        Physics2DHelper.GetAllNear(transform.position, 1f, -1, closeInteractables);
        closeInteractables.Sort(SortInteractables);
        HideHologram();

        if (closeInteractables.Count == 0)
            return;

        var interactable = closeInteractables[0];
        if (inputs.interactDown)
            interactable.OnInteractDown();
        if (inputs.interactHeld)
            interactable.OnInteractHeld();
        ShowHologram(interactable);
    }

    private int SortInteractables(IInteractable x, IInteractable y)
    {
        var myPos = transform.position;
        var xDist = Vector3.SqrMagnitude(((Component) x).transform.position - myPos);
        var yDist = Vector3.SqrMagnitude(((Component) y).transform.position - myPos);

        return xDist.CompareTo(yDist);
    }

    private void ShowHologram(IInteractable interactable)
    {
        selectionHologram.SetActive(true);
        MonoBehaviour mb = ((MonoBehaviour) interactable);
        MeshFilter mf = mb.GetComponentInChildren<MeshFilter>();
        hologramMeshFilter.mesh = mf.mesh;
        selectionHologram.transform.position = mf.transform.position;
        selectionHologram.transform.localScale = mf.transform.localScale;
        selectionHologram.transform.rotation = mf.transform.rotation;
    }

    private void HideHologram()
    {
        selectionHologram.SetActive(false);
    }
}