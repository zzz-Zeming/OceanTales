using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDockControl : MonoBehaviour
{
    bool CanGetOnBoat = false;

    private void Update()
    {
        if (CanGetOnBoat && Input.GetKeyDown(KeyCode.F) && !PlayerControl.playerControl.Fishing)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().GetOnBoat();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CanGetOnBoat = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CanGetOnBoat = false;
        }
    }
}
