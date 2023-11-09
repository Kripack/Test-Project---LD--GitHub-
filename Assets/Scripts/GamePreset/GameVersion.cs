using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVersion : MonoBehaviour
{

    [SerializeField] private TMPro.TextMeshProUGUI text;

    private void Awake()
    {
        text.text = Application.version.ToString();
    }
}
