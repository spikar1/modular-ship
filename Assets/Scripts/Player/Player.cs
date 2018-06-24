using UnityEngine;
using System.Collections;

public class Player : Entity
{
    public override void Start()
    {
        base.Start();
        transform.parent = directionParent;
    }

    public void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        leftBumper = Input.GetKey(KeyCode.Q);
        rightBumper = Input.GetKey(KeyCode.E);


        switch (state)
        {
            case State.normal:
                NormalStateUpdate();
                break;
            case State.seated:
                Seated();
                break;
            case State.dead:
                break;
            default:
                break;
        }
    }

    public override void NormalStateUpdate()
    {
        base.NormalStateUpdate();
        if (Input.GetButtonDown("Jump")) {
            Interact();
        }

        Camera.main.transform.parent = transform;
        Camera.main.transform.position = new Vector3(transform.position.x,  transform.position.y, Camera.main.transform.position.z);
        if (directionParent)
        {
            dir = directionParent.right * moveInput.x + directionParent.up * moveInput.y;
        }
        else
        {
            dir = moveInput;
        }
        dir = moveInput;
        //Vector2 lookDir;
        Vector2 velocity = new Vector3(dir.x, dir.y) * maxSpeed;
        controller.Move(velocity * Time.deltaTime, collidableMask);
        //transform.LookAt(lookDir.toVector3() + transform.position, Vector3.back);
    }


    public override void Seated() {
        base.Seated();
        if (Input.GetButtonDown("Jump")) {
            Interact();
        }
        if (!directionParent) {
            directionParent = seat.transform;
        }
    }

}
