using System;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IDamagable {
    public int integrity;
    public WallOrientation orientation;

    [NonSerialized]
    public Attachment attachedThing;
    public RoomNode roomNode { get; private set; }
    
    //in local space
    public Vector3 attachPoint { get; private set; }

    private int maxIntegrity;
    private Renderer wallRenderer;

    private static Material materialTemplate;
    private static readonly Dictionary<int, Material> integrityToRenderer = new Dictionary<int, Material>();
    
    
    public void Initialize(RoomNode roomNode)
    {
        this.roomNode = roomNode;

        attachPoint = orientation.ToOffset() * .5f; 
        
        if (name.StartsWith("GameObject"))
            name = name.Replace("GameObject", "Wall");
        
        wallRenderer = GetComponent<Renderer>();
        if (wallRenderer == null)
        {
            Debug.LogError($"Missing wall renderer on {name}!", this);
            return;
        }
        maxIntegrity = integrity;

        if (materialTemplate == null)
            materialTemplate = wallRenderer.sharedMaterial;
        SetMaterialForDurability();
    }

    public void Damage(Vector2 relativeVelocity, float damage) {
        Debug.Log(relativeVelocity.magnitude);
        DamageText txt = gameObject.AddComponent<DamageText>();
        txt.ShowDamageText(damage);
        integrity -= (Mathf.FloorToInt(damage));
        if (integrity <= 0)
        {
            Destroy(gameObject);
        }
        else
            SetMaterialForDurability();

    }

    private void SetMaterialForDurability() {
        Material mat;
        
        var integrityFraction = integrity / (float) maxIntegrity;
        var integrityInt = Mathf.CeilToInt(integrityFraction * 100f);
        
        if (!integrityToRenderer.TryGetValue(integrityInt, out mat)) {
            mat = new Material(materialTemplate) {
                color = Color.Lerp(Color.black, Color.white, integrityInt / 100f)
            };
            integrityToRenderer[integrityInt] = mat;
        }

        wallRenderer.sharedMaterial = mat;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3) orientation.ToOffset());
    }
}   