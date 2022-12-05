using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    #region Fields
    public static List<LevelData> levelData;
    [SerializeField] private LevelData levelDataRuins;
    [SerializeField] private LevelData levelDataHills;
    [SerializeField] private LevelData levelDataOutpost;

    public static LevelData ActiveLevel { get; set; }

    public static Settings playerSettings;

    public static float completionProgress = 0f;

    public static int activeSceneIndex;

    private GameObject overlayCanvas;

    //public static bool isBlastUnlocked = false;
    //public static bool isBarrierUnlocked = false;
    #endregion

    #region UnityMethods
    private void Awake() {
        activeSceneIndex = SceneManager.GetActiveScene().buildIndex;

        overlayCanvas = GameObject.FindGameObjectWithTag("Overlay");

        playerSettings = new Settings();
    }

    private void Start() {
        levelData = new List<LevelData> { levelDataRuins, levelDataHills, levelDataOutpost };

        if (activeSceneIndex > 0) {
            ActiveLevel = levelData[activeSceneIndex - 1];
        }

        playerSettings.sensitivity = 0.3f;

        overlayCanvas.SetActive(false);
    }
    #endregion

    #region PublicMethods
    public static void UpdateGameProgress() {
        float temp = 0f;
        for (int i = 1; i < levelData.Count; i++) {
            temp += levelData[i].CompletionProgress;
        }
        completionProgress = temp;
    }
    #endregion

}
