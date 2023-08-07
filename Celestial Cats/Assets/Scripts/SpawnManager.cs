using System.Collections.Generic;
using UnityEngine;

// https://docs.unity3d.com/ScriptReference/Random.Range.html
// https://docs.unity3d.com/ScriptReference/MonoBehaviour.InvokeRepeating.html
// https://docs.unity3d.com/ScriptReference/MonoBehaviour.CancelInvoke.html
// https://youtu.be/Hy6Gxtk0QwY

public class SpawnManager : MonoBehaviour {

    public InGameManager Manager;

    [Space]
    public List<SpawnableObject> SpawnableObjects = new();
    public float SpawnRate = 1.0f;

    private float _totalWeight = 0f;

    private void Awake() {
        Manager.LevelBegan += Manager_LevelBegan;
        Manager.LevelWon += Manager_LevelWon;

        CalculateWeights();
    }   

    private void OnDisable() {
        Manager.LevelBegan -= Manager_LevelBegan;
        Manager.LevelWon -= Manager_LevelWon;
    }

    private void Manager_LevelBegan(Player player) {
        InvokeRepeating(nameof(SpawnTheUniverse), 0f, SpawnRate);
    }

    private void Manager_LevelWon() {
        CancelInvoke();
    }    

    private void CalculateWeights() {
        _totalWeight = 0;

        foreach (SpawnableObject b in SpawnableObjects) {
            _totalWeight += b.SpawnChance;
            b._weight = _totalWeight;
        }
    }

    private void SpawnTheUniverse() {

        float spawnX = Manager.CameraUpperRight.x + 10.0f;
        float spawnY = Random.Range(Manager.CameraBottomRight.y, Manager.CameraUpperRight.y);

        // randomly select universe bit to spawn
        float random = Random.Range(0f, 100f);
        int spawnIndex = 0;

        for (int i = 0; i < SpawnableObjects.Count; i++) {

            if (SpawnableObjects[i]._weight >= random) {

                spawnIndex = i;
                break;
            }
        }

        Instantiate(SpawnableObjects[spawnIndex].Prefab, new Vector2(spawnX, spawnY), Quaternion.identity);
    }
}

[System.Serializable]
public class SpawnableObject {
    public GameObject Prefab;

    [Range(0f, 100f)]
    public float SpawnChance = 100f;

    [HideInInspector]
    public float _weight;
}