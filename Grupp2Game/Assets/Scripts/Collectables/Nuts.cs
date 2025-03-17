using System.Linq;
using UnityEngine;


//Author Vidar Edlund
//I will probably combine all different collectible classes(Screw, Nuts, etc...) into one, because I see no reason to have more than one
public class Nuts : MonoBehaviour, ICollectable, ISaveable
{
    [SerializeField] private GameObject collectedParticleSystem; //Do I need it to be a GameObject or could I just instantiate a particlesystem?
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
        if (isCollected) return; //We don't wanna do this stuff if we are collected

        var speed = 40;
        var rotation = speed * Time.deltaTime;
        transform.Rotate(0, rotation, rotation);
    }
    public void Collected()
    {
        //Play particle system or whatever
        IsCollected = true;
        gameObject.SetActive(false);
        Instantiate(collectedParticleSystem, transform.position, Quaternion.identity);
    }
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
