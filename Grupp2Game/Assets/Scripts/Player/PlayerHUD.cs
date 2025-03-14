using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Author Vidar Edlund
public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text collectedCollectablesText;
    [SerializeField] private TMP_Text timerScore;


    [SerializeField] private Image crosshair; //Change color and stuff when interacting?

    public void Awake()
    {
        //Call LevelManager to update collect score
    }

    public void UpdateHud()
    {
        collectedCollectablesText.text = "Whatever";
    }

}
