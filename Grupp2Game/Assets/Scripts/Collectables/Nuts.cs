using System.Linq;
using UnityEngine;


//Author Vidar Edlund
//I will probably combine all different collectible classes(Screw, Nuts, etc...) into one, because I see no reason to have more than one
public class Nuts : MonoBehaviour, ICollectable, ISaveable
{
    [SerializeField] private GameObject collectedParticleSystem;
    private bool isCollected = false;
    public bool IsCollected
    {
        get => isCollected;
        set => isCollected = value;
    }
    //I could probably use GUID here, but to lazy
    [SerializeField] private string id;


    private void Update()
    {
        RotateObject();
    }
    /// <summary>
    /// Rotate object as long as it is not collected
    /// </summary>
    private void RotateObject()
    {
        if (isCollected) return; //We don't wanna do this stuff if we are collected

        var speed = 40;
        var rotation = speed * Time.deltaTime;
        transform.Rotate(0, rotation, rotation);
    }
    /// <summary>
    /// Logic that should happen when collected
    /// Set IsCollected to true
    /// visually remove gameobject
    /// Instantiate particles
    /// </summary>
    public void Collected()
    {
        IsCollected = true;
        gameObject.SetActive(false);
        Instantiate(collectedParticleSystem, transform.position, Quaternion.identity);
    }

    /// <summary>
    /// If a nut with the same ID as this one exists in the loaded data
    /// than that means this object should use that loaded data
    /// </summary>
    /// <param name="data"></param>
    public void LoadData(GameData data)
    {
        bool isInList = data.Collectable.Any(c => c.ID == id);
        Debug.Log(isInList);
        if (isInList)
        {
            Debug.Log($"{id} is in list");
            //Find this specific collectable in the list
            CollectableData GetCollectable = data.Collectable.Find(c => c.ID == id);

            transform.position = GetCollectable.Position;
            isCollected = GetCollectable.IsCollected;
            if(IsCollected)
            {
                gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Check if the nut exists in the game data.
    /// If not then add it to the collectable list and than
    /// Save the Nuts position and if it is collected or not.
    /// </summary>
    /// <param name="data"></param>
    public void SaveData(GameData data)
    {
        bool isInList = data.Collectable.Any(c => c.ID == id);
        if (!isInList)
        {
            data.Collectable.Add(new CollectableData(id, transform.position, isCollected));
        }
        //Find this specific collectable in the list
        CollectableData GetCollectable = data.Collectable.Find(c => c.ID == id);

        //Update its data
        GetCollectable.Position = transform.position;
        GetCollectable.IsCollected = isCollected;
    }
}
