using UnityEngine;
using System.Collections.Generic;

// https://youtu.be/u8tot-X_RBI
// https://www.techwithsach.com/post/how-to-add-a-simple-countdown-timer-in-unity
// https://discussions.unity.com/t/how-to-keep-an-object-within-the-camera-view/117989

public class Player : MonoBehaviour {

    public Rigidbody2D Rigidbody;
    public HealthComponent Health;

    private InGameManager _manager;

    [Space]
    public float MovementSpeed = 1f;
    public float SpaceResistance = 0.5f;
    private float _currentMovementSpeed = 1f;
    private Vector2 _moveDirection;

    [Space]
    public GameObject ProjectilePrefab;
    public GameObject BlastPrefab;
    public float ProjectileOffset = 1f;

    [Space]
    public float StarDuration = 4f;
    public float StarSpeed = 8f;
    private float _currentStarDuration = 0f;

    [Space]
    public float BlastDuration = 4f;
    private float _currentBlastDuration = 0f;
    private GameObject _blast;

    private SpecialAbility _currentSpecialAbility = SpecialAbility.None;
    private float _supernovaProgress = 0f;

    [Space]
    public List<Transform> NovaCloneSpawnPositions = new();
    public GameObject ClonePrefab;
    public SpriteRenderer SpriteRenderer;
    public float SupernovaDuration = 4f;
    public float NovaSpeed = 8f;
    private float _currentSupernovaDuration = 0f;
    private List<GameObject> SpawnedClones = new();

    public event System.Action<float> SupernovaProgressChanged;
    public event System.Action<SpecialAbility> SpecialAbilityChanged;

    private void Awake() {
        _currentMovementSpeed = MovementSpeed;
        _manager = FindObjectOfType<InGameManager>();
    }

    // note to self: Update depends on framerate, good for processing input
    private void Update() {
        HandleInput();

        // update Star duration
        if (_currentStarDuration > 0f) {

            _currentStarDuration -= Time.deltaTime;

            if (_currentStarDuration <= 0f) {
                Health.CanBeDamaged = true;
                _currentMovementSpeed = MovementSpeed;
            }
        }

        // update Blast duration
        if (_currentBlastDuration > 0f) {

            _currentBlastDuration -= Time.deltaTime;

            if (_currentBlastDuration <= 0f) {
                Destroy(_blast);
            }
        }

        // update Supernova duration
        if (_currentSupernovaDuration > 0f) {

            _currentSupernovaDuration -= Time.deltaTime;

            if (_currentSupernovaDuration <= 0f) {

                foreach(GameObject clone in SpawnedClones) {
                    Destroy(clone);
                }

                Color a = SpriteRenderer.color;
                a.a = 1f;

                SpriteRenderer.color = a;

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

        if (_currentSupernovaDuration <= 0f) {

            // shoot projectile
            if (Input.GetButtonDown("Fire1")) {
                Instantiate(ProjectilePrefab, new Vector2(transform.position.x + ProjectileOffset, transform.position.y), Quaternion.identity);
            }

            // use special ability
            if (Input.GetButtonDown("Fire2") && _currentSpecialAbility != SpecialAbility.None) {
                UseSpecialAbility();
            }

            // activate supernova
            if (Input.GetButtonDown("Fire3") && _supernovaProgress == 100f) {
                ActivateSupernova();
            }
        }
    }

    private void Move() {

        Rigidbody.velocity = new Vector2(_moveDirection.x * _currentMovementSpeed - SpaceResistance, _moveDirection.y * _currentMovementSpeed);

        // keep the player on the screen
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);

        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        
        // check if the other thing is a piece of the universe
        // if so, increase supernova progress and eat the universe
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
            
            else if (_supernovaProgress < 100f) {

                float add = collision.gameObject.GetComponent<PieceOfTheUniverse>().SupernovaValue;

                _supernovaProgress = _supernovaProgress + add <= 100f ? _supernovaProgress + add : 100f;

                SupernovaProgressChanged?.Invoke(_supernovaProgress);
            }
            
            Destroy(collision.gameObject);
        }
    }

    private void UseSpecialAbility() {

        switch (_currentSpecialAbility) {
            case SpecialAbility.None:
                break;
            case SpecialAbility.Star:
                Health.CanBeDamaged = false;
                _currentMovementSpeed = StarSpeed;
                _currentStarDuration = StarDuration;
                break;
            case SpecialAbility.Blast:

                if (!_blast) {
                    float blastWidth = _manager.CameraBottomRight.x - _manager.CameraBottomLeft.x;

                    _blast = Instantiate(BlastPrefab, new Vector2(transform.position.x + ProjectileOffset + blastWidth / 2f, transform.position.y), Quaternion.identity, transform);
                    _blast.transform.localScale = new Vector3(blastWidth, 0.5f, 1f);
                }

                _currentBlastDuration = BlastDuration;

                break;
            default:
                Debug.LogError("Unknown Special Ability: " + _currentSpecialAbility);
                break;
        }

        _currentSpecialAbility = SpecialAbility.None;
        SpecialAbilityChanged?.Invoke(_currentSpecialAbility);
    }

    private void ActivateSupernova() {

        // turn player invisible
        Color a = SpriteRenderer.color;
        a.a = 0f;

        SpriteRenderer.color = a;

        // spawn clones
        foreach(Transform spawnPoint in NovaCloneSpawnPositions) {
            GameObject clone = Instantiate(ClonePrefab, spawnPoint);
            SpawnedClones.Add(clone);
        }

        // apply Star ability effects
        Health.CanBeDamaged = false;
        _currentMovementSpeed = NovaSpeed;
        _currentSupernovaDuration = SupernovaDuration;

        _supernovaProgress = 0f;
        SupernovaProgressChanged?.Invoke(_supernovaProgress);
    }
}

public enum SpecialAbility {
    None,
    Star,
    Blast
}