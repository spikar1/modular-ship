using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour {

    public Sprite[] cloudSprites;
    public int amount = 50;
    public float width = 100, height = 100;
	// Use this for initialization
	void Start () {
        for (int i = 0; i < amount; i++) {
            SpriteRenderer sr = new GameObject("Cloud_" + i).AddComponent<SpriteRenderer>();
            sr.sprite = cloudSprites[Random.Range(0, cloudSprites.Length)];
            sr.transform.position = new Vector3(Random.Range(-width*.5f, width*.5f), Random.Range(-height * .5f, height * .5f), Random.Range(10f, 40f));
            sr.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            float scale = Random.Range(1f, 3f);
            sr.transform.localScale = new Vector3(scale, scale, 2);
            float f = Mathf.Clamp(Mathf.InverseLerp(40F, 10F, sr.transform.position.z), .6f, 1f);
            sr.color = new Color(f, f, f, f*.5f);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
