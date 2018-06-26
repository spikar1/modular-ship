using UnityEngine;

public class Player : Entity
{
    public int player;
    public override void Start()
    {
        base.Start();
        transform.parent = directionParent;
    }

    void GetPlayerInput() {
        switch (player) {
            case 0:
                moveInput = new Vector2();
                moveInput.x += Input.GetKey(KeyCode.D) ? 1 : 0;
                moveInput.x -= Input.GetKey(KeyCode.A) ? 1 : 0;
                moveInput.y += Input.GetKey(KeyCode.W) ? 1 : 0;
                moveInput.y -= Input.GetKey(KeyCode.S) ? 1 : 0;
                leftBumper = Input.GetKey(KeyCode.Q);
                rightBumper = Input.GetKey(KeyCode.E);
                interactButton = Input.GetKey(KeyCode.Space);
                interactButtonDown = Input.GetKeyDown(KeyCode.Space);
                interactButtonUp = Input.GetKeyUp(KeyCode.Space);
                action = Input.GetKey(KeyCode.LeftShift);
                actionDown = Input.GetKeyDown(KeyCode.LeftShift);
                actionUp = Input.GetKeyUp(KeyCode.LeftShift);
                break;
            case 1:
                moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                leftBumper = Input.GetButton("LeftBumper");
                rightBumper = Input.GetButton("RightBumper");
                interactButton = Input.GetButton("Fire1");
                interactButtonDown = Input.GetButtonDown("Fire1");
                interactButtonUp = Input.GetButtonUp("Fire1");
                action = Input.GetButton("Fire2");
                actionDown = Input.GetButtonDown("Fire2");
                actionUp = Input.GetButtonUp("Fire2");
                break;
            default:
                Debug.LogError("The player int " + player + " is not supported");
                break;
        }
    }

    public override void Update() {
        GetPlayerInput();
        base.Update();
    }
}