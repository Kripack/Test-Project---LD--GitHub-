using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [Layer][SerializeField] private int _playerLayerNum = 5;
    public enum ObjectDoing { None, Destroy };
    [SerializeField] private ObjectDoing _reaction;
    [SerializeField] private UnityEvent _eventEnter;
    [SerializeField] private UnityEvent _eventExit;
    private bool _active = true;

    private void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.layer == _playerLayerNum)
        {
            if (_active)
            {
                _active = false;
                if (_reaction == ObjectDoing.Destroy)
                    Destroy(obj.gameObject);
                _eventEnter.Invoke();
            }
        }
    }
    private void OnTriggerExit(Collider obj)
    {
        if (obj.gameObject.layer == _playerLayerNum)
        {
            if (_active)
            {
                _active = false;
                _eventExit.Invoke();
            }
        }
    }

    public void SetActive(bool state)
    {
        _active = state;
    }
    public void SetSize(Vector3 size)
    {
        if (TryGetComponent<BoxCollider>(out BoxCollider box))
        {
            box.size = size;
        }
    }
    public void SetSize(float radius)
    {
        if (TryGetComponent<SphereCollider>(out SphereCollider sphere))
        {
            sphere.radius = radius;
        }
    }
}
