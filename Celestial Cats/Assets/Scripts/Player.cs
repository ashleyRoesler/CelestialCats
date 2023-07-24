using UnityEngine;

// https://youtu.be/u8tot-X_RBI
// https://www.techwithsach.com/post/how-to-add-a-simple-countdown-timer-in-unity

public class Player : MonoBehaviour {

    public Rigidbody2D Rigidbody;
    public HealthComponent Health;

    [Space]
    public float MovementSpeed = 1f;
    public float SpaceResistance = 0.5f;
    private float _currentMovementSpeed = 1f;

    [Space]
    public GameObject ProjectilePrefab;
    public float ProjectileOffset = 1f;

    [Space]
    public float StarDuration = 4f;
    public float StarSpeed = 8f;
    private float _currentStarDuration = 0f;

    private Vector2 _moveDirection;

    private SpecialAbility _currentSpecialAbility = SpecialAbility.None;
    private float PowerupProgress = 0f;

    public event System.Action<float> PowerupProgressChanged;
    public event System.Action<SpecialAbility> SpecialAbilityChanged;

    private void Awake() {
        _currentMovementSpeed = MovementSpeed;
    }

    // note to self: Update depends on framerate, good for processing input
    private void Update() {
        HandleInput();

        // start Star special ability
        if (_currentStarDuration == StarDuration) {
            Health.CanBeDamaged = false;
            _currentMovementSpeed = StarSpeed;
        }

        // update Star duration
        if (_currentStarDuration > 0f) {

            _currentStarDuration -= Time.deltaTime;

            if (_currentStarDuration <= 0f) {
                Health.CanBeDamaged = true;
                _currentMovementSpeed = MovementSpeed;
            }
        }
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

        // use special ability
        if (Input.GetButtonDown("Fire2") && _currentSpecialAbility != SpecialAbility.None) {
            UseSpecialAbility();
        }
    }

    private void Move() {
        Rigidbody.velocity = new Vector2(_moveDirection.x * _currentMovementSpeed - SpaceResistance, _moveDirection.y * _currentMovementSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        
        // check if the other thing is a piece of the universe
        // if so, increase powerup progress and eat the universe
        if (collision.gameObject.GetComponent<PieceOfTheUniverse>()) {
            
            // check if star
            if (collision.gameObject.name.ToLower().Contains("star") && _currentSpecialAbility != SpecialAbility.Star) {
                _currentSpecialAbility = SpecialAbility.Star;
                SpecialAbilityChanged?.Invoke(_currentSpecialAbility);
            }

            // check if magic
            else if (collision.gameObject.name.ToLower().Contains("satellite") && _currentSpecialAbility != SpecialAbility.Blast) {
                _currentSpecialAbility = SpecialAbility.Blast;
                SpecialAbilityChanged?.Invoke(_currentSpecialAbility);
            }
            
            else {
                PowerupProgress++;
                PowerupProgressChanged?.Invoke(PowerupProgress);
            }
            
            Destroy(collision.gameObject);
        }
    }

    private void UseSpecialAbility() {

        switch (_currentSpecialAbility) {
            case SpecialAbility.None:
                break;
            case SpecialAbility.Star:
                _currentStarDuration = StarDuration;
                break;
            case SpecialAbility.Blast:
                break;
            default:
                Debug.LogError("Unknown Special Ability: " + _currentSpecialAbility);
                break;
        }

        _currentSpecialAbility = SpecialAbility.None;
        SpecialAbilityChanged?.Invoke(_currentSpecialAbility);
    }
}

public enum SpecialAbility {
    None,
    Star,
    Blast
}