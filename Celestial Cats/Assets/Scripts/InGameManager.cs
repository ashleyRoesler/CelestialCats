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

    public void BeginGame() {

        Cursor.visible = false;

        _player = Instantiate(PlayerPrefab, new Vector2(PlayerSpawnPoint.transform.position.x, PlayerSpawnPoint.transform.position.y), Quaternion.identity).GetComponent<Player>();

        GameBegan?.Invoke(_player);
    }

    public void EndGame() {
        GameEnded?.Invoke();
    }
}