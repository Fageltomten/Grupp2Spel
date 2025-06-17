using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Author Vidar Edlund
public class MainMenu : MonoBehaviour
{
    private ISaver saveSystem;

   // [SerializeField] private string persistentScene = "PersistentManagers";
    [SerializeField] private GameObject continueBtn;
    [SerializeField] private GameObject SaveNotificationGameObject;

    private void Awake()
    {
        CanContinue();
    }
    /// <summary>
    /// Try to load latest save. If latest save returns null then you have no saved data.
    /// Which means the continue button should be grayed out and disabled.
    /// </summary>
    private void CanContinue()
    {
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
    
    public void Play()
    {
        //Default scene
        // (HubLevel)

        //Check if there already is a save
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
    /// <summary>
    /// Scene to load when you press play
    /// </summary>
    private void LoadStartScene()
    {
        SceneHandler.Instance.ChangeSceneWithPersistance(Level.Hub);
    }
    public void Continue()
    {
        //Middle scene to load neccessary GameObjects
        SceneHandler.Instance.ChangeToLatestScene();
    }
    public void Exit()
    {
        //Only works in build mode, not edit mode
        Application.Quit();
    }
}
