using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour {

    public void ShowDamageText(float amount) {
        if (amount < 1) {
            Destroy(this);
            return;
        }
        GameObject go = new GameObject();
        TextMesh tm = go.AddComponent<TextMesh>();
        tm.text = amount.ToString();
        tm.characterSize = .05f;
        tm.fontSize = 36;
        tm.anchor = TextAnchor.MiddleCenter;
        tm.transform.position = transform.position + Vector3.up * .2f;
        tm.transform.rotation = Quaternion.Euler(-28, 0, 0);

        StartCoroutine(TextEffect(go));
    }

    IEnumerator TextEffect(GameObject _go) {
        float t = 0;
        while(t < 1) {
            _go.transform.localPosition += Vector3.up * .001f;
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        Destroy(_go);
        Destroy(this);

    }
}
