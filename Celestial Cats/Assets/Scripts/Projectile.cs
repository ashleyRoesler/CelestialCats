using UnityEngine;

public class Projectile : MonoBehaviour {

    public Rigidbody2D Rigidbody;

    public float DamageAmount = 5f;
    public float MovementSpeed = 4f;

    [HideInInspector]
    public Vector2 MoveDirection = new(1f, 0f);

    public bool DestroyOnHit = true;

    [Header("0 = Player, 1 = Enemy"), Range(0, 1)]
    public int Team = 0;

    private void FixedUpdate() {
        Move();
    }

    private void Move() {
        Rigidbody.velocity = new Vector2(MoveDirection.x * MovementSpeed, MoveDirection.y * MovementSpeed);
    }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}