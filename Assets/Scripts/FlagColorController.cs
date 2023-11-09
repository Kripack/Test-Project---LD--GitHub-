using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagColorController : MonoBehaviour
{

    [SerializeField, BoxGroup("Settings")] private Material _trueMat;
    [SerializeField, BoxGroup("Settings")] private Material _falseMat;

    private List<FlagColorObject> _flags = new List<FlagColorObject>();

    public void AddFlag(FlagColorObject obj)
    {
        _flags.Add(obj);
    }

    public void RemoveFlag(FlagColorObject obj)
    {
        _flags.Remove(obj);
    }

    public void UpdateFlags(float nowPower)
    {
        foreach (FlagColorObject obj in _flags)
        {
            obj._renderer.material = (obj._hp.GetHP() < (int)nowPower) ? _trueMat : _falseMat;
        }
    }

}
