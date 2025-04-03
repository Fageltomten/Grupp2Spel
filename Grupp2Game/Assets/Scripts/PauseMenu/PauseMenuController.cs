using UnityEngine;
using UnityEngine.InputSystem;

// Author: Carl Åslund
public class PauseMenuController : MonoBehaviour
{

    [SerializeField] SettingsMenuController settingsMenu;
    [SerializeField] GameObject pauseMenu;

    bool isPaused = false;

    float latestTimeScale;

    InputAction pauseToggle;

    private void Start()
    {
        pauseToggle = InputSystem.actions.FindAction("PauseToggle");
        pauseToggle.performed += _ => OnPauseToggle();
    }

    void OnPauseToggle()
    {
        if (isPaused)
        {
            UnPause();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        latestTimeScale = Time.timeScale;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public void UnPause()
    {
        isPaused = false;
        Time.timeScale = latestTimeScale;
        settingsMenu.ExitSettingsMenu();
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void Respawn()
    {
        // add respawn logic
    }

    public void ExitGame()
    {
       // add exit logic
    }

}
