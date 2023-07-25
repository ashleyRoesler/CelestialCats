using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

    public InGameManager Manager;

    [Space]
    public Sprite Star;
    public Sprite Blast;

    [Space]
    public Slider SuperNovaProgress;
    public Image SpecialAbilityIcon;

    [Space]
    public Slider LevelProgress;

    private Player _player;

    private void Awake() {
        Manager.GameBegan += Manager_GameBegan;

        SpecialAbilityIcon.gameObject.SetActive(false);

        LevelProgress.maxValue = Manager.LevelDuration * 60f;
    }

    private void Update() {
        LevelProgress.value = Manager.CurrentLevelDuration;
    }

    private void OnDisable() {
        Manager.GameBegan -= Manager_GameBegan;
        
        if (_player) {
            _player.SupernovaProgressChanged -= Player_PowerupProgressChanged;
            _player.SpecialAbilityChanged -= Player_SpecialAbilityChanged;
        }
    }

    private void Manager_GameBegan(Player player) {
        _player = player;
        _player.SupernovaProgressChanged += Player_PowerupProgressChanged;
        _player.SpecialAbilityChanged += Player_SpecialAbilityChanged;
    }

    private void Player_PowerupProgressChanged(float newProgress) {
        SuperNovaProgress.value = newProgress;
    }

    private void Player_SpecialAbilityChanged(SpecialAbility newSpecialAbility) {

        switch (newSpecialAbility) {
            case SpecialAbility.Star:
                SpecialAbilityIcon.sprite = Star;
                SpecialAbilityIcon.gameObject.SetActive(true);
                break;
            case SpecialAbility.Blast:
                SpecialAbilityIcon.sprite = Blast;
                SpecialAbilityIcon.gameObject.SetActive(true);
                break;
            case SpecialAbility.None:
            default:
                SpecialAbilityIcon.gameObject.SetActive(false);
                break;
        }
    }
}