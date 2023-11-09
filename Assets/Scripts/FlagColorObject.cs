using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagColorObject : MonoBehaviour
{

    public HPObject _hp;
    public Renderer _renderer;

    private void OnEnable()
    {
        if (!GeneralSettings.Instance)
        {
            GeneralSettings.Instance = FindObjectOfType<GeneralSettings>();
        }
        GeneralSettings.Instance._flagColorController.AddFlag(this);
    }

    private void OnDisable()
    {
        GeneralSettings.Instance._flagColorController.RemoveFlag(this);
    }

    private void OnDestroy()
    {
        OnDisable();
    }

}
