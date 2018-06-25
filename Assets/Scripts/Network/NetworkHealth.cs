using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkHealth : NetworkBehaviour {

    public const int maxHealth = 100;
    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    Color color;
    bool isBeingShot = false;

    public void TakeDamage(int amount) {
        if (!isServer)
            return;
        currentHealth -= amount;
        if(currentHealth <= 0) {
            currentHealth = 0;

        }
    }

    void OnChangeHealth(int health) {
        print("I'm hit!");
        if (!isBeingShot) {
            color = GetComponent<MeshRenderer>().material.color;
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else
            CancelInvoke("ResetColor");

        Invoke("ResetColor", .2f);

    }

    void ResetColor() {
        GetComponent<MeshRenderer>().material.color = color;
        isBeingShot = false;
    }
}
