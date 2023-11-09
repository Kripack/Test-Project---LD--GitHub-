using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin_Spin : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float y_rotation = 0.1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, y_rotation);
    }
}
