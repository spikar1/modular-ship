using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IDamagable {
    public int integrity;

    private int maxIntegrity;
    private Renderer wallRenderer;

    private static Material materialTemplate;
    private static readonly Dictionary<int, Material> integrityToRenderer = new Dictionary<int, Material>();

    void Start() {
        wallRenderer = GetComponent<Renderer>();
        maxIntegrity = integrity;

        if (materialTemplate == null)
            materialTemplate = wallRenderer.sharedMaterial;
        SetMaterialForDurability();
    }

    public void Damage(Vector3 direction, float damage) {
        integrity -= (Mathf.FloorToInt(damage));
        if (integrity <= 0)
            DestroyWall();
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

    void DestroyWall() {
        Destroy(gameObject);
    }
}   
