using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCall : MonoBehaviour
{

    public SoundController.SoundIds IdSound;

    public void CallSound()
    {
        SoundController.Instance.PlaySound(IdSound);
    }


}
