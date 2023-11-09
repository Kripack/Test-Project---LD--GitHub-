using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITextEffect : MonoBehaviour
{
    [SerializeField, BoxGroup("Prefab")] private TextMeshProUGUI _prefab;

    [SerializeField, BoxGroup("Settings")] private Ease _moveType;
    [SerializeField, BoxGroup("Settings")] private float _flyingValue = 1.5f;
    [SerializeField, BoxGroup("Settings")] private float _flyingTime = 1f;
    [SerializeField, BoxGroup("Settings")] private float _alphaTime = 0.3f;

    private List<TextMeshProUGUI> _prefabs = new List<TextMeshProUGUI>();

    public void ShowEffect(Vector2 position, float value)
    {
        value = Mathf.FloorToInt(value * 10f) * 0.1f;
        if (value <= 0f)
            return;
        TextMeshProUGUI item = GetItem();
        item.rectTransform.position = position;
        int intValue = Mathf.FloorToInt(value);
        int floatValue = Mathf.FloorToInt((value - (float)intValue) * 10f);
        string resText = "+" + intValue;
        if (floatValue > 0f)
            resText += "." + floatValue;
        item.text = resText;
        item.gameObject.SetActive(true);
        Color startColor = item.color;
        Color endColor = startColor;
        endColor.a = 0f;

        /*DOTween.Sequence()
            .SetEase(_moveType)
            .Append(item.transform.DOMoveY(item.transform.position.y + _flyingValue * 1920f, _flyingTime))
            .Append(item.DOColor(endColor, _alphaTime)).OnComplete(
                () => { item.gameObject.SetActive(false); item.color = startColor; });*/
    }

    private void DeactiveObj(GameObject obj)
    {
        obj.SetActive(false);
    }

    private TextMeshProUGUI GetItem()
    {
        if (_prefabs.Count > 0)
        {
            for (int i = 0; i < _prefabs.Count; i++)
            {
                if (!_prefabs[i].gameObject.activeSelf)
                {
                    return _prefabs[i];
                }
            }
        }
        TextMeshProUGUI item = Instantiate(_prefab, transform);
        _prefabs.Add(item);
        return _prefabs[^1];
    }


}
