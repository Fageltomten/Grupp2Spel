using UnityEngine;


//Author Vidar Edlund
//I will probably combine all different collectible classes(Screw, Nuts, etc...) into one, because I see no reason to have more than one
public class Screw : MonoBehaviour, ICollectable
{
    [SerializeField] private GameObject collectedParticleSystem;
    private bool isCollected = false;
    public bool IsCollected
    {
        get => isCollected;
        set => isCollected = value;
    }



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
}
