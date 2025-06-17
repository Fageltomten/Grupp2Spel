using UnityEngine;
//Author Vidar Edlund
/// <summary>
/// Base Singleton class to make using singleton easier.
/// Just derive from this class and you have a singleton
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindFirstObjectByType(typeof(T));
                if (instance == null)
                {
                    SetupInstance();
                }
            }
            return instance;
        }
    }
    public virtual void Awake()
    {
        RemoveDuplicates();
    }
    /// <summary>
    /// Create a new gameobject with T component and add DontDestroyOnLoad
    /// </summary>
    private static void SetupInstance()
    {
        instance = (T)FindFirstObjectByType(typeof(T));
        if (instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = typeof(T).Name;
            instance = gameObj.AddComponent<T>();
            DontDestroyOnLoad(gameObj);
        }
    }
    /// <summary>
    /// Remove any duplicate gameobjects
    /// </summary>
    private void RemoveDuplicates()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
