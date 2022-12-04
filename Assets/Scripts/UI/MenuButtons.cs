using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {

    #region Fields
    [SerializeField] private GameObject defaultScreen;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject creditsScreen;

    #endregion

    #region MenuMethods
    public void OnPlay() {
        LevelManager.LoadLevel(1);
    }

    public void OnReturn() {
        SetActiveScreen(0);
    }

    public void OnSettings() {
        SetActiveScreen(1);
    }

    public void OnCredits() {
        SetActiveScreen(2);
    }

    public void OnQuit() {
        Application.Quit(0);
    }

    private void SetActiveScreen(int screenIndex) {
        if (screenIndex == 0) {
            defaultScreen.SetActive(true);
            settingsScreen.SetActive(false);
            creditsScreen.SetActive(false);
        }
        else if (screenIndex == 1) {
            defaultScreen.SetActive(false);
            settingsScreen.SetActive(true);
            creditsScreen.SetActive(false);

        }
        else if (screenIndex == 2) {
            defaultScreen.SetActive(false);
            settingsScreen.SetActive(false);
            creditsScreen.SetActive(true);
        }
        else {
            Debug.LogWarning("Warning! screenIndex: " + screenIndex + " is not a valid index.");
        }
    }
    #endregion

}
