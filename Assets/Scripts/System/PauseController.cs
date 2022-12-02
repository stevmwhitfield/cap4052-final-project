using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour {

    public static bool IsPaused { get; private set; }

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject settingsScreen;

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
        settingsScreen.SetActive(true);
    }

    public void OnReturnPress() {
        mainScreen.SetActive(true);
        settingsScreen.SetActive(false);
    }

    public void OnMenuMenuPress() {
        SceneManager.LoadScene(0);
    }

    public void OnQuitPress() {
        Application.Quit(0);
    }

    private void ResumeGame() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void PauseGame() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
