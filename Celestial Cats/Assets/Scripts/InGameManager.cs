using UnityEngine;

// https://docs.unity3d.com/ScriptReference/Object.Instantiate.html
// https://docs.unity3d.com/ScriptReference/Camera.ScreenToWorldPoint.html
// https://gamedevbeginner.com/the-right-way-to-pause-the-game-in-unity/#pause_time_scale

/*
 * Screenspace:
 * 
 * 0,1--------------1,1
 * |                  |
 * |                  |
 * |                  |
 * 0,0---------------1,0
 */

public class InGameManager : MonoBehaviour {

    [Space]
    public GameObject PlayerSpawnPoint;
    public GameObject PlayerPrefab;

    [Space]
    public float LevelDuration = 5f;

    [HideInInspector]
    public float CurrentLevelDuration = 0f;

    private bool _gameIsRunning = false;
    public static bool IsPaused = false;

    public event System.Action<Player> LevelBegan;
    public event System.Action LevelWon;
    public event System.Action<bool> PauseToggle;

    private Player _player;

    [HideInInspector]
    public Vector3 CameraUpperRight;

    [HideInInspector]
    public Vector3 CameraBottomRight;

    [HideInInspector]
    public Vector3 CameraBottomLeft;   

    private void Start() {
        CameraUpperRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        CameraBottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
        CameraBottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
    }

    private void OnDisable() {        
        if (_player) {
            _player.Revived -= Player_Revived;
        }
    }

    private void Update() {
        
        if (GameManager.Instance.CanBePaused && Input.GetButtonDown("Pause")) {
            TogglePause();
        }

        // update level duration
        if (_gameIsRunning && CurrentLevelDuration < LevelDuration * 60f) {

            CurrentLevelDuration += Time.deltaTime;

            // end level
            if (CurrentLevelDuration >= LevelDuration * 60f) {
                WinLevel();
            }
        }
    }

    public void BeginLevel() {

        Cursor.visible = false;

        _player = Instantiate(PlayerPrefab, new Vector2(PlayerSpawnPoint.transform.position.x, PlayerSpawnPoint.transform.position.y), Quaternion.identity).GetComponent<Player>();

        _player.Revived += Player_Revived;

        _gameIsRunning = true;
        LevelBegan?.Invoke(_player);
    }

    private void Player_Revived() {
        _player.transform.position = PlayerSpawnPoint.transform.position;
    }

    public void WinLevel() {
        _gameIsRunning = false;

        GameManager.Instance.CurrentLevelWon();

        LevelWon?.Invoke();
    }

    public void TogglePause() {
        IsPaused = !IsPaused;

        Time.timeScale = IsPaused ? 0f : 1f;

        PauseToggle?.Invoke(IsPaused);
    }

    public Vector3 GetPlayerPosition() {
        return _player.transform.position;
    }
}