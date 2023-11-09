using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CollectionObject : MonoBehaviour
{
    [BoxGroup("Settings")][SerializeField] private int PowerCount = 1;
    [BoxGroup("Settings")][SerializeField] private float SizeCount = 0.2f;
    [BoxGroup("Settings")][SerializeField] private int Coins = 1;

    [BoxGroup("Settings")][SerializeField] private bool DisableOutline = false;
    [BoxGroup("Settings")][SerializeField][ShowIf("DisableOutline")] private Outline Outline;

    [BoxGroup("Settings")][SerializeField] private float Scale = 1f;


    public void Collect()
    {
        GeneralSettings.Instance.AddCoin(Coins, transform);
        PlayerController.Controller.AddPowerSize((float)PowerCount, SizeCount);
        PlayerController.Controller.AddOnLayer(transform);
        transform.localScale = transform.localScale * Scale;
        if (DisableOutline)
        {
            Outline.enabled = false;
        }
        Destroy(GetComponent<TriggerEvent>());
        Destroy(GetComponent<Collider>());
        Destroy(this);
    }

}
