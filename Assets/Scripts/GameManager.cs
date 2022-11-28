using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    #region Fields
    public static bool blastRuneCollected = false;
    public static bool barrierRuneCollected = false;

    public static bool isPaused = false;
    public static bool isGameOver = false;
    public static bool isGameWon = false;

    public static List<GameObject> artifacts = new List<GameObject>();
    public static List<GameObject> manaCrystals = new List<GameObject>();
    public static List<GameObject> runes = new List<GameObject>();

    private const int ARTIFACTS_PER_LEVEL = 3;
    private const int MANA_CRYSTALS_PER_LEVEL = 200;
    private const int RUNES_PER_LEVEL = 1;

    // 0 = main menu, 1 = ruins, 2 = hills, 3 = outpost
    private static int currentScene;
    private static int artifactsCollected = 0;
    private static int manaCrystalsCollected = 0;
    private static int runesCollected = 0;

    private static float gamePercentComplete = 0f;

    private static Hashtable perLevelArtifactsCollected = new Hashtable();
    private static Hashtable perLevelManaCrystalsCollected = new Hashtable();
    private static Hashtable perLevelRuneCollected = new Hashtable();
    private static Hashtable perLevelPercentComplete = new Hashtable();

    [SerializeField] public static GameObject artifactPrefab;
    [SerializeField] public static GameObject blueCrystalPrefab;
    [SerializeField] public static GameObject greenCrystalPrefab;
    [SerializeField] public static GameObject redCrystalPrefab;
    [SerializeField] public static GameObject purpleCrystalPrefab;
    [SerializeField] public static GameObject runePrefab;
    #endregion

    #region UnityMethods
    private void Awake() {
        perLevelArtifactsCollected.Add("Ruins", 0);
        perLevelArtifactsCollected.Add("Hills", 0);
        perLevelArtifactsCollected.Add("Outpost", 0);

        perLevelManaCrystalsCollected.Add("Ruins", 0);
        perLevelManaCrystalsCollected.Add("Hills", 0);
        perLevelManaCrystalsCollected.Add("Outpost", 0);

        perLevelRuneCollected.Add("Hills", 0);
        perLevelRuneCollected.Add("Outpost", 0);

        perLevelPercentComplete.Add("Ruins", 0f);
        perLevelPercentComplete.Add("Hills", 0f);
        perLevelPercentComplete.Add("Outpost", 0f);
    }

    private void Start() {
        
    }
    #endregion

    #region PublicMethods
    public static void AddArtifact() {
        string sceneName = "";

        if (currentScene == 1) {
            sceneName = "Ruins";
        }
        if (currentScene == 2) {
            sceneName = "Hills";
        }
        if (currentScene == 3) {
            sceneName = "Outpost";
        }

        perLevelArtifactsCollected[sceneName] = (int)perLevelArtifactsCollected[sceneName] + 1;
        UpdateLevelProgress(sceneName);

        artifactsCollected += 1;
    }

    public static void AddManaCrystal(int value) {
        string sceneName = "";

        if (currentScene == 1) {
            sceneName = "Ruins";
        }
        if (currentScene == 2) {
            sceneName = "Hills";
        }
        if (currentScene == 3) {
            sceneName = "Outpost";
        }

        perLevelManaCrystalsCollected[sceneName] = (int)perLevelManaCrystalsCollected[sceneName] + value;
        //UpdateLevelProgress(sceneName);

        manaCrystalsCollected += value;
        Debug.Log(manaCrystalsCollected);
    }

    public static void UnlockRune(GameObject rune) {
        runesCollected += 1;

        if (rune.name == "BlastRune") {
            blastRuneCollected = true;
            UpdateLevelProgress("Hills");
        }
        if (rune.name == "BarrierRune") {
            barrierRuneCollected = true;
            UpdateLevelProgress("Outpost");
        }
    }
    #endregion

    #region PrivateMethods
    private static void UpdateLevelProgress(string levelName) {
        // 60% artifacts + 40% mana crystals
        if (levelName == "Ruins") {
            perLevelPercentComplete[levelName] = 0.6f * ((int)perLevelArtifactsCollected[levelName] / ARTIFACTS_PER_LEVEL) + 0.4f * ((int)perLevelManaCrystalsCollected[levelName] / MANA_CRYSTALS_PER_LEVEL);
        }
        // 40% artifacts + 30% mana crystals + 30% runes
        else {
            perLevelPercentComplete[levelName] = 0.4f * ((int)perLevelArtifactsCollected[levelName] / ARTIFACTS_PER_LEVEL) + 0.3f * ((int)perLevelManaCrystalsCollected[levelName] / MANA_CRYSTALS_PER_LEVEL) + 0.3f * (int)perLevelRuneCollected[levelName];
        }
        UpdateGameProgress();
    }

    private static void UpdateGameProgress() {
        int totalArtifacts = ARTIFACTS_PER_LEVEL * perLevelArtifactsCollected.Count;
        int totalManaCrystals = MANA_CRYSTALS_PER_LEVEL * perLevelManaCrystalsCollected.Count;
        int totalRunes = RUNES_PER_LEVEL * perLevelRuneCollected.Count;

        // 40% artifacts + 30% mana crystals + 30% runes
        gamePercentComplete = 0.4f * (artifactsCollected / totalArtifacts) + 0.3f * (manaCrystalsCollected / totalManaCrystals) + 0.3f * (runesCollected / totalRunes);
    }

    
    #endregion

}
