using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    public static void LoadLevel(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }
}
