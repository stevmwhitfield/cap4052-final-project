using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private TextMeshProUGUI artifactsCollectedText;
    private TextMeshProUGUI crystalsCollectedText;
    private TextMeshProUGUI levelProgressText;

    private GameObject winCanvas;
    private GameObject overlayCanvas;

    public float CompletionProgress { get; set; }

    private int artifactsCollected = 0;
    private int crystalsCollected = 0;
    private int isRuneActivated = 0;
    #endregion

    #region UnityMethods
    private void Awake() {
        overlayCanvas = GameObject.FindGameObjectWithTag("Overlay");

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) {
            throw new System.Exception("Error! " + name + ": missing AudioSource.");
        }

        winCanvas = GameObject.FindGameObjectWithTag("WinCanvas");
        if (winCanvas == null)
            throw new System.Exception("missing winCanvas");


        artifactsCollectedText = GameObject.FindGameObjectWithTag("ArtifactsCollectedText").GetComponent<TextMeshProUGUI>();
        crystalsCollectedText = GameObject.FindGameObjectWithTag("CrystalsCollectedText").GetComponent<TextMeshProUGUI>();
        levelProgressText = GameObject.FindGameObjectWithTag("LevelProgressText").GetComponent<TextMeshProUGUI>();
    }

    private void Start() {
        CompletionProgress = 0f;
        winCanvas.SetActive(false);
    }
    #endregion

    #region Methods
    private void UpdateLevelProgress() {
        CompletionProgress = 0.6f * ((float)artifactsCollected / (float)ARTIFACTS_PER_LEVEL) + 0.4f * ((float)crystalsCollected / (float)CRYSTALS_PER_LEVEL);
        levelProgressText.text = $"{CompletionProgress * 100}%";
        if (CompletionProgress == 1) {
            winCanvas.SetActive(true);
            StartCoroutine(LoadNextLevel());
        }
        GameManager.UpdateGameProgress();
    }

    private IEnumerator LoadNextLevel() {
        yield return new WaitForSeconds(5.0f);
        winCanvas.SetActive(false);
        artifactsCollectedText.text = "Stars: 0/3";
        crystalsCollectedText.text = "Crystals: 0/200";
        levelProgressText.text = "0%";
        if (levelName == "Ruins") {
            LevelManager.LoadLevel(2);
        }
        if (levelName == "Hills") {
            overlayCanvas.SetActive(false);
            SceneManager.LoadScene(0);
        }
    }

    public void UpdateArtifacts() {
        artifactsCollected += 1;
        artifactsCollectedText.text = $"Stars: {artifactsCollected}/3";
        UpdateLevelProgress();
    }

    public void UpdateCrystals(int value) {
        crystalsCollected += value;
        crystalsCollectedText.text = $"Crystals: {crystalsCollected}/200";
        UpdateLevelProgress();
    }

    public void ActivateRune() {
        isRuneActivated = 1;
        UpdateLevelProgress();
    }

    public void ToggleOverlay(bool isActive) {
        overlayCanvas.SetActive(isActive);
    }
    #endregion
}
