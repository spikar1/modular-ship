using UnityEngine;

public class Player : Entity
{
    public override void Start()
    {
        base.Start();
        transform.parent = directionParent;
    }
    
    protected override void NormalStateUpdate()
    {
        moveInput = Camera.main.transform.TransformDirection(moveInput);
        base.NormalStateUpdate();
        if (Input.GetButtonDown("Jump"))
            Interact();

        if (directionParent)
            dir = directionParent.right * moveInput.x + directionParent.up * moveInput.y;
        else
            dir = moveInput;

        dir = moveInput;
        Vector2 velocity = new Vector3(dir.x, dir.y) * maxSpeed;
        controller.Move(velocity * Time.deltaTime, collidableMask);
    }


    protected override void SeatedUpdate()
    {
        base.SeatedUpdate();
        if (Input.GetButtonDown("Jump"))
            Interact();

        if (!directionParent)
            directionParent = seat.transform;
    }
}