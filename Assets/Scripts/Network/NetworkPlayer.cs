 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour {

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    void Update() {
        if (!isLocalPlayer)
            return;

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, 0, -x);
        transform.Translate(0, z, 0);

        if (Input.GetKeyDown(KeyCode.Space))
            CmdShoot();
    }

    public override void OnStartLocalPlayer() {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
    


    [Command]
    void CmdShoot() {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * 6;

        //Spawn bullet on clients
        NetworkServer.Spawn(bullet, new NetworkHash128());

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }
}
