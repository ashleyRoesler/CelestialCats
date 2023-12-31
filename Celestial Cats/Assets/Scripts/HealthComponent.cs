using UnityEngine;

public class HealthComponent : MonoBehaviour {

    public float MaxHealth = 100f;
    private float _currentHealth = 100f;

    [HideInInspector]
    public bool IsDead = false;

    [HideInInspector]
    public bool CanBeDamaged = true;

    public event System.Action<float> HealthChanged;
    public event System.Action Died;

    private void Awake() {
        _currentHealth = MaxHealth;
    }

    public void TakeDamage(float amount) {
        if (CanBeDamaged && !IsDead) {
            _currentHealth = _currentHealth - amount < 0f ? 0f : _currentHealth - amount;
            IsDead = _currentHealth == 0;

            if (IsDead) {
                Died?.Invoke();
            }

            HealthChanged?.Invoke(_currentHealth);
        }
    }

    public void Revive() {
        _currentHealth = MaxHealth;
        IsDead = false;

        HealthChanged?.Invoke(_currentHealth);
    }
}