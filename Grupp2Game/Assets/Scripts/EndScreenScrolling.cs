using TMPro;
using UnityEngine;
//Author Vidar Edlund

public class EndScreenScrolling : MonoBehaviour
{
    [Header("End Screen Config")]
    [SerializeField] private GameObject scrollTextGameobject;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float scrollDistance;

    private void Start()
    {
        RemoveNotNeededGameObjects();
    }

    void Update()
    {
        Scrolling();
    }
    private void Scrolling()
    {
        if(scrollTextGameobject.transform.position.y < scrollDistance)
        {
            scrollTextGameobject.transform.Translate(0, scrollSpeed * Time.deltaTime, 0);
        }
    }

    //This should probably be moved to a "EndScreenManager"
    //or something like that. This class should be for scrolling. Not doing this
    //We don't want the Player, PlayerUI, to follow into the EndScreen so I will remove them
    private void RemoveNotNeededGameObjects()
    {
        GameObject PlayerHud = GameObject.Find("PlayerHUDCanvas");
        GameObject PlayerGameObject = GameObject.Find("Player");
        GameObject PlayerMainCamera = GameObject.Find("Main Camera");

        Destroy(PlayerHud);
        Destroy(PlayerGameObject);
        Destroy(PlayerMainCamera);
    }
}
