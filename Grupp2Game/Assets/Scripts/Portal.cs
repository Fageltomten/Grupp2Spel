using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private Vector3 spawnPosition;
    private void OnTriggerEnter(Collider other)
    {
        //Check for player, can be whatever script or tag or layer
        if(other.GetComponent<PlayerMovement>())
        {
            //Should add some kind of UI here, like a black screen or something
            //So we don't see the scene stop for a second like we do now



            //Okej s� jag f�rs�kte spara shitet innan man byte scene men jag kunde inte hitta n�got skit som gjorde det.
            //Jag provade olika event inom SceneManager klassen som detta: SceneManager.activeSceneChanged
            //Men det ville inte fungera. S� jag l�gger bara saven h�r, orkar inte l�ngre
            GameObject.FindAnyObjectByType<SaveManager>().SaveGame();
            SceneManager.LoadScene(sceneToLoad);
            other.transform.position = spawnPosition;
        }
    }
}
