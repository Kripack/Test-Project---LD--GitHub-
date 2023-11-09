using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Catapult_Launcher : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField]private Vector3 point_player_adjustment;
    [SerializeField] private Transform anchor_left;
    [SerializeField] private Transform anchor_right;
    [SerializeField] private Vector3 point_anchor_adjustment;
    private LineRenderer lr;
    private Vector3 p1, p2, p3, p4, p5;
    [SerializeField] private float player_radius = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        p1 = anchor_left.position + point_anchor_adjustment;
        p2 = player.position + point_player_adjustment - Vector3.right * player_radius;
        p3 = player.position + point_player_adjustment;
        p4 = player.position + point_player_adjustment + Vector3.right * player_radius;
        p5 = anchor_right.position + point_anchor_adjustment;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.z < transform.position.z - point_player_adjustment.z*2.5)
        {
            p1 = anchor_left.position + point_anchor_adjustment;
            p2 = player.position + point_player_adjustment - Vector3.right * player_radius;
            p3 = player.position + point_player_adjustment;
            p4 = player.position + point_player_adjustment + Vector3.right * player_radius;
            p5 = anchor_right.position + point_anchor_adjustment;

            lr.SetPosition(0, p1);
            lr.SetPosition(1, p2);
            lr.SetPosition(2, p3);
            lr.SetPosition(3, p4);
            lr.SetPosition(4, p5);

            anchor_left.LookAt(player, Vector3.up);
            Vector3 eulerAngles = anchor_left.rotation.eulerAngles;
            eulerAngles.x = 0;
            eulerAngles.z = 0;
            anchor_left.rotation = Quaternion.Euler(eulerAngles);

            anchor_right.LookAt(player, Vector3.up);
            eulerAngles = anchor_left.rotation.eulerAngles;
            eulerAngles.x = 0;
            eulerAngles.z = 0;
            anchor_left.rotation = Quaternion.Euler(eulerAngles);
        } else
        {
            Vector3 p = (anchor_left.position + point_anchor_adjustment * 2 + anchor_right.position) / 2;

            lr.SetPosition(0, p1);
            lr.SetPosition(1, p);
            lr.SetPosition(2, p);
            lr.SetPosition(3, p);
            lr.SetPosition(4, p5);
        }
    } 
}
