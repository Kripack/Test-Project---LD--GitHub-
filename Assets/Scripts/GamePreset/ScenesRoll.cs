using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class ScenesRoll : MonoBehaviour
{
    public static ScenesRoll Instance = null;

    [Scene] public string[] Tutorial;
    [Scene] public string[] Repeated;

    private int nowId = -1;
    private string lastScene = "LastScene";

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);

            bool isLoading = SceneManager.GetActiveScene().buildIndex == 0;
            if (isLoading)
            {
                Debug.Log("Now scene <Loading>, load save level...");
                nowId = PlayerPrefs.GetInt(lastScene, 0);
            }
            else
            {
                nowId = GetActiveScene();
            }
            Debug.Log("Loaded level no: " + nowId.ToString());
            if (isLoading)
                SceneManager.LoadScene(GetScene(nowId));
        }
    }

    public int GetActiveScene()
    {
        int id = -1;
        string thisScene = SceneManager.GetActiveScene().name;
        for (int i = 0; i < Tutorial.Length; i++)
        {
            if (Tutorial[i] == thisScene)
            {
                id = i;
                break;
            }
        }
        if (id == -1)
            for (int i = 0; i < Repeated.Length; i++)
            {
                if (Repeated[i] == thisScene)
                {
                    id = i + Tutorial.Length;
                    break;
                }
            }
        if (id == -1)
        {
            Debug.LogWarning("This scene non in list ScenesRoll! Add please!");
            id = 0;
        }
        return id;
    }

    public int GetLevelNo()
    {
#if !UNITY_EDITOR
        nowId = PlayerPrefs.GetInt(lastScene, 0);
#endif
        return nowId;
    }

    public void SaveSceneName()
    {
        nowId++;
        PlayerPrefs.SetInt(lastScene, nowId);
    }

    public string GetThisScene()
    {
        return GetScene(nowId);
    }

    public string GetNextScene()
    {
        Debug.Log("New Scene id:" + nowId.ToString());
        return GetScene(nowId);
    }

    public string GetScene(int id)
    {
        // Tutorial
        if (id < Tutorial.Length)
        {
            return Tutorial[id];
        }
        // Repeated
        else
        {
            id -= Tutorial.Length;
            while (id >= Repeated.Length)
                id -= Repeated.Length;
            return Repeated[id];
        }
    }



}
