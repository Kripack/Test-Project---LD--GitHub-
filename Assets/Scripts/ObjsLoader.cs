using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjsLoader : MonoBehaviour
{

    private void Start()
    {
        GameObject loaded = FindObjectOfType<ObjsSaver>().Load();
        if (loaded != null)
        {
            Destroy(transform.GetChild(0).gameObject);
            Instantiate(loaded, transform);
            transform.GetChild(0).gameObject.name = "SaveObjs";
        }
    }

    public void Save()
    {
        FindObjectOfType<ObjsSaver>().Save(transform.GetChild(0).gameObject);
    }

    #region Debug

    public void ReloadScene()
    {
        Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion

}
