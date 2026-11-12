using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HookControl : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Ocean")
        {
            rb.gravityScale = 0.1f;
            rb.linearDamping = 4f;
        }
        else if (collision.transform.tag == "DB_Inshore")
        {
            StatusTable.instance.SaveCurrentSceneData();
            SceneManager.LoadScene("Inshore");
        }
        else if (collision.transform.tag == "DB_Offshore")
        {
            StatusTable.instance.SaveCurrentSceneData();
            SceneManager.LoadScene("Offshore");
        }
        else if (collision.transform.tag == "DB_DeepOcean")
        {
            StatusTable.instance.SaveCurrentSceneData();
            SceneManager.LoadScene("DeepOcean");
        }
    }
}
