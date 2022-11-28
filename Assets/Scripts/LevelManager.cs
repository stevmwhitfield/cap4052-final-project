using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    private AudioSource audioSource;
    [SerializeField] AudioClip mainMenuBgm;
    [SerializeField] AudioClip ruinsBgm;
    [SerializeField] AudioClip hillsBgm;
    [SerializeField] AudioClip outpostBgm;

    private enum Scenes {
        MainMenu, Ruins, Hills, Outpost
    }
    Scenes currentScene;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) {
            throw new System.Exception("Error! " + name + ": missing AudioSource.");
        }

        int sceneId = SceneManager.GetActiveScene().buildIndex;
        if (sceneId == 0) {
            currentScene = Scenes.MainMenu;
        }
        if (sceneId == 1) {
            currentScene = Scenes.Ruins;
        }
        if (sceneId == 2) {
            currentScene = Scenes.Hills;
        }
        if (sceneId == 3) {
            currentScene = Scenes.Outpost;
        }

        InitScene(currentScene);
    }

    private void Update() {

    }

    private void InitScene(Scenes scene) {
        InitItems();
        PlayBgm();
    }

    private void InitItems() {
        GameManager.artifacts.Add(GameManager.blueCrystalPrefab);
        GameManager.artifacts.Add(GameManager.greenCrystalPrefab);
        GameManager.artifacts.Add(GameManager.redCrystalPrefab);
        GameManager.artifacts.Add(GameManager.purpleCrystalPrefab);
    }

    // Play background music based on the active scene
    private void PlayBgm() {
        if (currentScene == Scenes.MainMenu) {
            audioSource.clip = mainMenuBgm;
        }
        if (currentScene == Scenes.Ruins) {
            audioSource.clip = ruinsBgm;
        }
        if (currentScene == Scenes.Hills) {
            audioSource.clip = hillsBgm;
        }
        if (currentScene == Scenes.Outpost) {
            audioSource.clip = outpostBgm;
        }

        audioSource.Play();
        audioSource.loop = true;
    }
}
