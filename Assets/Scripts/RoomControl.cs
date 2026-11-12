using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomControl : MonoBehaviour
{
    private bool GetIn = false;

    private void Update()
    {
        if (GetIn && Input.GetKeyDown(KeyCode.F))
        {
            StatusTable.instance.SaveCurrentSceneData();
            SceneManager.LoadScene("Room");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GetIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GetIn = false;
        }
    }
}
