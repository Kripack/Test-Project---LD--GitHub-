using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using TMPro;
using UnityEditor;
using UnityEngine.UI;
using BoingKit;
using PathCreation;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    [BoxGroup("Input")][SerializeField] private float _sencivity = 3.5f;
    [BoxGroup("Input")][SerializeField] private float _rotSencivity = 3.5f;

    [BoxGroup("Camera")][SerializeField] private float _camSpeed = 3f;
    [BoxGroup("Camera")][SerializeField] private Vector3 _camLerp;
    [BoxGroup("Camera")][SerializeField] private Vector3 _camFinishOffset;
    [BoxGroup("Camera")][SerializeField] private Vector3 _camFinishRot;
    [BoxGroup("Camera")][SerializeField] private AnimationCurve _camLerpMultCurve;
    [BoxGroup("Camera")][SerializeField] private float _camMaxAngle;
    [BoxGroup("Camera")][SerializeField] private float _camMaxFov;
    [BoxGroup("Camera")][SerializeField] private float _camLerpFov;

    [BoxGroup("Power")][SerializeField] private TextMeshPro _sizeText;
    [BoxGroup("Power")][SerializeField] private float _sizeTextScale;
    [BoxGroup("Power")][SerializeField] private float _sizeTextLerp;

    [BoxGroup("Moving")][SerializeField] private float _speed = 5f;
    [BoxGroup("Moving")][SerializeField] private float _gravitAdd = 1.15f;
    [BoxGroup("Moving")][SerializeField] private float _jumpMult = 0.5f;
    [BoxGroup("Moving")][SerializeField] private float _rotatePlayer = 10f;
    [BoxGroup("Moving")][SerializeField] private float _rotateFlySlowness = 0.5f;
    [BoxGroup("Moving")][SerializeField] private AnimationCurve _speedUp;
    [BoxGroup("Moving")][SerializeField] private float _speedUpTime = 1f;
    [BoxGroup("Moving")][SerializeField] private AnimationCurve _speedDown;
    [BoxGroup("Moving")][SerializeField] private AnimationCurve _doodleJumpCurve;
    [BoxGroup("Moving")][SerializeField] private float _doodleJumpMult;
    [BoxGroup("Moving")][SerializeField] private float _doodleJumpTimer;
    [BoxGroup("Moving")][SerializeField] private LayerMask _roadLayer;
    [BoxGroup("Moving")][SerializeField] private LayerMask _roadAllLayer;

    [BoxGroup("Effects")][SerializeField] private ParticleSystem _damageEff;
    [BoxGroup("Effects")][SerializeField] private ParticleSystem _destroyEff;
    [BoxGroup("Effects")][SerializeField] private ParticleSystem _rollingEff;
    [BoxGroup("Effects")][SerializeField] private ParticleSystem _langingEff;
    [BoxGroup("Effects")][SerializeField] private BoingEffector _effector;
    [BoxGroup("Effects")][SerializeField] private bool _langingShake;
    [BoxGroup("Effects")][SerializeField] private bool _damageShake;
    [BoxGroup("Effects")][SerializeField] private float _shakePower;
    [BoxGroup("Effects")][SerializeField] private float _shakeTime;
    private float _initialBoingValue = 0f;


    [Foldout("Debug")][ReadOnly][SerializeField] private RoadIdentificator _road;
    [Foldout("Debug")][ReadOnly][SerializeField] private bool _isTouch = false;
    [Foldout("Debug")][ReadOnly][SerializeField] private bool _isMove = false;
    [Foldout("Debug")][SerializeField] private bool _isControll = true;
    [Foldout("Debug")][ReadOnly][SerializeField] private Camera _camera;
    [Foldout("Debug")][ReadOnly][SerializeField] private Rigidbody _rigid;
    [Foldout("Debug")][ReadOnly][SerializeField] private Transform _rotateObj;
    [Foldout("Debug")][ReadOnly][SerializeField] private bool _isGround;
    //[Foldout("Debug")][ReadOnly][SerializeField] private float _timeInFlying;
    [Foldout("Debug")][ReadOnly][SerializeField] private bool _doodleJumping;
    [Foldout("Debug")][ReadOnly][SerializeField] private float _doodleJumpingValue;
    [Foldout("Debug")][ReadOnly][SerializeField] private Vector3 _camOffset;
    [Foldout("Debug")][ReadOnly][SerializeField] private Vector3 _camRotStart;
    [Foldout("Debug")][ReadOnly][SerializeField] private Vector3 _camTargetPos;
    [Foldout("Debug")][ReadOnly][SerializeField] private float _camLerpMult = 1f;
    [Foldout("Debug")][ReadOnly][SerializeField] private float _camRotValue;
    [Foldout("Debug")][ReadOnly][SerializeField] private float _camRotMult;
    [Foldout("Debug")][ReadOnly][SerializeField] private float _nowSpeed = 0f;
    [Foldout("Debug")][ReadOnly][SerializeField] private float _camX = 0f;
    [Foldout("Debug")][ReadOnly][SerializeField] private float _camStartFov = 0f;
    [Foldout("Debug")][ReadOnly][SerializeField] private Vector3 _targetPos;
    [Foldout("Debug")][ReadOnly][SerializeField] private Vector3 _rigidPosPrev;
    [Foldout("Debug")][ReadOnly][SerializeField] private float _rigidAngle;
    [Foldout("Debug")][ReadOnly][SerializeField] private Vector3 _distanceDir;
    [Foldout("Debug")][ReadOnly][SerializeField] private Vector3 _sizeTextOffset;
    [Foldout("Debug")][ReadOnly][SerializeField] private float _power = 10;
    [Foldout("Debug")][ReadOnly][SerializeField] private float _size = 1f;
    [Foldout("Debug")][ReadOnly][SerializeField] private Vector3 _sizeStartScale;
    [Foldout("Debug")][ReadOnly][SerializeField] private float _powerBonus = 1f;
    [Foldout("Debug")][ReadOnly][SerializeField] private float _speedBonus = 1f;
    [Foldout("Debug")][ReadOnly][SerializeField] private float _distance = 1f;
    [Foldout("Debug")][ReadOnly][SerializeField] private float _maxDistance = 1f;

    [Foldout("Debug")][ReadOnly][SerializeField] private Vector3 _distancePos;
    [Foldout("Debug")][ReadOnly][SerializeField] private PlayerPowerEnemys _powerScript;
    //[Foldout("Debug")][ReadOnly][SerializeField] private bool _isGround;
    [Foldout("Debug")][ReadOnly][SerializeField] private float _borderDistance;
    [Foldout("Debug")][ReadOnly][SerializeField] private Vector3 _touchMousePos;
    [Foldout("Debug")][ReadOnly][SerializeField] private float _touchStartDelta;

    // Air vars
    [Foldout("Debug")][ReadOnly][SerializeField] private bool _inAirTrigger;
    [Foldout("Debug")][ReadOnly][SerializeField] private Vector3 _inAirVector;

    // Fix Air vars
    [Foldout("Debug")][ReadOnly][SerializeField] private bool _inFixAirTrigger;
    [Foldout("Debug")][ReadOnly][SerializeField] private float _inFixAirDistNow;
    [Foldout("Debug")][ReadOnly][SerializeField] private float _inFixAirDistAll;
    [Foldout("Debug")][ReadOnly][SerializeField] private Vector3 _inFixAirStart;
    [Foldout("Debug")][ReadOnly][SerializeField] private Vector3 _inFixAirEnd;
    [Foldout("Debug")][ReadOnly][SerializeField] private float _inFixAirUp;

    [Foldout("Debug")][ReadOnly][SerializeField] private float _rollingEmiss;
    private float _startTime = 0f;
    //[Foldout("Debug")][ReadOnly][SerializeField] private float _touchBorderDelta = 0f;
    private float _lastRoadWidth;

    public static PlayerController Controller;

    private void Awake()
    {
        Controller = this;
        _startTime = Time.time;
    }

    void Start()
    {
        _road = null;
        _camera = Camera.main;
        _camStartFov = _camera.fieldOfView;
        _rigid = GetComponent<Rigidbody>();
        _rigid.useGravity = true;
        _powerScript = GetComponent<PlayerPowerEnemys>();
        _rotateObj = _rigid.transform.GetChild(0);
        _targetPos = _rigid.transform.position;
        _camOffset = _camera.transform.position - _rigid.position;
        _camRotStart = _camera.transform.rotation.eulerAngles;
        _sizeText.transform.SetParent(null);
        Vector3 sizeOffset = _sizeText.transform.position - _rigid.position;
        _rollingEmiss = _rollingEff.emissionRate;
        UpdateSize();
        #region Start player pos
        SphereCollider selectCollider = _powerScript.GetActiveCollider();
        if (Physics.Raycast(_rigid.position + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 150f, _roadLayer))
        {
            _targetPos = hit.point + new Vector3(0f, selectCollider.radius * selectCollider.transform.localScale.y, 0f);
        }
        _rigid.position = _targetPos;
        #endregion

        _sizeText.transform.position = _rigid.position + sizeOffset;
        _sizeStartScale = _sizeText.transform.localScale;
        _sizeTextOffset = _sizeText.transform.position - _rigid.position;
        _sizeTextOffset -= _sizeTextOffset.normalized * (selectCollider.radius * selectCollider.transform.localScale.y);

        _initialBoingValue = _effector.Radius;
        _effector.Radius = _initialBoingValue * selectCollider.radius * selectCollider.transform.localScale.y *2f;
        GeneralSettings.Instance._flagColorController.UpdateFlags(_power);
    }
    void FixedUpdate()
    {
        if (_isMove)
        {
            if (_road && !_inAirTrigger && !_inFixAirTrigger)
            {
                RoadMoving();
            }
            else if (!_inFixAirTrigger)
            {
                AirMoving();
            }
            else
            {
                FixAirMoving();
            }
            BallRotateX();
        }
        CameraPosRot();
        SizeTextSetPos();
        RollingEffect();
        _sizeText.transform.localScale = Vector3.Lerp(_sizeText.transform.localScale, _sizeStartScale, Time.fixedDeltaTime * _sizeTextLerp);
    }

    private void RoadMoving()
    {
        // Target Pos Set X && Z
        _distance = Mathf.Clamp(_distance + Time.fixedDeltaTime * _nowSpeed * _speedBonus, 0f, _road.Path.path.length);
        _distancePos = _road.Path.path.GetPointAtDistance(Mathf.Min(_maxDistance, _distance));
        SphereCollider selectCollider = _powerScript.GetActiveCollider();
        _effector.Radius = _initialBoingValue * selectCollider.radius * selectCollider.transform.localScale.y *2f;
        float worldX = (_distancePos.x - _road.Width * 0.5f) + _borderDistance;
        _targetPos = _distancePos + new Vector3(0f, selectCollider.radius * selectCollider.transform.localScale.y, 0f);
        worldX = Mathf.Clamp(worldX, _distancePos.x - _road.Width * 0.5f, _distancePos.x + _road.Width * 0.5f);
        _targetPos.x = worldX;
        if (_isControll)
        {
            if (_isTouch)
            {
                _borderDistance = _touchStartDelta + _sencivity * 0.001f * (Input.mousePosition.x - _touchMousePos.x);
                worldX = _borderDistance + (_distancePos.x - _road.Width * 0.5f);
                //worldX -= _touchBorderDelta;
                worldX = Mathf.Clamp(worldX, _distancePos.x - _road.Width * 0.5f, _distancePos.x + _road.Width * 0.5f);
                _targetPos.x = worldX;
            }
        }
        else
        {
            _borderDistance = Mathf.Lerp(_borderDistance, _road.Width * 0.5f, Time.fixedDeltaTime * 8f);
        }
        if (Physics.Raycast(_targetPos + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 15f, _roadAllLayer))
        {
            _targetPos = hit.point + new Vector3(0f, selectCollider.radius * selectCollider.transform.localScale.y, 0f);
        }

        Vector3 normal = Quaternion.Euler(0f, 0f, 90f) * _road.Path.path.GetNormalAtDistance(_distance);
        Debug.DrawRay(_targetPos, normal * 10f, Color.green, 0.02f);
        float rotateAngle = Vector3.Angle(Vector3.up, normal);
        Vector3 fromPointToCenter = _targetPos - _distancePos;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, normal).normalized;
        Quaternion rotation = Quaternion.AngleAxis(rotateAngle, rotationAxis);
        _targetPos = rotation * fromPointToCenter + _distancePos;

        // Ball velocity Set
        _distanceDir = (_targetPos - transform.position) * _speed;
        _distanceDir = transform.InverseTransformDirection(_distanceDir);

        _rigid.velocity = Vector3.zero;
        _rigid.angularVelocity = Vector3.zero;

        _isGround = Physics.OverlapSphere(transform.position, selectCollider.radius * selectCollider.transform.localScale.y + 0.65f, _roadLayer).Length > 0 || _doodleJumping; //Physics.OverlapSphere(transform.position, _powerScript.GetRadius() * 1.1f, _roadLayer).Length > 0;
        _distanceDir.y = 0;
        _distanceDir = transform.TransformDirection(_distanceDir);

        if (_doodleJumping)
        {
            _targetPos.y += _doodleJumpingValue;
        }

        _rigid.MovePosition(Vector3.Lerp(_rigid.position, _targetPos, 0.5f));

        _camRotMult = Mathf.Lerp(_camRotMult, 1, Time.fixedDeltaTime * 10f);
        Vector3 camPos = _rigid.position;
        camPos.x = _distancePos.x;
        _camTargetPos = camPos + _camOffset;
    }
    private void AirMoving()
    {
        // Old Controller
        /*_isGround = false;
        float rotOffset = 0f;
        if (_isControll)
        {
            if (_isTouch)
            {
                rotOffset = _rotSencivity * 0.001f * (Input.mousePosition.x - _touchMousePos.x);
                _touchMousePos = Input.mousePosition;
            }
        }
        _camRotValue += rotOffset;
        _inAirVector.y = _rigid.velocity.y;
        _inAirVector = Quaternion.Euler(0f, rotOffset, 0f) * _inAirVector;
        _rigid.velocity = _inAirVector;

        _camRotMult = Mathf.Lerp(_camRotMult, 0f, Time.fixedDeltaTime * _rotateFlySlowness);

        SphereCollider selectCollider = _powerScript.GetActiveCollider();
        Vector3 camRigidOffset = new Vector3(0f, selectCollider.radius * selectCollider.transform.localScale.y, 0f);
        _camTargetPos = _rigid.position - camRigidOffset * 0.1f + Quaternion.Euler(0f, _camRotValue, 0f) * _camOffset;*/


        // New Controller
        _isGround = false;
        // Target Pos Set X && Z
        
        float worldX = (_distancePos.x - _lastRoadWidth * 0.5f) + _borderDistance;
        _targetPos = _rigid.position;// + _rigid.velocity * Time.fixedDeltaTime;
        if (_isControll)
        {
            if (_isTouch)
            {
                _borderDistance = _touchStartDelta + _sencivity * 0.001f * (Input.mousePosition.x - _touchMousePos.x);
                worldX = _borderDistance + (_distancePos.x - _lastRoadWidth * 0.5f);
                _targetPos.x = worldX;
            }
        }
        else
        {
            _borderDistance = Mathf.Lerp(_borderDistance, _lastRoadWidth * 0.5f, Time.fixedDeltaTime * 8f);
        }

        worldX = Mathf.Clamp(worldX, _distancePos.x - _lastRoadWidth * 0.5f, _distancePos.x + _lastRoadWidth * 0.5f);
        _targetPos.x = worldX;

        _inAirVector.y = _rigid.velocity.y;
        _rigid.velocity = _inAirVector;
        _rigid.MovePosition(Vector3.Lerp(_rigid.position, _targetPos, 0.5f));

        Vector3 camPos = _rigid.position;
        camPos.x = _distancePos.x;
        _camTargetPos = camPos + _camOffset;
    }
    private void FixAirMoving()
    {
        _isGround = false;
        _inFixAirDistNow += Time.fixedDeltaTime * _nowSpeed * _speedBonus;

        SphereCollider selectCollider = _powerScript.GetActiveCollider();
        float t = _inFixAirDistNow / _inFixAirDistAll;
        _targetPos = Vector3.Lerp(_inFixAirStart, _inFixAirEnd, t) + new Vector3(0f, Mathf.Sin(t * Mathf.PI) * _inFixAirUp, 0f)
            + new Vector3(0f, selectCollider.radius * selectCollider.transform.localScale.y, 0f);
        _rigid.MovePosition(Vector3.Lerp(_rigid.position, _targetPos, 0.5f));

        _camRotMult = Mathf.Lerp(_camRotMult, 0f, Time.fixedDeltaTime * _rotateFlySlowness);

        Vector3 camRigidOffset = new Vector3(0f, selectCollider.radius * selectCollider.transform.localScale.y, 0f);
        _camTargetPos = _rigid.position - camRigidOffset * 0.1f + _camOffset;
    }

    private void BallRotateX()
    {
        if (_road && !_inAirTrigger)
        {
            // Ball Rot Set
            float angleToTarget = Mathf.Atan2(_distanceDir.x, _distanceDir.z) * Mathf.Rad2Deg;
            Quaternion targetRot = Quaternion.Euler(_rigid.rotation.eulerAngles.x, angleToTarget, _rigid.rotation.eulerAngles.z);
            _rigid.transform.rotation = targetRot;
        }
        _rotateObj.Rotate(_camRotMult * _rotatePlayer * 10f * Time.fixedDeltaTime * (_nowSpeed / _speed), 0f, 0f, Space.Self);
    }
    private void CameraPosRot()
    {
        if (!_isControll)
            _camTargetPos += _camFinishOffset;
        // x lerp
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, new Vector3(_camTargetPos.x, _camera.transform.position.y, _camera.transform.position.z), Time.fixedDeltaTime * _camLerp.x * _camLerpMult);
        // y lerp
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, new Vector3(_camera.transform.position.x, _camTargetPos.y, _camera.transform.position.z), Time.fixedDeltaTime * _camLerp.y * _camLerpMult);
        // z lerp
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, new Vector3(_camera.transform.position.x, _camera.transform.position.y, _camTargetPos.z), Time.fixedDeltaTime * _camLerp.z);

        _camera.transform.rotation = Quaternion.Lerp(_camera.transform.rotation, Quaternion.Euler((new Vector3(0f, (_road && !_inAirTrigger ? 0f : _camRotValue) + _camRotStart.y, 0f) + new Vector3(_camRotStart.x, 0f, _camRotStart.z)) + (_isControll ? Vector3.zero : _camFinishRot)), Time.fixedDeltaTime * 5f);
        if (!_isControll)
            _camTargetPos -= _camFinishOffset;

        // FOV
        Vector3 targetDir = (_rigid.transform.position - _rigidPosPrev).normalized;
        targetDir.x = 0f;
        float angle = Vector3.Angle(Vector3.forward, targetDir) * -1f;
        if (targetDir.y > 0f)
            angle *= -1f;

        angle = Mathf.Clamp(angle, -_camMaxAngle, 0f);
        _rigidAngle = Mathf.Lerp(_rigidAngle, angle, Time.fixedDeltaTime * 10f);
        float anglePercent = _rigidAngle / -_camMaxAngle;
        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, 
            Mathf.Lerp(_camStartFov, _camMaxFov, anglePercent), 
            Time.fixedDeltaTime * _camLerpFov);
        _rigidPosPrev = _rigid.transform.position;
    }
    private void SizeTextSetPos()
    {
        // Size Pos Set
        SphereCollider selectCollider = _powerScript.GetActiveCollider();

        Vector3 addOffset = _sizeTextOffset.normalized * (selectCollider.radius * selectCollider.transform.localScale.y);
        _sizeText.transform.position = _rigid.position + _sizeTextOffset + addOffset;
    }
    private void RollingEffect()
    {
        SphereCollider selectCollider = _powerScript.GetActiveCollider();
        _rollingEff.transform.localPosition = new Vector3(0f, -0.8f * selectCollider.radius * selectCollider.transform.localScale.y, 0f);
        if (!_isMove)
        {
            _rollingEff.emissionRate = 0f;
            return;
        }
        if (_doodleJumping || _inAirTrigger)
        {
            _rollingEff.emissionRate = (Physics.OverlapSphere(transform.position, selectCollider.radius * selectCollider.transform.localScale.y + 0.45f, _roadLayer).Length > 0) ? _rollingEmiss : 0f;
        }
        else
        {
            _rollingEff.emissionRate = _isGround ? _rollingEmiss : 0f;
        }
    }

    public void InAirTrigger(bool enter)
    {
        if (_inAirTrigger == enter) return;
        _inAirTrigger = enter;
        if (enter && _road)
        {
            _lastRoadWidth = _road.Width;
            SoundController.Instance.PlaySound(SoundController.SoundIds.Jump);
            _rigid.useGravity = true;

            _inAirVector = _road.Path.path.GetPoint(_road.Path.path.NumPoints - 1) - _road.Path.path.GetPointAtDistance(_distance);
            _inAirVector = _inAirVector.normalized * _nowSpeed * _jumpMult;
            _camRotValue = 0f;
            StartCoroutine(CamLerpChanging());
        }
        _road = null;
    }
    public void InFixAirTrigger(bool enter, Vector3 startPoint, Vector3 endPoint, float upLength = 0f)
    {
        if (_inAirTrigger == enter) return;
        _inAirTrigger = enter;
        if (enter)
        {
            SoundController.Instance.PlaySound(SoundController.SoundIds.Jump);
            _camRotValue = 0f;

            SphereCollider selectCollider = _powerScript.GetActiveCollider();
            _inFixAirStart = _rigid.position - new Vector3(0f, selectCollider.radius * selectCollider.transform.localScale.y, 0f);
            _inFixAirEnd = endPoint;
            _inFixAirUp = upLength;
            _inFixAirDistNow = 0f;
            _inFixAirDistAll = 0f;
            for (int i = 0; i < 30; i++)
            {
                float t_1 = ((float)i / 30f);
                float add_y_1 = Mathf.Sin(t_1 * Mathf.PI) * upLength;
                Vector3 pos_1 = Vector3.Lerp(_inFixAirStart, endPoint, t_1) + new Vector3(0f, add_y_1, 0f);

                float t_2 = ((float)(i + 1) / 30f);
                float add_y_2 = Mathf.Sin(t_2 * Mathf.PI) * upLength;
                Vector3 pos_2 = Vector3.Lerp(_inFixAirStart, endPoint, t_2) + new Vector3(0f, add_y_2, 0f);

                _inFixAirDistAll += Vector3.Distance(pos_1, pos_2);
            }
            _inFixAirTrigger = true;
            StartCoroutine(CamLerpChanging());
        }
        _road = null;
    }
    public void SetControll(bool state)
    {
        _isControll = state;
        GeneralSettings.Instance.SetTutorial(state);
    }
    private IEnumerator CamLerpChanging()
    {
        float t = 0f;
        float t_all = 1.5f;
        while (t <= t_all)
        {
            _camLerpMult = _camLerpMultCurve.Evaluate(t / t_all);
            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator SpeedUp()
    {
        float t = 0f;
        while (t < _speedUpTime)
        {
            _nowSpeed = _speed * _speedUp.Evaluate(t / _speedUpTime);
            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        _nowSpeed = _speed * _speedUp.Evaluate(1f);
        StartCoroutine(SpeedDown());
    }
    private IEnumerator SpeedDown()
    {
        Vector3 finishPos;
        if (GameObject.FindWithTag("Finish"))
        {
            finishPos = GameObject.FindWithTag("Finish").transform.position;
        }
        else
        {
            List<PathCreator> roads = FindObjectsOfType<PathCreator>().ToList();
            PathCreator longRoad = roads[0];
            for (int i = 0; i < roads.Count; i++)
            {
                if (roads[i].path.GetPoint(roads[i].path.NumPoints - 1).magnitude < longRoad.path.GetPoint(longRoad.path.NumPoints - 1).magnitude)
                {
                    longRoad = roads[i];
                }
            }
            finishPos = longRoad.path.GetPoint(longRoad.path.NumPoints - 1);
        }

        float startDistance = Vector3.Distance(transform.position, finishPos);
        while (_isMove)
        {
            _nowSpeed = _speed * _speedDown.Evaluate(1f - Vector3.Distance(transform.position, finishPos) / startDistance);
            yield return new WaitForFixedUpdate();
        }
    }
    public void SetSpeedBonus(float bonus)
    {
        _speedBonus = bonus;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
            _isTouch = false;
    }
    public void Touch(bool state)
    {
        _isTouch = state;
        if (state && _road && !_inAirTrigger)
        {
            _touchStartDelta = _borderDistance;
            _touchStartDelta = Mathf.Clamp(_touchStartDelta, 0, _road.Width);
        }
        _touchMousePos = Input.mousePosition;
    }
    public void SetMove(bool state)
    {
        _isMove = state;
        _rigid.velocity = Vector3.zero;
        if (state)
        {
            StartCoroutine(SpeedUp());
        }
    }
    public Vector3 GetVelocity()
    {
        return _rigid.velocity;
    }
    public void AddPowerSize(float power, float size)
    {
        _power += power * _powerBonus;
        _size *= (1f + size);
        UpdateSize();
        _sizeText.transform.localScale = _sizeStartScale * _sizeTextScale;
    }
    public void SetPowerBonus(float bonus)
    {
        _powerBonus = bonus;
    }
    public IEnumerator CameraShake()
    {
        yield return null;
        float t = 0f;
        while (t <= _shakeTime)
        {
            Vector3 shakeOffset = new Vector3(
                Random.Range(-_shakePower, _shakePower) * ((_shakeTime - t) / _shakeTime),
                Random.Range(-_shakePower, _shakePower) * ((_shakeTime - t) / _shakeTime),
                Random.Range(-_shakePower, _shakePower) * ((_shakeTime - t) / _shakeTime)
                );
            _camera.transform.position = _camera.transform.position + shakeOffset;
            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    public void AddOnLayer(Transform obj)
    {
        PlayerPowerEnemys.Instance.AddUnit(obj);
    }
    public float RemovePowerSize(float power, float size, bool useShake = true)
    {
        ParticleSystem damageEff = Instantiate(_damageEff, _damageEff.transform.position, _damageEff.transform.rotation, null);
        damageEff.Play();
        float fromPower = _power;
        while (power > 0 && _power > 0)
        {
            power--;
            _power--;
            _size *= (1f - size);
        }
        PlayerPowerEnemys.Instance.RemoveUnit(1f - (_power / fromPower));
        UpdateSize();
        if (_power <= 0)
            Dead();
        if (_damageShake && useShake)
            StartCoroutine(CameraShake());
        return power;
    }
    public float GetPower()
    {
        return _power;
    }
    private void Dead()
    {
        _destroyEff.Play();
        _rollingEff.emissionRate = 0f;
        _rotateObj.gameObject.SetActive(false);
        _sizeText.gameObject.SetActive(false);
        Vibration.Vibrate(Vibration.VibroType.Default);
        Debug.Log("Controller - Dead!");
        SetMove(false);
        FindObjectOfType<GeneralSettings>(true).LoseFunc();
        Debug.Log("Controller - Dead complete!");
    }
    private void UpdateSize()
    {
        _sizeText.text = _power.ToString("0");
        Vector3 scale = new Vector3(_size, _size, _size);
        _sizeText.transform.localScale = scale;
        GeneralSettings.Instance._flagColorController.UpdateFlags(_power);
    }
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (_inAirTrigger)
            return;
        if (collision.gameObject.TryGetComponent<RoadIdentificator>(out RoadIdentificator newRoad)) // Layer - Ground
        {
            if (newRoad != _road)
            {
                _maxDistance = newRoad.Path.path.length - 0.01f;
                _rigid.useGravity = false;
                _distance = newRoad.Path.path.GetClosestDistanceAlongPath(_rigid.position);
                _distancePos = newRoad.Path.path.GetPointAtDistance(_distance);
                _touchStartDelta = _rigid.position.x - (_distancePos.x - newRoad.Width * 0.5f);
                _borderDistance = _touchStartDelta;

                _rigid.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

                _road = newRoad;

                if (!_inAirTrigger && _inFixAirTrigger)
                {
                    _inFixAirTrigger = false;
                }

                if (Time.time > _startTime + 0.1f)
                {
                    ParticleSystem langingEff = Instantiate(_langingEff, _langingEff.transform.position, _langingEff.transform.rotation, null);
                    langingEff.Play();
                    StartCoroutine(CamLerpChanging());
                    StartCoroutine(DoodleJump());
                    if (_langingShake)
                        StartCoroutine(CameraShake());
                }
                RoadMoving();
            }
        }
    }
    private IEnumerator DoodleJump()
    {
        SoundController.Instance.PlaySound(SoundController.SoundIds.Landing);
        Vibration.Vibrate(Vibration.VibroType.Default);
        _doodleJumping = true;
        float t = 0f;
        float t_all = _doodleJumpTimer;
        while (t <= t_all)
        {
            _doodleJumpingValue = _doodleJumpCurve.Evaluate(t / t_all) * _doodleJumpMult;
            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        _doodleJumpingValue = 0f;
        _doodleJumping = false;
    }

#if UNITY_EDITOR
    [Foldout("Debug")][ReadOnly][SerializeField] private List<Vector3> camPoss = new List<Vector3>();
    [Foldout("Debug")][ReadOnly][SerializeField] private List<Vector3> playerPoss = new List<Vector3>();
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            camPoss.Add(_camTargetPos);
            playerPoss.Add(_rigid.position);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, _targetPos);

            if (_isGround)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.green;
            }

            if (camPoss.Count > 1)
            {
                Gizmos.color = Color.red;
                for(int i = 0; i < camPoss.Count - 1; i++)
                {
                    Gizmos.DrawLine(camPoss[i + 1], camPoss[i]);
                }
            }
            if (playerPoss.Count > 1)
            {
                Gizmos.color = Color.yellow;
                for (int i = 0; i < playerPoss.Count - 1; i++)
                {
                    Gizmos.DrawLine(playerPoss[i + 1], playerPoss[i]);
                }
            }
            //SphereCollider selectCollider = _powerScript.GetActiveCollider();
            //Gizmos.DrawWireSphere(_targetPos, selectCollider.radius * selectCollider.transform.localScale.y + 0.45f);
        }
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
            return;

        _targetPos = transform.position;
    }

    [Button]
    public void ChangeTime()
    {
        Time.timeScale = Time.timeScale == 0.15f ? 1f : 0.15f;
    }
#endif
}
