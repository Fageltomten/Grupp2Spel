using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private ISaver saveSystem;

   // [SerializeField] private string persistentScene = "PersistentManagers";
    [SerializeField] private GameObject continueBtn;
    [SerializeField] private GameObject SaveNotificationGameObject;

    private void Awake()
    {
        //if (!IsSceneLoaded(persistentScene))
        //{
        //    SceneManager.LoadScene(persistentScene, LoadSceneMode.Additive);
        //}

        saveSystem = new JsonSaver(false);
        GameData data = saveSystem.LoadLatest();
        if (data != null)
        {
            continueBtn.GetComponentInChildren<TMP_Text>().color = new Color32(0, 0, 0, 255);
            continueBtn.GetComponent<Button>().interactable = true;

        }
        else
        {
            continueBtn.GetComponentInChildren<TMP_Text>().color = new Color32(130, 130, 130, 255);
            continueBtn.GetComponent<Button>().interactable = false;
        }
    }
    //private bool IsSceneLoaded(string sceneName)
    //{
    //    for (int i = 0; i < SceneManager.sceneCount; i++)
    //    {
    //        Scene scene = SceneManager.GetSceneAt(i);
    //        if (scene.name == sceneName)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}
    public void Play()
    {
        //Default scene
        //So motherboard scene(Or I guess we call it HubLevel)

        //We should check if there already is a save
        //If there is a save we should ask them if they really wanna 
        //create a new one and delete the old one
        if(saveSystem.FilesExists())
        {
            Debug.Log("File exists - MainMenu");
            
            //Give them a pop up
            //"Are you sure you wanna create a new save?"
            //"This will delete your old saves"
            //"This action is not reversible"
            SaveNotificationGameObject.SetActive(true);
            Button[] buttons = SaveNotificationGameObject.GetComponentsInChildren<Button>();
            foreach (Button btn in buttons)
            {
                if(btn.name == "YesBtn")
                {
                    btn.onClick.AddListener(() => saveSystem.DeleteAllFiles());
                    btn.onClick.AddListener(() => LoadStartScene());
                }
            }
        }
        else
        {
            LoadStartScene();
        }
    }
    private void LoadStartScene()
    {
        //ScenesManager.Instance.LoadScene("HubLevel");
        SceneHandler.Instance.ChangeSceneWithPersistance(Level.Hub);
    }
    public void Continue()
    {
        //Middle scene to load neccessary GameObjects
        ScenesManager.Instance.LoadLatestSavedScene();
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
