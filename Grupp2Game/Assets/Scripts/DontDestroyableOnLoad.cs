using UnityEngine;

//Author Vidar Edlund
public class DontDestroyableOnLoad : MonoBehaviour
{
    //Component that should be attached to objects that should not be destroyed when switching scene
    //Will maybe make this a manager later
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
