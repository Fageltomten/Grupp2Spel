using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMainMenu : MonoBehaviour
{
    private void Start()
    {
        if (!GameObject.Find("Player"))
            SceneManager.LoadScene("MainMenu");
    }
}
