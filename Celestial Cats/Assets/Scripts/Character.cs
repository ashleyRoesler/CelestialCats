using UnityEngine;

// https://youtu.be/u8tot-X_RBI
// https://discussions.unity.com/t/how-to-keep-an-object-within-the-camera-view/117989

public abstract class Character : MonoBehaviour {

    protected InGameManager manager;

    public Rigidbody2D Rigidbody;
    public HealthComponent Health;

    [Space]
    public float MovementSpeed = 1f;
    public float SpaceResistance = 0.5f;
    protected float currentMovementSpeed = 1f;
    protected Vector2 moveDirection;

    [Space]
    public GameObject ProjectilePrefab;
    public float ProjectileOffset = 1f;

    private void Awake() {
        currentMovementSpeed = MovementSpeed;
        manager = FindObjectOfType<InGameManager>();

        manager.LevelWon += Manager_LevelWon;
    }

    // note to self: FixedUpdate happens a set amount of times per frame, good for physics calculations
    private void FixedUpdate() {
        Move();
    }

    protected virtual void Manager_LevelWon() {
        currentMovementSpeed = 0f;
        SpaceResistance = 0f;
        moveDirection = new Vector2(0, 0);
    }

    protected void Move() {

        Rigidbody.velocity = new Vector2(moveDirection.x * currentMovementSpeed - SpaceResistance, moveDirection.y * currentMovementSpeed);

        // keep the character on the screen
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);

        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    public void Shoot() {
        Instantiate(ProjectilePrefab, new Vector2(transform.position.x + ProjectileOffset, transform.position.y), Quaternion.identity);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        
        // take damage from attacks
        if (collision.gameObject.GetComponent<Projectile>()) {

            Projectile p = collision.gameObject.GetComponent<Projectile>();

            Health.TakeDamage(p.DamageAmount);

            if (Health.IsDead) {
                Die();
            }

            if (p.DestroyOnHit) {
                Destroy(collision.gameObject);
            }
        }
    }

    protected abstract void Die();
}