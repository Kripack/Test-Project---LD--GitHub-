using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EffectButton : MonoBehaviour
{

    [BoxGroup("UI Settings")][SerializeField] private TextMeshProUGUI EffectPowerText;
    [BoxGroup("UI Settings")][SerializeField] private TextMeshProUGUI EffectCostText;

    [BoxGroup("Data Settings")][SerializeField] private EffectType Type;
    [BoxGroup("Data Settings")][SerializeField] private int StartCost;
    [BoxGroup("Data Settings")][SerializeField] private int CostStep;
    [BoxGroup("Data Settings")][SerializeField] private int PercentStep;
    [BoxGroup("Data Settings")][SerializeField] private int MaxLevel = 100;

    [BoxGroup("Debug")][ReadOnly][SerializeField] private int _level = 0;

    private enum EffectType { Speed, Power, Gold };

    private void Start()
    {
        UpdateVisual();
        UpdateEffects();
    }

    private void UpdateVisual()
    {
        int value = Mathf.RoundToInt((_level + 1) * (PercentStep * 0.1f));
        EffectPowerText.text = "x " + ((value / 10) + 1).ToString("0") + "." + (value % 10).ToString("0");
        EffectCostText.text = (_level * CostStep + StartCost).ToString() + 'g';
    }

    private void UpdateEffects()
    {
        switch (Type)
        {
            case (EffectType.Speed):
                {
                    PlayerController.Controller.SetSpeedBonus(1f + (_level  * (PercentStep * 0.01f)));
                    break;
                }
            case (EffectType.Power):
                {
                    PlayerController.Controller.SetPowerBonus(1f + (_level * (PercentStep * 0.01f)));
                    break;
                }
            case (EffectType.Gold):
                {
                    GeneralSettings.Instance.SetCoinBonus(1f + (_level * (PercentStep * 0.01f)));
                    break;
                }
        }
    }

    public void TryBuy()
    {
        int needCoin = _level * CostStep + StartCost;
        if (needCoin <= GeneralSettings.Instance.GetCoin())
        {
            Debug.Log("Buy succefull!");
            GeneralSettings.Instance.RemoveCoin(needCoin);
            _level = Mathf.Clamp(_level + 1, 0, MaxLevel);
            UpdateEffects();
            UpdateVisual();
        }
        else
        {
            Debug.Log("Buy false...");
        }
    }


}
