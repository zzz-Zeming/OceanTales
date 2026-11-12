using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatControl : MonoBehaviour
{
    GameObject player;
    float dis;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerControl.playerControl.OnBoat && !PlayerControl.playerControl.Fishing)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlayerControl.playerControl.Row = !PlayerControl.playerControl.Row;
                dis = player.transform.position.x - this.transform.position.x;
            }
            if (PlayerControl.playerControl.Row && PlayerControl.playerControl.Rowing)
            {
                this.transform.position = new Vector3(player.transform.position.x - dis, this.transform.position.y, this.transform.position.z);
            }
        }
    }
}
