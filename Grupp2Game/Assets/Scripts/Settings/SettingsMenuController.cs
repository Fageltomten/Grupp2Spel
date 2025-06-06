using UnityEngine;


// Author: Carl �slund
/// <summary>
/// Component that handles the navigation in the settingsmenu 
/// </summary>
public class SettingsMenuController : MonoBehaviour
{

    [SerializeField] GameObject aside;
    [SerializeField] GameObject graphicsScreen;
    [SerializeField] GameObject audioScreen;
    [SerializeField] GameObject gameplayScreen;
    [Tooltip("Menu to return to when exiting settings menu.")]
    [SerializeField] GameObject mainMenu;

    public void EnterSettingsMenu()
    {
        mainMenu.SetActive(false);
        aside.SetActive(true);
        DeactivateScreens();
    }

    public void ExitSettingsMenu() 
    {
        DeactivateScreens();
        aside.SetActive(false);
        mainMenu.SetActive(true);
        SettingsManager.SaveSettings();
    }


    public void GoToGraphics()
    {
        DeactivateScreens();
        graphicsScreen.SetActive(true);

       
    }

    public void GoToAudio()
    {
        DeactivateScreens();
        audioScreen.SetActive(true);
    }

    public void GoToGameplay()
    {
        DeactivateScreens();
        gameplayScreen.SetActive(true);
    }


    private void DeactivateScreens()
    {
        graphicsScreen.SetActive(false);
        audioScreen.SetActive(false);
        gameplayScreen.SetActive(false);
    }

}
