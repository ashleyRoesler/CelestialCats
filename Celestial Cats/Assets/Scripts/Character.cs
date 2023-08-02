using UnityEngine;

// https://youtu.be/u8tot-X_RBI

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

    [Space]
    public float DamageAmount = 10f;

    private void Awake() {
        currentMovementSpeed = MovementSpeed;
        manager = FindObjectOfType<InGameManager>();

        manager.LevelWon += Manager_LevelWon;
        Health.Died += Health_Died;
    }

    private void OnDisable() {

        if (manager) {
            manager.LevelWon -= Manager_LevelWon;
        }

        Health.Died -= Health_Died;
    }

    // note to self: FixedUpdate happens a set amount of times per frame, good for physics calculations
    // note to self: FixedUpdate is NOT called when Time.timeScale == 0
    private void FixedUpdate() {
        Move();
    }

    protected virtual void Manager_LevelWon() {
        currentMovementSpeed = 0f;
        SpaceResistance = 0f;
        moveDirection = new Vector2(0, 0);
    }

    protected virtual void Move() {
        Rigidbody.velocity = new Vector2(moveDirection.x * currentMovementSpeed - SpaceResistance, moveDirection.y * currentMovementSpeed);
    }

    public void Shoot() {
        Instantiate(ProjectilePrefab, new Vector2(transform.position.x + ProjectileOffset, transform.position.y), Quaternion.identity);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        
        // take damage from attacks
        if (collision.gameObject.GetComponent<Projectile>()) {

            Projectile p = collision.gameObject.GetComponent<Projectile>();

            Health.TakeDamage(p.DamageAmount);

            if (p.DestroyOnHit) {
                Destroy(collision.gameObject);
            }
        }
    }

    private void Health_Died() {
        Die();
    }

    protected abstract void Die();
}