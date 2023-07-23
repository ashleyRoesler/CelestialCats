using UnityEngine;

// https://docs.unity3d.com/ScriptReference/Object.Instantiate.html

public class InGameManager : MonoBehaviour {

    [Space]
    public GameObject PlayerSpawnPoint;
    public GameObject PlayerPrefab;

    public event System.Action<PlayerController> GameBegan;
    public event System.Action GameEnded;

    private PlayerController _player;

    public void BeginGame() {

        Cursor.visible = false;

        _player = Instantiate(PlayerPrefab, new Vector2(PlayerSpawnPoint.transform.position.x, PlayerSpawnPoint.transform.position.y), Quaternion.identity).GetComponent<PlayerController>();

        GameBegan?.Invoke(_player);
    }

    public void EndGame() {
        GameEnded?.Invoke();
    }
}