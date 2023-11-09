using BoingKit;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class BoingAngleReader : MonoBehaviour
{

    [SerializeField] private float _maxAngle = 15f;
    [SerializeField] private bool _onceUse = true;
    [SerializeField] private UnityEvent _borderEvent;
    [SerializeField, ReadOnly] private float _nowAngle = 0f;

    private Vector3 _startUp;
    private BoingReactor _reactor;
    private bool _once = true;

    private void Awake()
    {
        _startUp = transform.up;
        _reactor = GetComponent<BoingReactor>();
    }

    private void LateUpdate()
    {
        Vector3 nowUp = _reactor.RenderRotationWs * transform.up;
        _nowAngle = Vector3.Angle(nowUp, _startUp);
        if (Mathf.Abs(_nowAngle) >= _maxAngle && (!_onceUse || (_onceUse && _once)))
        {
            _once = false;
            _borderEvent.Invoke();
        }
    }

}
