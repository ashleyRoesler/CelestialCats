using UnityEngine;

// https://docs.unity3d.com/ScriptReference/Object.Instantiate.html

public class InGameManager : MonoBehaviour {

    public GameObject PlayerSpawnPoint;
    public GameObject PlayerPrefab;

    public void BeginGame() {

        Instantiate(PlayerPrefab, new Vector2(PlayerSpawnPoint.transform.position.x, PlayerSpawnPoint.transform.position.y), Quaternion.identity);
    }
}