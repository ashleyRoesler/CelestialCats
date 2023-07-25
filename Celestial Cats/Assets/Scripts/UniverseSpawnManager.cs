using System.Collections.Generic;
using UnityEngine;

// https://docs.unity3d.com/ScriptReference/Random.Range.html
// https://docs.unity3d.com/ScriptReference/MonoBehaviour.InvokeRepeating.html
// https://docs.unity3d.com/ScriptReference/MonoBehaviour.CancelInvoke.html
// https://youtu.be/Hy6Gxtk0QwY

public class UniverseSpawnManager : MonoBehaviour {

    public InGameManager Manager;

    [Space]
    public List<UniverseBit> UniverseBits = new();
    public float UniverseSpawnRate = 1.0f;

    private float _weightOfTheUniverse = 0f;

    private void Awake() {
        Manager.GameBegan += Manager_GameBegan;
        Manager.GameEnded += Manager_GameEnded;

        CalculateWeights();
    }   

    private void OnDisable() {
        Manager.GameBegan -= Manager_GameBegan;
        Manager.GameEnded -= Manager_GameEnded;
    }

    private void Manager_GameBegan(Player player) {
        InvokeRepeating(nameof(SpawnTheUniverse), 0.0f, UniverseSpawnRate);
    }

    private void Manager_GameEnded() {
        CancelInvoke();
    }    

    private void CalculateWeights() {
        _weightOfTheUniverse = 0;

        foreach (UniverseBit b in UniverseBits) {
            _weightOfTheUniverse += b.SpawnChance;
            b._weight = _weightOfTheUniverse;
        }
    }

    private void SpawnTheUniverse() {

        float spawnX = Manager.CameraUpperRight.x + 10.0f;
        float spawnY = Random.Range(Manager.CameraBottomRight.y, Manager.CameraUpperRight.y);

        // randomly select universe bit to spawn
        float random = Random.Range(0f, 100f);
        int spawnIndex = 0;

        for (int i = 0; i < UniverseBits.Count; i++) {

            if (UniverseBits[i]._weight >= random) {

                spawnIndex = i;
                break;
            }
        }

        Instantiate(UniverseBits[spawnIndex].Prefab, new Vector2(spawnX, spawnY), Quaternion.identity);
    }
}

[System.Serializable]
public class UniverseBit {
    public GameObject Prefab;

    [Range(0f, 100f)]
    public float SpawnChance = 100f;

    [HideInInspector]
    public float _weight;
}