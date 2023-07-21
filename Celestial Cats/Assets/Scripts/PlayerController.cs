using UnityEngine;

// tutorial: https://youtu.be/u8tot-X_RBI

public class PlayerController : MonoBehaviour {

    public float MovementSpeed = 1.0f;

    public Rigidbody2D Rigidbody;

    private Vector2 _moveDirection;

    // note to self: Update depends on framerate, good for processing input
    private void Update() {
        HandleInput();
    }

    // note to self: FixedUpdate happens a set amount of times per frame, good for physics calculations
    private void FixedUpdate() {
        Move();
    }

    private void HandleInput() {

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        _moveDirection = new Vector2(moveX, moveY).normalized;
    }

    private void Move() {
        Rigidbody.velocity = new Vector2(_moveDirection.x * MovementSpeed, _moveDirection.y * MovementSpeed);
    }
}