using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkHealth : NetworkBehaviour {

    public const int maxHealth = 100;
    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    public void TakeDamage(int amount) {
        if (!isServer)
            return;
        currentHealth -= amount;
        if(currentHealth <= 0) {
            currentHealth = 0;

        }
    }

    void OnChangeHealth(int health) {
        GetComponent<MeshRenderer>().material.color = Color.red;
        
    }
}
