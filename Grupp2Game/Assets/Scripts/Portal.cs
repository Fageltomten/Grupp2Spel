using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private Level sceneToLoad;
    private void OnTriggerEnter(Collider other)
    {
        //Check for player, can be whatever script or tag or layer
        if(other.tag == "Player")
        {
            GameObject sceneManager = GameObject.FindGameObjectWithTag("SceneManager");

            other.GetComponent<PlayerMovement>().ForceResetDash();

            sceneManager.GetComponent<SceneHandler>().ChangeScene(sceneToLoad);
        }
    }
}
