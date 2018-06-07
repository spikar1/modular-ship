using UnityEngine;
using System.Collections;

public class LightSeat : Seat
{

    public GameObject lightObj;

    public override void Update()
    {
        base.Update();
        
    }
    public override void OnEject(Entity _entity) 
    {
        base.OnEject(_entity);
        inputs.axis = Vector2.zero;
    }
    public override void WhileSeated(Entity _entity)
    {
        base.WhileSeated(_entity);

        if (!lightObj)
            throw new System.Exception("There is no Light Attached");
        lightObj.transform.localPosition += inputs.axis.toVector3().normalized * .3f;
    }
    public override void OnSeated(Entity _entity)
    {
        base.OnSeated(_entity);
    }
}
    

