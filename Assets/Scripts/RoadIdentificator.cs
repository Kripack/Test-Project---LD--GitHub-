using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadIdentificator : MonoBehaviour
{

    [BoxGroup("Info"), ReadOnly] public float Width;
    [BoxGroup("Info"), ReadOnly] public PathCreation.PathCreator Path;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 pos = Path.path.GetPoint(0);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(pos, new Vector3(Width, 0.15f, 0.15f));
        Gizmos.DrawCube(new Vector3(pos.x - Width * 0.5f, pos.y, pos.z), new Vector3(0.15f, 0.15f, 1f));
        Gizmos.DrawCube(new Vector3(pos.x + Width * 0.5f, pos.y, pos.z), new Vector3(0.15f, 0.15f, 1f));
        pos = Path.path.GetPoint(Path.path.NumPoints - 1);
        Gizmos.DrawCube(pos, new Vector3(Width, 0.15f, 0.15f));
        Gizmos.DrawCube(new Vector3(pos.x - Width * 0.5f, pos.y, pos.z), new Vector3(0.15f, 0.15f, 1f));
        Gizmos.DrawCube(new Vector3(pos.x + Width * 0.5f, pos.y, pos.z), new Vector3(0.15f, 0.15f, 1f));
    }
#endif

}
