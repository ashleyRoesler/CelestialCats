using UnityEngine;

public class PieceOfTheUniverse : MonoBehaviour {

    public Rigidbody2D Rigidbody;

    public float MovementSpeed = -1f;

    public float SupernovaValue = 1f;

    private void FixedUpdate() {
        Move();
    }

    private void Move() {
        Rigidbody.velocity = new Vector2(MovementSpeed, 0f);
    }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}