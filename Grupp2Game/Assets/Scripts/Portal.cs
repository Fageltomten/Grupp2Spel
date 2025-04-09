using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private Level sceneToLoad;
    [SerializeField] private Vector3 spawnPosition; //Är kvar eftersom att all information inte har lagts in i ChoosePosition()
    private void OnTriggerEnter(Collider other)
    {
        //Check for player, can be whatever script or tag or layer
        if(other.GetComponent<PlayerMovement>())
        {
            GameObject sceneManager = GameObject.FindGameObjectWithTag("SceneManager");
            sceneManager.GetComponent<SceneHandler>().ChangeScene(sceneToLoad);
        }
    }
}
