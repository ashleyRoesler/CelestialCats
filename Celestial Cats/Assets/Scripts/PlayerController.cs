using UnityEngine;

// tutorial: https://youtu.be/u8tot-X_RBI

public class PlayerController : MonoBehaviour {

    public Rigidbody2D Rigidbody;

    public float MovementSpeed = 1.0f;
    public float SpaceResistance = 0.5f;

    [Space]
    public GameObject ProjectilePrefab;
    public float ProjectileOffset = 1.0f;

    private Vector2 _moveDirection;

    private float PowerupProgress = 0f;

    public event System.Action<float> PowerupProgressChanged;

    // note to self: Update depends on framerate, good for processing input
    private void Update() {
        HandleInput();
    }

    // note to self: FixedUpdate happens a set amount of times per frame, good for physics calculations
    private void FixedUpdate() {
        Move();
    }

    private void HandleInput() {

        // move
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        _moveDirection = new Vector2(moveX, moveY).normalized;

        // shoot projectile
        if (Input.GetButtonDown("Fire1")) {
            Instantiate(ProjectilePrefab, new Vector2(gameObject.transform.position.x + ProjectileOffset, gameObject.transform.position.y), Quaternion.identity);
        }
    }

    private void Move() {
        Rigidbody.velocity = new Vector2(_moveDirection.x * MovementSpeed - SpaceResistance, _moveDirection.y * MovementSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        
        // check if the other thing is a piece of the universe
        // if so, increase powerup progress and eat the universe
        if (collision.gameObject.GetComponent<PieceOfTheUniverse>()) {
            PowerupProgress++;
            Destroy(collision.gameObject);

            PowerupProgressChanged?.Invoke(PowerupProgress);
        }
    }
}