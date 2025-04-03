using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

// Author Anton Sundell
public class PlatformSpawn : MonoBehaviour
{

    [SerializeField] private GameObject platform;
    [SerializeField] private float spawnDelay;


    void Update()
    {
        if (platform != null && !platform.activeInHierarchy)
        {
            StartCoroutine(RespawnPlatform());
        }
    }
    private IEnumerator RespawnPlatform()
    {
        yield return new WaitForSeconds(spawnDelay);

        if (platform != null)
        {
            platform.SetActive(true);
        }
    }
}
