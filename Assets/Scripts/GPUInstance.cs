using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUInstance : MonoBehaviour
{

    public enum OperStart { Awake, Start};
    public OperStart FuncType = OperStart.Awake;
    public bool Group = false;


    private void Awake()
    {
        if (FuncType == OperStart.Awake)
            Instance();
    }

    private void Start()
    {
        if (FuncType != OperStart.Start)
            Instance();
    }

    private void Instance()
    {
        if (Group)
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>(true);
            if (renderers.Length > 0)
            {
                for (int i = 0; i < renderers.Length; i++)
                {
                    MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
                    renderers[i].SetPropertyBlock(materialPropertyBlock);
                }
            }
        }
        else
        {
            if (TryGetComponent<Renderer>(out Renderer meshRenderer))
            {
                MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
                meshRenderer.SetPropertyBlock(materialPropertyBlock);
            }
        }
    }
}
