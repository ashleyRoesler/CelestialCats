using UnityEngine;

public class Projectile : MonoBehaviour {

    public Rigidbody2D Rigidbody;

    public float DamageAmount = 5f;
    public float MovementSpeed = 4f;

    public bool DestroyOnHit = true;

    private void FixedUpdate() {
        Move();
    }

    private void Move() {
        Rigidbody.velocity = new Vector2(MovementSpeed, 0);
    }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}