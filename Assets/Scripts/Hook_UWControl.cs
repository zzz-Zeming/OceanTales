using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hook_UWControl : MonoBehaviour
{
    public static Hook_UWControl instance;

    private Rigidbody2D rb;
    public float LimitLeft;
    public float LimitRight;
    public bool hasFish = false;
    private float timer = 0f;
    public float SeaHorizon;

    public float gs;
    public float d;

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.gravityScale = gs;
        rb.linearDamping = d;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (!hasFish)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector2.up, ForceMode2D.Impulse);
            }
            if (LimitLeft < transform.position.x && Input.GetKeyDown(KeyCode.A))
            {
                rb.AddForce(Vector2.left, ForceMode2D.Impulse);
            }
            if (Input.GetKeyDown(KeyCode.D) && transform.position.x < LimitRight)
            {
                rb.AddForce(Vector2.right, ForceMode2D.Impulse);
            }
        }
        else if (hasFish && BarControl.instance.dragging && timer > 0.3f)
        {
            rb.AddForce(Vector2.up * Random.Range(1f, 5f), ForceMode2D.Impulse);
            timer = 0f;
        }

        if (transform.position.y > SeaHorizon)
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "Inshore" || scene.name == "Offshore")
            {
                SceneManager.LoadScene("Land");
            }
            else if (scene.name == "DeepOcean")
            {
                SceneManager.LoadScene("Ocean");
            }
        }
    }

    public void CatchFish(string ID)
    {
        hasFish = true;
        StatusTable.instance.AddFish(ID);
    }
}
