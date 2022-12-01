using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    public static void LoadLevel(int sceneIndex) {
        GameManager.ActiveLevel = GameManager.levelData[sceneIndex - 1];
        SceneManager.LoadScene(sceneIndex);
    }
}
