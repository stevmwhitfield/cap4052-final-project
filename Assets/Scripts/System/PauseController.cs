using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour {

    public static bool IsPaused { get; private set; }

    private GameObject pauseMenu;
    private GameObject mainScreen;
    //private GameObject settingsScreen;

    private void Awake() {
        IsPaused = false;
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        if (pauseMenu == null) throw new System.Exception("Pause menu is null");
        mainScreen = GameObject.FindGameObjectWithTag("PauseMenuMain");
        if (mainScreen == null) throw new System.Exception("Pause main screen is null");
        //settingsScreen = GameObject.FindGameObjectWithTag("PauseMenuSettings");
        //if (settingsScreen == null) throw new System.Exception("Pause settings screen is null");
    }

    private void Start()
    {
        pauseMenu.SetActive(false);
        //settingsScreen.SetActive(false);
    }

    public void OnPause(InputAction.CallbackContext c) {
        if (c.phase == InputActionPhase.Started) {
            if (IsPaused) {
                ResumeGame();
            }
            if (!IsPaused) {
                PauseGame();
            }
        }
    }

    public void OnResumePress() {
        ResumeGame();
    }

    public void OnSettingsPress() {
        mainScreen.SetActive(false);
        //settingsScreen.SetActive(true);
    }

    public void OnReturnPress() {
        mainScreen.SetActive(true);
        //settingsScreen.SetActive(false);
    }

    public void OnMenuMenuPress() {
        pauseMenu.SetActive(false);
        SceneManager.LoadScene(0);
    }

    public void OnQuitPress() {
        Application.Quit(0);
    }

    public void ResumeGame() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PauseGame() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
