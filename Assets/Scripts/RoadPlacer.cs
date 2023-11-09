using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using NaughtyAttributes;
using System.Linq;

public class RoadPlacer : MonoBehaviour
{
    public enum PlaceType { Manual, Auto};
    [Header("Placer settings")]
    public PlaceType _placeType;

    // Manual
    [ShowIf(EConditionOperator.And, "ItManual")] public Vector3 _rotateOffset;
    [ShowIf(EConditionOperator.And, "ItManual")] public Vector3 _rayPointOffset;
    [ShowIf(EConditionOperator.And, "ItManual")] public float _rayLength;
    [ShowIf(EConditionOperator.And, "ItManual")] public LayerMask _layers;

    // Auto
    [ShowIf(EConditionOperator.And, "ItAuto")] public float _distance;
    [ShowIf(EConditionOperator.And, "ItAuto")] public Vector3 _posOffset;
    [ShowIf(EConditionOperator.And, "ItAuto")] public Vector3 _rotOffset;
    [ShowIf(EConditionOperator.And, "ItAuto", "ItNoChild")][Dropdown("_roadsName")] public PathCreator _pathCreator;

    [SerializeField] private bool RandomYRotation = false;
    private List<PathCreator> _roadsName { get { return FindObjectsOfType<PathCreator>().ToList(); } }

    private bool ItChild()
    {
        bool hasParetn = transform.parent != null;
        return hasParetn && transform.parent.GetComponent<RoadPlacer>() != null;
    }
    private bool ItNoChild()
    {
        return !ItChild();
    }
    private RoadPlacer GetParent()
    {
        bool hasParetn = transform.parent != null;
        if (hasParetn)
        {
            return transform.parent.GetComponent<RoadPlacer>();
        }
        else
        {
            return null;
        }
    }
    private bool ItManual()
    {
        return _placeType == PlaceType.Manual;
    }
    private bool ItAuto()
    {
        return _placeType == PlaceType.Auto;
    }

    private void Awake()
    {
        Destroy(this);
    }

    private void OnDrawGizmosSelected()
    {
        if (_placeType == PlaceType.Auto)
        {

            if (ItNoChild())
            {
                if (!_pathCreator)
                    return;

                Vector3 posOnDistance = _pathCreator.path.GetPointAtDistance(_distance);
                Vector3 norOnDistance = _pathCreator.path.GetNormalAtDistance(_distance);
                Vector3 dirOnDistance = _pathCreator.path.GetDirectionAtDistance(_distance);

                Vector3 normal = Quaternion.Euler(0f, 0f, 90f) * norOnDistance;
                float rotateAngle = Vector3.Angle(Vector3.up, normal);
                Vector3 rotationAxis = Vector3.Cross(Vector3.up, normal).normalized;
                Quaternion rotation = Quaternion.AngleAxis(rotateAngle, rotationAxis);
                transform.position = rotation * _posOffset + posOnDistance;

                float rotX = Quaternion.LookRotation(dirOnDistance).eulerAngles.x;
                Vector3 rotYZ = Quaternion.FromToRotation(Vector3.up, norOnDistance).eulerAngles;
                transform.rotation = Quaternion.Euler(rotX, rotYZ.y, rotYZ.z);
                transform.Rotate(_rotOffset, Space.Self);
            }
            else
            {
                RoadPlacer parent = GetParent();
                _pathCreator = parent._pathCreator;

                Vector3 posOnDistance = _pathCreator.path.GetPointAtDistance(parent._distance + _distance);
                Vector3 norOnDistance = _pathCreator.path.GetNormalAtDistance(parent._distance + _distance);
                Vector3 dirOnDistance = _pathCreator.path.GetDirectionAtDistance(parent._distance + _distance);

                Vector3 normal = Quaternion.Euler(0f, 0f, 90f) * norOnDistance;
                float rotateAngle = Vector3.Angle(Vector3.up, normal);
                Vector3 rotationAxis = Vector3.Cross(Vector3.up, normal).normalized;
                Quaternion rotation = Quaternion.AngleAxis(rotateAngle, rotationAxis);
                transform.position = rotation * (parent._posOffset + _posOffset) + posOnDistance;

                float rotX = Quaternion.LookRotation(dirOnDistance).eulerAngles.x;
                Vector3 rotYZ = Quaternion.FromToRotation(Vector3.up, norOnDistance).eulerAngles;
                transform.rotation = Quaternion.Euler(rotX, rotYZ.y, rotYZ.z);
                transform.Rotate(_rotOffset, Space.Self);
            }
        }
        else
        {
            if (Physics.Raycast(transform.position + _rayPointOffset, Vector3.down, out RaycastHit hit, _rayLength, _layers))
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position + _rayPointOffset, hit.point);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(hit.point, hit.point + hit.normal * (_rayLength - hit.distance));

                Vector3 rotOffset = hit.normal;
                transform.rotation = Quaternion.FromToRotation(Vector3.up, rotOffset);
                transform.Rotate(_rotateOffset, Space.Self);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position + _rayPointOffset, transform.position + _rayPointOffset + Vector3.down * _rayLength);
            }
        }

    }

}
