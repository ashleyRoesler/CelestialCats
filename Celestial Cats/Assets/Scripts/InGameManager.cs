using UnityEngine;

// https://docs.unity3d.com/ScriptReference/Object.Instantiate.html
// https://docs.unity3d.com/ScriptReference/Camera.ScreenToWorldPoint.html

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

    public event System.Action<Player> GameBegan;
    public event System.Action GameEnded;

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

    private void Update() {
        
        // update level duration
        if (_gameIsRunning && CurrentLevelDuration < LevelDuration * 60f) {

            CurrentLevelDuration += Time.deltaTime;

            // end level
            if (CurrentLevelDuration >= LevelDuration * 60f) {

            }
        }
    }

    public void BeginGame() {

        Cursor.visible = false;

        _player = Instantiate(PlayerPrefab, new Vector2(PlayerSpawnPoint.transform.position.x, PlayerSpawnPoint.transform.position.y), Quaternion.identity).GetComponent<Player>();

        _gameIsRunning = true;
        GameBegan?.Invoke(_player);
    }

    public void EndGame() {
        _gameIsRunning = false;
        GameEnded?.Invoke();
    }
}