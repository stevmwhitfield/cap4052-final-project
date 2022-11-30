using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour {

    #region Constants
    private const int ARTIFACTS_PER_LEVEL = 3;
    private const int CRYSTALS_PER_LEVEL = 200;
    #endregion

    #region Fields
    private enum LevelType {
        Hub, Domain
    }
    [SerializeField] private LevelType type;
    [SerializeField] private string levelName;

    private AudioSource audioSource;
    [SerializeField] private AudioClip bgm;

    [SerializeField] private Vector3 playerSpawnLocation;
    private List<Vector3> enemySpawnLocations;  // might delete
    private List<Vector3> itemSpawnLocations;   // might delete

    public float CompletionProgress { get; set; } = 0f;

    private int artifactsCollected = 0;
    private int crystalsCollected = 0;
    private int isRuneActivated = 0;
    #endregion

    #region UnityMethods
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) {
            throw new System.Exception("Error! " + name + ": missing AudioSource.");
        }
    }
    #endregion

    #region Methods
    private void UpdateLevelProgress() {
        // 60% artifacts + 40% crystals
        if (levelName == "Ruins") {
            CompletionProgress = 0.6f * (artifactsCollected / ARTIFACTS_PER_LEVEL) + 0.4f * (crystalsCollected / CRYSTALS_PER_LEVEL);
        }
        // 40% artifacts + 30% crystals + 30% runes
        else {
            CompletionProgress = 0.4f * (artifactsCollected / ARTIFACTS_PER_LEVEL) + 0.3f * (crystalsCollected / CRYSTALS_PER_LEVEL) + 0.3f * isRuneActivated;
        }
        GameManager.UpdateGameProgress();
    }

    public void UpdateArtifacts() {
        artifactsCollected += 1;
        UpdateLevelProgress();
    }

    public void UpdateCrystals(int value) {
        crystalsCollected += value;
        UpdateLevelProgress();
    }

    public void ActivateRune() {
        isRuneActivated = 1;
        UpdateLevelProgress();
    }
    #endregion
}
