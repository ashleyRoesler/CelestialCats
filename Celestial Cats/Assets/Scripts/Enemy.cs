using UnityEngine;

public class Enemy : Character {

    protected override void Die() {
        Destroy(gameObject);
    }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}