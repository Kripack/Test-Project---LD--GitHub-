using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Hand_Movement : MonoBehaviour
{
    [SerializeField] private float distance = 1f;
    [SerializeField] private float speed = 1f;
    private float current_pos;
    // Start is called before the first frame update
    void Start()
    {
        current_pos = 0;
    }

    // Update is called once per frame
    void Update()
    {
        current_pos += Time.deltaTime * speed;
        Vector2 pos = GetComponent<RectTransform>().anchoredPosition;
        pos.x += Mathf.Cos(current_pos)*distance;
        GetComponent<RectTransform>().anchoredPosition = pos;
    }
}
