using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour {

    public static bool IsPaused { get; private set; }

    [SerializeField] private static GameObject pauseMenu;
    [SerializeField] private static GameObject mainScreen;
    [SerializeField] private static GameObject settingsScreen;

    private void Awake() {
        IsPaused = false;
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

    public static void ResumeGame() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void PauseGame() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
