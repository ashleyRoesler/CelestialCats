using UnityEngine;

public class Enemy : Character {

    [Space]
    public float DamageAmount = 10f;

    protected override void Die() {
        Destroy(gameObject);
    }
}