using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

    public InGameManager Manager;
    private GameManager _gameManager;

    public Slider PlayerHealth;

    [Space]
    public Sprite Star;
    public Sprite Blast;

    [Space]
    public Slider SuperNovaProgress;
    public Image SpecialAbilityIcon;

    [Space]
    public Slider LevelProgress;

    [Space]
    public GameObject WinScreen;
    public Button ContinueButton;
    public Button MainMenuButton;

    [Space]
    public GameObject DeathScreen;
    public Button MainMenuButton_Death;

    [Space]
    public GameObject PauseMenu;
    public Button MainMenuButton_Pause;

    private Player _player;
    private bool _cursorShouldShow = true;

    private void Awake() {
        Manager.LevelBegan += Manager_LevelBegan;
        Manager.LevelWon += Manager_LevelWon;
        Manager.PauseToggle += Manager_PauseToggle;

        _gameManager = FindObjectOfType<GameManager>();

        SpecialAbilityIcon.gameObject.SetActive(false);

        LevelProgress.maxValue = Manager.LevelDuration * 60f;

        ContinueButton.onClick.AddListener(() => {
            _gameManager.PlayNextLevel();
        });

        MainMenuButton.onClick.AddListener(() => {
            _gameManager.GoToMainMenu();
        });

        MainMenuButton_Death.onClick.AddListener(() => {
            _gameManager.GoToMainMenu();
        });

        MainMenuButton_Pause.onClick.AddListener(() => {
            _gameManager.GoToMainMenu();
        });
    }

    private void Update() {
        LevelProgress.value = Manager.CurrentLevelDuration;
    }

    private void OnDisable() {
        Manager.LevelBegan -= Manager_LevelBegan;
        Manager.LevelWon -= Manager_LevelWon;
        Manager.PauseToggle -= Manager_PauseToggle;

        if (_player) {
            _player.SupernovaProgressChanged -= Player_PowerupProgressChanged;
            _player.SpecialAbilityChanged -= Player_SpecialAbilityChanged;
            _player.Died -= Player_Died;
        }
    }

    private void Manager_LevelBegan(Player player) {
        _player = player;
        _player.SupernovaProgressChanged += Player_PowerupProgressChanged;
        _player.SpecialAbilityChanged += Player_SpecialAbilityChanged;
        _player.Health.HealthChanged += Player_HealthChanged;
        _player.Died += Player_Died;

        _cursorShouldShow = false;
    }    

    private void Manager_LevelWon() {

        ContinueButton.gameObject.SetActive(_gameManager.HasNextLevel());

        WinScreen.SetActive(true);
        Cursor.visible = true;

        _cursorShouldShow = true;
    }

    private void Manager_PauseToggle(bool isPaused) {
        PauseMenu.SetActive(isPaused);
        Cursor.visible = isPaused || _cursorShouldShow;
    }

    public void Resume() {
        Manager.TogglePause();
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

    private void Player_HealthChanged(float newHealth) {
        PlayerHealth.value = newHealth / _player.Health.MaxHealth;
    }

    private void Player_Died() {
        Cursor.visible = true;
        DeathScreen.SetActive(true);

        _cursorShouldShow = true;
    }

    public void RevivePlayer() {
        _player.Revive();
        Cursor.visible = false;
        DeathScreen.SetActive(false);

        _cursorShouldShow = false;
    }
}