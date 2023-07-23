using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour {

    public InGameManager Manager;

    [Space]
    public TextMeshProUGUI PowerupProgressText;

    private PlayerController _player;

    private void Awake() {
        Manager.GameBegan += Manager_GameBegan;

        PowerupProgressText.text = 0.ToString();
    }

    private void OnDisable() {
        Manager.GameBegan -= Manager_GameBegan;
        
        if (_player) {
            _player.PowerupProgressChanged -= Player_PowerupProgressChanged;
        }
    }

    private void Manager_GameBegan(PlayerController player) {
        _player = player;
        _player.PowerupProgressChanged += Player_PowerupProgressChanged;
    }

    private void Player_PowerupProgressChanged(float newProgress) {
        PowerupProgressText.text = newProgress.ToString();
    }
}