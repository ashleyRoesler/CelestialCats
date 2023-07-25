using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public string MainMenuSceneName;

    [Space]
    public List<string> LevelNames = new();

    private static int _currentLevelIndex = 0;
    public static int LastLevelWon = -1;

    [Space]
    public GameObject LoadingScreenCanvas;
    public Image LoadingScreen;

    [Space]
    public float LoadTime = 1f;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    public void PlayGame(int levelIndex = 0) {
        _currentLevelIndex = levelIndex;
        StartCoroutine(LoadScene_Coroutine(LevelNames[_currentLevelIndex]));
    }

    public bool HasNextLevel() {
        return _currentLevelIndex + 1 < LevelNames.Count;
    }

    public void PlayNextLevel() {
        if (HasNextLevel()) {
            PlayGame(_currentLevelIndex + 1);
        }
    }

    public void GoToMainMenu() {
        StartCoroutine(LoadScene_Coroutine(MainMenuSceneName));
    }

    private IEnumerator LoadScene_Coroutine(string scene) {

        LoadingScreenCanvas.SetActive(true);

        Color c = LoadingScreen.color;

        // fade in loading screen
        for (float alpha = 0f; alpha <= 1f; alpha += 0.2f) {
            c.a = alpha;
            LoadingScreen.color = c;
            yield return new WaitForSeconds(0.02f);
        }

        // load scene
        SceneManager.LoadScene(scene);

        yield return new WaitForSeconds(LoadTime);

        // fade out loading screen
        for (float alpha = 1f; alpha >= 0f; alpha -= 0.2f) {
            c.a = alpha;
            LoadingScreen.color = c;
            yield return new WaitForSeconds(0.02f);
        }

        LoadingScreenCanvas.SetActive(false);
    }

    public void QuitGame() {

#if UNITY_EDITOR
        Debug.Log("Game quit!");
#endif

        Application.Quit();
    }

    public void CurrentLevelWon() {
        if (LastLevelWon < _currentLevelIndex) {
            LastLevelWon = _currentLevelIndex;
        }
    }
}