using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Author: Carl �slund
/// <summary>
/// Component that handles logic and navigation for the pause menu.
/// Pauses and unpauses the game
/// </summary>
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
        DisablePlayerMovement();
    }

    public void UnPause()
    {
        isPaused = false;
        Time.timeScale = latestTimeScale;
        settingsMenu.ExitSettingsMenu();
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        EnablePlayerMovement();
       
    }


    void DisablePlayerMovement()
    {
        InputSystem.actions.FindActionMap("Player").Disable();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerLook>().enabled = false;
    }

    void EnablePlayerMovement()
    {
        InputSystem.actions.FindActionMap("Player").Enable();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerLook>().enabled = true;
    }

    public void Respawn()
    {
        /*
        Dictionary<Level, Vector3> spawnPos = SceneHandler.GetStartingPosition;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        player.transform.position = spawnPos[SceneHandler.Instance.CurrentLevel];
        */
        GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneHandler>().ChoosePosition();
        UnPause();
    }

    public void ExitGame()
    {
       Application.Quit();
    }

}
