using UnityEngine;
using System.Collections;

public class HealthComponent : MonoBehaviour {

    public float MaxHealth = 100f;
    private float _currentHealth = 100f;

    [HideInInspector]
    public bool CanBeDamaged = true;

    private void Awake() {
        _currentHealth = MaxHealth;
    }

    public void Damage(float amount) {
        if (CanBeDamaged) {
            _currentHealth = _currentHealth - amount < 0 ? 0 : _currentHealth - amount;
        }
    }
}