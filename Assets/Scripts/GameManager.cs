using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static bool blastRuneCollected = false;
    public static bool smashRuneCollected = false;
    public static bool barrierRuneCollected = false;

    // 0 = preload, 1 = main menu, 2 = ruins, 3 = hills, 4 = outpost
    private static int currentScene;

    private static int totalArtifactsCollected = 0;
    private static int totalManaCrystalsCollected = 0;

    // 40% artifacts + 30% mana crystals + 30% runes
    private static float gamePercentComplete = 0f;

    private static Hashtable perLevelArtifactsCollected = new Hashtable();
    private static Hashtable perLevelManaCrystalsCollected = new Hashtable();
    private static Hashtable perLevelPercentComplete = new Hashtable();

    void Awake() {
        DontDestroyOnLoad(gameObject);

        perLevelArtifactsCollected.Add("Ruins", 0);     // total = 3
        perLevelArtifactsCollected.Add("Hills", 0);     // total = 3
        perLevelArtifactsCollected.Add("Outpost", 0);   // total = 3

        perLevelManaCrystalsCollected.Add("Ruins", 0);      // total = 200
        perLevelManaCrystalsCollected.Add("Hills", 0);      // total = 250
        perLevelManaCrystalsCollected.Add("Outpost", 0);    // total = 250

        perLevelPercentComplete.Add("Ruins", 0f);   // 65% artifacts + 35% mana crystals
        perLevelPercentComplete.Add("Hills", 0f);   // 60% artifacts + 40% mana crystals
        perLevelPercentComplete.Add("Outpost", 0f); // 60% artifacts + 40% mana crystals
    }

    public static void AddArtifact() {
        totalArtifactsCollected += 1;

        // ruins
        if (currentScene == 2) {
            perLevelArtifactsCollected["Ruins"] = (int)perLevelArtifactsCollected["Ruins"] + 1;
        }
        // hills
        if (currentScene == 3) {
            perLevelArtifactsCollected["Hills"] = (int)perLevelArtifactsCollected["Hills"] + 1;
        }
        // outpost
        if (currentScene == 4) {
            perLevelArtifactsCollected["Outpost"] = (int)perLevelArtifactsCollected["Outpost"] + 1;
        }
    }

    public static void AddManaCrystal() {
        totalManaCrystalsCollected += 1;

        // ruins
        if (currentScene == 2) {
            perLevelManaCrystalsCollected["Ruins"] = (int)perLevelManaCrystalsCollected["Ruins"] + 1;
        }
        // hills
        if (currentScene == 3) {
            perLevelManaCrystalsCollected["Hills"] = (int)perLevelManaCrystalsCollected["Hills"] + 1;
        }
        // outpost
        if (currentScene == 4) {
            perLevelManaCrystalsCollected["Outpost"] = (int)perLevelManaCrystalsCollected["Outpost"] + 1;
        }
    }

    public static void UnlockRune(GameObject rune) {
        if (rune.name == "BlastRune") {
            blastRuneCollected = true;
        }
        if (rune.name == "SmashRune") {
            smashRuneCollected = true;
        }
        if (rune.name == "BarrierRune") {
            barrierRuneCollected = true;
        }
        gamePercentComplete += 10f;
    }
}
