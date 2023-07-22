using UnityEngine;
using System.Collections.Generic;

// https://docs.unity3d.com/ScriptReference/Object.Instantiate.html
// https://docs.unity3d.com/ScriptReference/Random.Range.html
// https://docs.unity3d.com/ScriptReference/Camera.ScreenToWorldPoint.html
// https://docs.unity3d.com/ScriptReference/MonoBehaviour.InvokeRepeating.html
// https://docs.unity3d.com/ScriptReference/MonoBehaviour.CancelInvoke.html
// https://youtu.be/XyNb41Zpeuo

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
    public List<UniverseBit> UniverseBits = new();
    public float UniverseSpawnRate = 1.0f;

    private Camera _mainCamera;
    private Vector3 _cameraUpperRight;
    private Vector3 _cameraBottomRight;

    private void Start() {
        _mainCamera = Camera.main;

        _cameraUpperRight = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        _cameraBottomRight = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
    }

    public void BeginGame() {

        Cursor.visible = false;

        Instantiate(PlayerPrefab, new Vector2(PlayerSpawnPoint.transform.position.x, PlayerSpawnPoint.transform.position.y), Quaternion.identity);

        InvokeRepeating(nameof(SpawnTheUniverse), 0.0f, UniverseSpawnRate);
    }

    private void SpawnTheUniverse() {

        float spawnX = _cameraUpperRight.x + 10.0f;
        float spawnY = Random.Range(_cameraBottomRight.y, _cameraUpperRight.y);

        // randomly select universe bit to spawn
        float random = Random.Range(0f, 1f);
        float numForAdding = 0f;
        float total = 0f;

        int spawnIndex = 0;

        foreach (UniverseBit b in UniverseBits) {
            total += b.SpawnChance;
        }

        for (int i = 0; i < UniverseBits.Count; i++) {

            if (UniverseBits[i].SpawnChance / total + numForAdding >= random) {
                spawnIndex = i;
                break;
            }
            else {
                numForAdding += UniverseBits[i].SpawnChance / total;
            }
        }

        Instantiate(UniverseBits[spawnIndex].Prefab, new Vector2(spawnX, spawnY), Quaternion.identity);
    }

    public void EndGame() {
        CancelInvoke();
    }
}

[System.Serializable]
public class UniverseBit {
    public GameObject Prefab;
    public float SpawnChance = 100.0f;
}