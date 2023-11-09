using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FallingObject : MonoBehaviour
{

    [BoxGroup("Trigger")][SerializeField] private TriggerEvent _distanceTrigger;
    [BoxGroup("Trigger")][SerializeField] private float _distanceTriggerring;

    [BoxGroup("Flying")][SerializeField] private Vector3 _angleFly;
    [BoxGroup("Flying")][SerializeField] private float _distanceFly;
    [BoxGroup("Flying")][SerializeField] private float _speedFly;

    [BoxGroup("Grounding")][SerializeField] private float _lerpGrounding;
    [BoxGroup("Grounding")][SerializeField] private GameObject _FXGrounding;
    [BoxGroup("Grounding")][SerializeField] private bool _cameraShake;

    [BoxGroup("Objects")][SerializeField] private TriggerEvent _fallingObj;
    [BoxGroup("Objects")][SerializeField] private GameObject _debugVisible;

    private Vector3 _startScale;

    private void Awake()
    {
        Vector3 startFly = Quaternion.Euler(_angleFly) * transform.up * _distanceFly + transform.position;
        _fallingObj.transform.position = startFly;
        _startScale = _fallingObj.transform.localScale;
        _fallingObj.transform.localScale = Vector3.zero;
        _fallingObj.SetActive(false);
        _debugVisible.SetActive(false);
    }

    private bool _falling = false;
    public void StartFalling()
    {
        if (!_falling)
            _falling = true;
        else
            return;

        StartCoroutine(Falling());
    }
    private IEnumerator Falling()
    {
        _fallingObj.transform.localScale = _startScale;
        float t = 0f;
        float t_all = (_distanceFly / _speedFly) * Time.fixedDeltaTime;
        Vector3 startPos = _fallingObj.transform.position;
        while (t <= t_all)
        {
            float percent = Mathf.Clamp01(t / t_all);
            _fallingObj.transform.position = Vector3.Lerp(startPos, transform.position, percent);
            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        _FXGrounding.SetActive(true);
        _fallingObj.SetActive(true);
        if (_cameraShake)
            StartCoroutine(PlayerController.Controller.CameraShake());

        float nowSpeed = _speedFly;
        Vector3 groundDir = Quaternion.Euler(_angleFly) * -transform.up;
        while (nowSpeed >= 0f)
        {
            _fallingObj.transform.position += groundDir * Time.fixedDeltaTime * nowSpeed;
            nowSpeed = Mathf.Lerp(nowSpeed, -0.1f, Time.fixedDeltaTime * _lerpGrounding);
            yield return new WaitForFixedUpdate();
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isPlaying)
            return;

        _distanceTrigger.SetSize(_distanceTriggerring);
        Vector3 startFly = Quaternion.Euler(_angleFly) * transform.up * _distanceFly + transform.position;
        _fallingObj.transform.position = startFly;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _distanceTriggerring);

        Gizmos.color = Color.red;
        Vector3 startFly = Quaternion.Euler(_angleFly) * transform.up * _distanceFly + transform.position;
        Gizmos.DrawLine(transform.position, startFly);
    }
    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 pos = transform.position;
        if (_fallingObj && _fallingObj.TryGetComponent<MeshFilter>(out MeshFilter filter))
        {
            Mesh mesh = filter.sharedMesh;
            Gizmos.DrawWireMesh(mesh, pos, _fallingObj.transform.rotation, _fallingObj.transform.localScale);
        }
        else
        {
            Gizmos.DrawWireSphere(pos, 1f);
        }
    }*/
#endif

}
