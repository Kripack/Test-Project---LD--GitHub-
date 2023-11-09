using UnityEngine;
using static Vibration;

public class VibrationEvent : MonoBehaviour
{
    public VibroType _type;

    public void Vibrate()
    {
        Vibration.Vibrate(_type);
    }

}
