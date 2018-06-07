using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSeat : Seat {

    public GameObject turret;

    public override void WhileSeated(Entity _entity) {
        base.WhileSeated(_entity);
        if (!turret)
            throw new System.Exception("There is no Light Attached");
        turret.transform.Rotate(Vector3.forward, -inputs.axis.x);

        print("yello");
    }
}
