using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelSelectController : MonoBehaviour {

    public List<Button> LevelButtons = new();

    private void Awake() {
        for (int i = 0; i <= GameManager.Instance.LastLevelWon + 1 && i < LevelButtons.Count; i++) {
            LevelButtons[i].interactable = true;
        }
    }
}