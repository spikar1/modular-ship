using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour {

    public static void ShowDamageText(GameObject go, float amount) {

        DamageText dt = new GameObject().AddComponent<DamageText>();
        dt.transform.position = go.transform.position;
        dt.ShowDamageText(go.transform.position, amount);
    }

    public void ShowDamageText(Vector3 pos, float amount) {
        if (amount < 1) {
            Destroy(this);
            return;
        }
        //GameObject go = new GameObject();
        TextMesh tm = gameObject.AddComponent<TextMesh>();
        tm.text = Mathf.Round(amount).ToString();
        tm.characterSize = .05f;
        tm.fontSize = 150;
        tm.color = Color.red;
        tm.anchor = TextAnchor.MiddleCenter;
        tm.transform.position = transform.position + Vector3.up * .2f;
        tm.transform.rotation = Quaternion.Euler(-28, 0, 0);
        
        StartCoroutine(TextEffect(tm));
    }

    IEnumerator TextEffect(TextMesh tm) {
        float t = 0;
        Color c = tm.color;
        while(t < 1) {
            transform.localPosition += Vector3.up * .001f;
            tm.color = new Color(c.r, c.g, c.b, 1 - t);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);

    }
}
