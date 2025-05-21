using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

// Author Anton Sundell
public class PlatformSpawn : MonoBehaviour
{

    [SerializeField] private GameObject platform;
    [SerializeField] private float spawnDelay;

    private bool isRespawning = false;


    void Update()
    {
        if (platform != null && !platform.activeInHierarchy && !isRespawning)
        {
            StartCoroutine(RespawnPlatform());
        }
    }
    private IEnumerator RespawnPlatform()
    {
        isRespawning = true;
        yield return new WaitForSeconds(spawnDelay);

        if (platform != null)
        {
            platform.SetActive(true);
        }
        isRespawning = false;
    }
}
