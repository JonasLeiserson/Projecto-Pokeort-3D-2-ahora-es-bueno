using UnityEngine;

public class PersistentRoot : MonoBehaviour
{
    public static PersistentRoot Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddToRoot(GameObject go)
    {
        go.transform.SetParent(transform);
    }
}

