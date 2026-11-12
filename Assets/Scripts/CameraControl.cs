using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    GameObject player;
    bool fishing = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!fishing)
        {
            if (player.transform.position.x >= -3.45f && player.transform.position.x <= 5f)
            {
                this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.85f, -10);
            }
        }
    }
}
