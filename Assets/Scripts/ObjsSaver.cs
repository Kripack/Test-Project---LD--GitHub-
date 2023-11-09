using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjsSaver : MonoBehaviour
{
    private static bool Instance = false;
    private Transform Parent = null;
    private GameObject SaveObj = null;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = true;
            Parent = transform.GetChild(0);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Save(GameObject obj)
    {
        if (SaveObj != null)
            Destroy(SaveObj);

        SaveObj = Instantiate(obj, Parent);
    }

    public GameObject Load()
    {
        return SaveObj;
    }
}
