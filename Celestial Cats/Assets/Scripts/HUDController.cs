using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour {

    public InGameManager Manager;

    [Space]
    public Sprite Star;
    public Sprite Blast;

    [Space]
    public TextMeshProUGUI PowerupProgressText;
    public Image SpecialAbilityIcon;

    private Player _player;

    private void Awake() {
        Manager.GameBegan += Manager_GameBegan;

        PowerupProgressText.text = 0.ToString();
        SpecialAbilityIcon.gameObject.SetActive(false);
    }

    private void OnDisable() {
        Manager.GameBegan -= Manager_GameBegan;
        
        if (_player) {
            _player.PowerupProgressChanged -= Player_PowerupProgressChanged;
            _player.SpecialAbilityChanged -= Player_SpecialAbilityChanged;
        }
    }

    private void Manager_GameBegan(Player player) {
        _player = player;
        _player.PowerupProgressChanged += Player_PowerupProgressChanged;
        _player.SpecialAbilityChanged += Player_SpecialAbilityChanged;
    }

    private void Player_PowerupProgressChanged(float newProgress) {
        PowerupProgressText.text = newProgress.ToString();
    }

    private void Player_SpecialAbilityChanged(SpecialAbility newSpecialAbility) {

        switch (newSpecialAbility) {
            case SpecialAbility.None:
                SpecialAbilityIcon.gameObject.SetActive(false);
                break;
            case SpecialAbility.Star:
                SpecialAbilityIcon.sprite = Star;
                SpecialAbilityIcon.gameObject.SetActive(true);
                break;
            case SpecialAbility.Blast:
                SpecialAbilityIcon.sprite = Blast;
                SpecialAbilityIcon.gameObject.SetActive(true);
                break;
            default:
                SpecialAbilityIcon.gameObject.SetActive(false);
                break;
        }
    }
}