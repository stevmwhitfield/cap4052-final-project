using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    #region Fields
    public static List<LevelData> levelData = new List<LevelData>();
    [SerializeField] private LevelData levelDataRuins;
    [SerializeField] private LevelData levelDataHills;
    [SerializeField] private LevelData levelDataOutpost;

    public static LevelData ActiveLevel { get; set; }

    public static Settings playerSettings;

    public static float completionProgress = 0f;

    private static int activeSceneIndex;

    //public static bool isPaused = false;
    //public static bool isBlastUnlocked = false;
    //public static bool isBarrierUnlocked = false;
    #endregion

    #region UnityMethods
    private void Awake() {
        activeSceneIndex = SceneManager.GetActiveScene().buildIndex;

        playerSettings = new Settings();

        levelData.Add(levelDataRuins);
        levelData.Add(levelDataHills);
        levelData.Add(levelDataOutpost);
    }

    private void Start() {
        if (activeSceneIndex > 0) {
            ActiveLevel = levelData[activeSceneIndex - 1];
        }

        playerSettings.sensitivity = 0.3f;
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
