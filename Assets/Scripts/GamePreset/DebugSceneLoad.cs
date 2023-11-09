using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugSceneLoad : MonoBehaviour
{
    private ScenesRoll Roll;

    [SerializeField] private Text SceneSelect;

    [SerializeField] private Text SceneLoad;
    [SerializeField] private Text SceneLoadId;

    [SerializeField] private int no = 0;
    private string sceneName = "";
    private bool PanelState = false;
    
    [SerializeField] private GameObject Panel;

    private void Start()
    {
        Roll = ScenesRoll.Instance;

        ResetSelectedScene();

        ChangeScene(0);
    }

    private void ResetSelectedScene()
    {
        no = Roll.GetActiveScene();
        SceneLoad.text = SceneManager.GetActiveScene().name;
        SceneLoadId.text = (no).ToString();
        SceneSelect.text = SceneLoad.text;
        sceneName = Roll.GetScene(no);
        SceneSelect.text = sceneName;
    }

    public void ChangeScene(int dir)
    {
        no = Mathf.Max(no + dir, 0);
        sceneName = Roll.GetScene(no);
        SceneSelect.text = sceneName;
        Debug.Log("Select Debug-id: " + (no).ToString());
    }

    public void LoadSelectScene()
    {
        ShowHidePanel();
        SceneLoadId.text = (no).ToString();
        SceneLoad.text = SceneSelect.text;
        SceneManager.LoadScene(sceneName);
    }

    public void ShowHidePanel()
    {
        PanelState = !PanelState;
        if (PanelState)
            ResetSelectedScene();
        Panel.SetActive(PanelState);
    }

    public void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        ChangeScene(-10000);
        LoadSelectScene();
    }

    public void SetWin()
    {
        ShowHidePanel();
        StartCoroutine(FindObjectOfType<GeneralSettings>().Win());
    }

    public void SetLose()
    {
        ShowHidePanel();
        StartCoroutine(FindObjectOfType<GeneralSettings>().Lose());
    }

}
