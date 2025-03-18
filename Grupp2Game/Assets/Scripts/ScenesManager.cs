using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : Singleton<ScenesManager>
{
    //This should probably be somewhere else
    //Probably load it in from the GameData that we have
    //To save data as json
    private Dictionary<string, Vector3> PlayerSceneSpawnPosition;


    private ISaver _saveSystem;
    public override void Awake()
    {
        base.Awake();
        _saveSystem = new JsonSaver(false);

        PlayerSceneSpawnPosition = new Dictionary<string, Vector3>()
        {
            ["HubLevel"] = new Vector3(0, 1, 0),
            ["HarddriveLevel"] = new Vector3(0, 2, -15),
            ["CPULevel"] = new Vector3(0, 2, 0),
            ["GPULevel"] = new Vector3(0, 1, 0),
            ["PowerSupplyLevel"] = new Vector3(0, 1, 0)
        };
    }
    public void LoadScene(string sceneToLoad)
    {
        //This scene gives use the gameobjects we need to play the game
        //Player, PlayerHUD, Music, etc...
        SceneManager.LoadScene("PersistManagersScene", LoadSceneMode.Additive);


        StartCoroutine(LoadingScene(sceneToLoad));
    }
    private IEnumerator LoadingScene(string sceneToLoad)
    {
        //Why do I do this? Because I just want to see the middle screen
        float waitTime = 1f;
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(sceneToLoad);
        //Wait for scene to load before setting position
        //Is this really the way to do it?
        //I know there is an event that goes of when a new scene is loaded
        //maybe that is a better way of doing it?
        yield return null;

        SetPlayerStartPosition(sceneToLoad);
    }


    public void LoadLatestSavedScene()
    {
        //This scene gives use the gameobjects we need to play the game
        //Player, PlayerHUD, Music, etc...
        SceneManager.LoadScene("PersistManagersScene", LoadSceneMode.Additive);


        StartCoroutine(LoadingSavedScene());
    }
    private IEnumerator LoadingSavedScene()
    {
        //Why do I do this? Because I just want to see the middle screen
        float waitTime = 1f;
        yield return new WaitForSeconds(waitTime);

        GameData data = _saveSystem.LoadLatest();
        if (data != null)
        {
            SceneManager.LoadScene(data.ActiveScene);

            yield return null;

            SetPlayerStartPosition(data.ActiveScene);
        }
    }

    private void SetPlayerStartPosition(string sceneToLoad)
    {
        //Maybe I should do some check to see if the value exists?
        //Maybe, but it would only crash if we/I wrote the wrong value
        if(PlayerSceneSpawnPosition.TryGetValue(sceneToLoad, out Vector3 startPos))
        {
            GameObject.FindAnyObjectByType<PlayerMovement>().transform.position = startPos;
        }
        else
        {
            Debug.LogError($"Could not find the specific scene name(key) in the 'PlayerSceneSpawnPosition' dictionary" +
                $" when trying to get the start position for the player in the specific scene. Did you mean to load another scene or did you make a typo?");
            Debug.LogError($"Error occured with the scene: {sceneToLoad}");
        }
    }
}
