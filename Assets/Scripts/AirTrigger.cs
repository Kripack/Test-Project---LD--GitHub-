using NaughtyAttributes;
using UnityEngine;

public class AirTrigger : MonoBehaviour
{

    [BoxGroup("Settings"), SerializeField] private bool FreeControll = true;

    [BoxGroup("Settings"), SerializeField, HideIf("FreeControll"), ReadOnly] private Transform _endPoint;
    [BoxGroup("Settings"), SerializeField, HideIf("FreeControll")] private float _upLength;


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!FreeControll)
        {
            if (!_endPoint)
            {
                if (transform.childCount > 0)
                {
                    _endPoint = transform.GetChild(0);
                }
                else
                {
                    _endPoint = new GameObject("EndPoint").transform;
                    _endPoint.SetParent(transform);
                    _endPoint.localPosition = Vector3.forward;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!FreeControll && _endPoint)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_endPoint.position, 0.3f);
            Gizmos.DrawSphere(transform.position, 0.3f);
            for (int i = 0; i < 100; i++)
            {
                float t_1 = ((float)i / 100f);
                float add_y_1 = Mathf.Sin(t_1 * Mathf.PI) * _upLength;
                Vector3 pos_1 = Vector3.Lerp(transform.position, _endPoint.position, t_1) + new Vector3(0f, add_y_1, 0f);

                float t_2 = ((float)(i + 1) / 100f);
                float add_y_2 = Mathf.Sin(t_2 * Mathf.PI) * _upLength;
                Vector3 pos_2 = Vector3.Lerp(transform.position, _endPoint.position, t_2) + new Vector3(0f, add_y_2, 0f);

                Gizmos.DrawLine(pos_1, pos_2);
            }
        }
        if (FreeControll)
        {
            if (_endPoint)
                DestroyImmediate(_endPoint.gameObject);
        }
    }
#endif

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3) // Layer - Player
        {
            if (FreeControll)
            {
                PlayerController.Controller.InAirTrigger(true);
            }
            else
            {
                PlayerController.Controller.InFixAirTrigger(true, transform.position, _endPoint.position, _upLength);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3) // Layer - Player
        {
            if (FreeControll)
            {
                PlayerController.Controller.InAirTrigger(false);
            }
            else
            {
                PlayerController.Controller.InFixAirTrigger(false, transform.position, _endPoint.position, _upLength);
            }
        }
    }


}
