using UnityEngine;

// https://forum.unity.com/threads/shooting-object-in-a-direction-of-another-object.942142/

public class Enemy : Character {

    [Space, Header("AI")]
    public MovementType MovementType = MovementType.None;

    [Space]
    public ProjectileType ProjectileType = ProjectileType.None;
    public float FiringRate = 0.5f;

    private Vector2 _shootLocation;

    private void Update() {
        UpdateMovement();
    }

    protected override void OnDisable() {
        base.OnDisable();
        CancelInvoke();
    }

    private void UpdateMovement() {

        //update moveDirection depending on MovementType
        switch (MovementType) {
            case MovementType.UpDown:
                break;
            case MovementType.Circle:
                break;
            case MovementType.Random:
                break;
            case MovementType.None:
            default:
                break;
        }
    }

    protected override void Shoot() {

        Vector2 dir = new();

        switch (ProjectileType) {
            case ProjectileType.StraightAhead:
                dir = new Vector2(-1, 0);
                _shootLocation = new Vector2(-ProjectileOffset, 0);
                break;

            case ProjectileType.Circle:
                dir = _shootLocation / ProjectileOffset;

                if (dir.x < 0f && dir.y >= 0f) {
                    _shootLocation.y -= ProjectileOffset;
                }
                else if (dir.x > 0f && dir.y <= 0f) {
                    _shootLocation.y += ProjectileOffset;
                }
                else if (dir.x <= 0f && dir.y < 0f) {
                    _shootLocation.x += ProjectileOffset;
                }
                else if (dir.x >= 0f && dir.y > 0f) {
                    _shootLocation.x -= ProjectileOffset;
                }

                dir = (_shootLocation / ProjectileOffset).normalized;
                break;

            case ProjectileType.Targeted:
                dir = (manager.GetPlayerPosition() - transform.position).normalized;
                _shootLocation = new Vector2(dir.x * ProjectileOffset, dir.y * ProjectileOffset);                
                break;

            case ProjectileType.Random:
                _shootLocation = GetRandomShootLocation();
                dir = (_shootLocation / ProjectileOffset).normalized;
                break;

            case ProjectileType.None:
            default:
                break;
        }

        if (ProjectileType != ProjectileType.None) {
            Projectile p = Instantiate(ProjectilePrefab, new Vector2(transform.position.x + _shootLocation.x, transform.position.y + _shootLocation.y), Quaternion.identity).GetComponent<Projectile>();
            p.MoveDirection = dir;
        }
    }

    protected override void Die() {
        Destroy(gameObject);
    }

    private void OnBecameVisible() {
        if (ProjectileType != ProjectileType.None) {
            _shootLocation = GetRandomShootLocation();

            InvokeRepeating(nameof(Shoot), 0f, FiringRate);
        }
    }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }

    private Vector2 GetRandomShootLocation() {

        int rand1 = Random.Range(-1, 2);

        if (rand1 != 0) {
            return new Vector2(ProjectileOffset * rand1, ProjectileOffset * Random.Range(-1, 2));
        }
        else {
            int rand2 = Random.Range(0, 2);

            return new Vector2(0f, rand2 == 0 ? -ProjectileOffset : ProjectileOffset);
        }
    }
}

public enum MovementType {
    None = 0,
    UpDown = 1,
    Circle = 2,
    Random = 3
}

public enum ProjectileType {
    None = 0,
    StraightAhead = 1,
    Circle = 2,
    Targeted = 3,
    Random = 4
}