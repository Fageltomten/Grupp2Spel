using UnityEngine;
using UnityEngine.SceneManagement;
/* Author: Anton Andersson */

public class Portal : MonoBehaviour
{
    [SerializeField] private Level sceneToLoad;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        GameObject sceneManager = GameObject.FindGameObjectWithTag("SceneManager");

        other.GetComponent<PlayerMovement>().ForceResetDash(); // Resets Dash, because otherwise dash can get stuck
        if (other.GetComponent<GrapplingHook>().IsGrappled()) // Releases grappling hook, because it doesn't reset if you go through level
            other.GetComponent<GrapplingHook>().ShootRelease();

        sceneManager.GetComponent<SceneHandler>().ChangeScene(sceneToLoad);
    }
}
