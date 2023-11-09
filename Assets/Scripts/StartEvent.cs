using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartEvent : MonoBehaviour
{

    [SerializeField] private UnityEvent awakeEvent;
    [SerializeField] private UnityEvent startEvent;

    private void Awake()
    {
        awakeEvent.Invoke();
    }

    void Start()
    {
        startEvent.Invoke();
    }

    

}
