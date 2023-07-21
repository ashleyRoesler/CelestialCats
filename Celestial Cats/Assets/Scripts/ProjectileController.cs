using UnityEngine;

public class ProjectileController : MonoBehaviour {

    public Rigidbody2D Rigidbody;

    public float MovementSpeed = 4.0f;

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