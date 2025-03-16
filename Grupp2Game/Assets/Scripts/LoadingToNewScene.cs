using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingToNewScene : MonoBehaviour
{
    private ISaver _saveSystem;

    private void Awake()
    {
        _saveSystem = new JsonSaver(false);
        StartCoroutine(LoadingSavedScene());
    }
    private IEnumerator LoadingSavedScene()
    {
        float waitTime = 2f;//Why do I do this? Because I just want to see the middle screen
        yield return new WaitForSeconds(waitTime);

        GameData data = _saveSystem.LoadLatest();
        if (data != null)
        {
            SceneManager.LoadScene(data.ActiveScene);
        }
    }
    //public void LoadingSavedScene()
    //{
    //    GameData data = _saveSystem.LoadLatest();
    //    if (data != null)
    //    {
    //        SceneManager.LoadScene(data.ActiveScene);
    //    }
    //}
}
