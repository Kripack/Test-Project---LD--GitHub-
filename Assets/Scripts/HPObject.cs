using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class HPObject : MonoBehaviour
{
    [BoxGroup("Settings")][SerializeField] private int HP = 50;
    [BoxGroup("Settings")][SerializeField] private float SizeCount = 0.2f;
    [BoxGroup("Settings")][SerializeField] private int Coins = 100;
    public enum ObjectType { None, Enemy, Obstacle, FinalCastle, FinalObject };
    [BoxGroup("Settings")][SerializeField] private ObjectType Type;
    [BoxGroup("Settings")][SerializeField] private bool TakeDamage = true;
    [BoxGroup("Settings")][SerializeField] private bool PowerTest = true;

    [BoxGroup("Shake")][SerializeField] private bool Shake = false;
    [BoxGroup("Shake")][SerializeField][ShowIf("Shake")] private float ShakePower;
    [BoxGroup("Shake")][SerializeField][ShowIf("Shake")] private float ShakeTime;
    [BoxGroup("Shake")][SerializeField] private bool CameraShake = true;

    [BoxGroup("Objects")][SerializeField] private TextMeshPro _hpText;

    [BoxGroup("Events")][SerializeField] private UnityEvent _damageEvent;
    [BoxGroup("Events")][SerializeField] private UnityEvent _deadEvent;

    private bool _once = true;

    private void Awake()
    {
        if (_hpText)
            _hpText.text = HP.ToString();
        _once = true;
    }

    public void Trigger()
    {
        if (_once)
        {
            _once = false;
            _damageEvent?.Invoke();
            if (PowerTest && PlayerController.Controller.GetPower() < (float)HP)
            {
                HP = Mathf.Max(HP - Mathf.RoundToInt(PlayerController.Controller.GetPower()), 0);
                PlayerController.Controller.RemovePowerSize(PlayerController.Controller.GetPower(), SizeCount, CameraShake);
            }
            else
            {
                if (TakeDamage)
                {
                    HP = Mathf.RoundToInt(PlayerController.Controller.RemovePowerSize((float)HP, SizeCount, CameraShake));
                }
                else
                {
                    HP = Mathf.Max(HP - Mathf.RoundToInt(PlayerController.Controller.GetPower()), 0);
                    if (CameraShake)
                        StartCoroutine(PlayerController.Controller.CameraShake());
                }
            }
            if (_hpText)
                _hpText.text = HP.ToString();
            if ((Type == ObjectType.FinalObject && HP > 0)
                    || (Type == ObjectType.FinalCastle
                    && !GeneralSettings.Instance.settings.FinishLine
                    && (HP <= 0 || !GeneralSettings.Instance.settings.NeedDestroyCastle)))
                Win();
            if (Type == ObjectType.FinalCastle)
                PlayerController.Controller.SetControll(false);
            if (HP <= 0)
            {
                Remove();
            }
            else
            {
                if (Shake)
                    StartCoroutine(ShakeEffect());
            }
        }
    }

    private void Remove()
    {
        AddCoin();
        _deadEvent?.Invoke();
    }
    public void AddCoin()
    {
        GeneralSettings.Instance.AddCoin(Coins, transform);
    }

    public void Win()
    {
        GeneralSettings.Instance.WinFunc();
    }

    public void SetHP(int value)
    {
        HP = value;
    }

    public int GetHP()
    {
        return HP;
    }

    private IEnumerator ShakeEffect()
    {
        Vector3 startPos = transform.position;
        float t = 0f;
        while (t <= ShakeTime)
        {
            Vector3 shakeOffset = new Vector3(
                Random.Range(-ShakePower, ShakePower) * ((ShakeTime - t) / ShakeTime),
                Random.Range(-ShakePower, ShakePower) * ((ShakeTime - t) / ShakeTime),
                Random.Range(-ShakePower, ShakePower) * ((ShakeTime - t) / ShakeTime)
                );

            transform.position = startPos + shakeOffset;
            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        transform.position = startPos;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            if (_hpText)
                _hpText.text = HP.ToString();
        }
    }
#endif

}
