using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomToggle : MonoBehaviour
{

    [SerializeField] private bool isOn = false;
    [SerializeField] private GameObject OnImage;
    [SerializeField] private GameObject OffImage;
    [SerializeField] private UnityEvent _update;
    
    public void SetValue(bool value, bool updating = true)
    {
        isOn = value;
        OnImage.SetActive(value);
        OffImage.SetActive(!value);
        if (updating)
            _update.Invoke();
    }

    public bool GetValue()
    {
        return isOn;
    }

    public void ChangeValue()
    {
        SetValue(!isOn);
    }

}
