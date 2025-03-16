using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Singleton<MainMenu>
{
    private ISaver _saveSystem;

    [SerializeField] private string persistentScene = "PersistentManagers";
    [SerializeField] private GameObject _continueBtn;
    public override void Awake()
    {
        base.Awake();
        if (!IsSceneLoaded(persistentScene))
        {
            SceneManager.LoadScene(persistentScene, LoadSceneMode.Additive);
        }

        _saveSystem = new JsonSaver(false);
        GameData data = _saveSystem.LoadLatest();
        if (data != null)
        {
            _continueBtn.GetComponentInChildren<TMP_Text>().color = new Color32(0, 0, 0, 255);
            _continueBtn.GetComponent<Button>().interactable = true;

        }
        else
        {
            _continueBtn.GetComponentInChildren<TMP_Text>().color = new Color32(130, 130, 130, 255);
            _continueBtn.GetComponent<Button>().interactable = false;
        }
    }
    private bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == sceneName)
            {
                return true;
            }
        }
        return false;
    }
    public void Play()
    {
        //Default scene
        //So motherboard scene(Or I guess we call it HubLevel)
        SceneManager.LoadScene("HubLevel");
    }
    public void Continue()
    {
        //Middle scene to load neccessary GameObjects
        SceneManager.LoadScene("PersistManagersScene");
    }
    public void Options()
    {
       //Not implemented
    }
    public void Exit()
    {
        //Only works in build mode, not edit mode
        Application.Quit();
    }
}
