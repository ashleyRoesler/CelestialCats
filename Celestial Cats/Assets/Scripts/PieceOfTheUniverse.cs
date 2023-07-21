using UnityEngine;

public class PieceOfTheUniverse : MonoBehaviour {

    public Rigidbody2D Rigidbody;

    public float MovementSpeed = -1.0f;

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