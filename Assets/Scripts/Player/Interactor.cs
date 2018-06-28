using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour, IToggelableInputReceiver
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
        selectionHologram = new GameObject(name+"_Selection Hologram");
        Material mat = selectionHologram.AddComponent<MeshRenderer>().material = Instantiate(hologramMaterial);
        mat.color = hologramColor;
        hologramMeshFilter = selectionHologram.AddComponent<MeshFilter>();
        
        
    }
    private readonly List<IInteractable> closeInteractables = new List<IInteractable>();
    public void OnUpdate(Inputs inputs){
        /*if (!inputs.interactDown && !inputs.interactHeld)
            return;*/
        Physics2DHelper.GetAllNear(transform.position, 1f, -1, closeInteractables);
        HideHologram();
        foreach (var interactable in closeInteractables)
        {
            if (inputs.interactDown)
                interactable.OnInteractDown();
            if (inputs.interactHeld)
                interactable.OnInteractHeld();
            ShowHologram(interactable);
        }     
    }

    void ShowHologram(IInteractable interactable)
    {
        selectionHologram.SetActive(true);
        MonoBehaviour mb = ((MonoBehaviour)interactable);
        MeshFilter mf = mb.GetComponentInChildren<MeshFilter>();
        hologramMeshFilter.mesh = mf.mesh;
        selectionHologram.transform.position = mf.transform.position;
        selectionHologram.transform.localScale = mf.transform.localScale;
        selectionHologram.transform.rotation = mf.transform.rotation;
    }

    void HideHologram() {
        selectionHologram.SetActive(false);
    }
}