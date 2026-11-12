using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarControl : MonoBehaviour
{
    public static BarControl instance;

    public GameObject JudgeBar;
    public GameObject Fish;
    private Rigidbody2D rb_J;
    private Rigidbody2D rb_F;
    public Vector3 CurrentPosition;
    public Vector3 TargetPosition;
    private float timer = 0f;
    private float dis;
    public bool dragging=false;

    private void Awake()
    {
        instance = this;
        transform.position = CurrentPosition;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb_J = JudgeBar.GetComponent<Rigidbody2D>();
        rb_F = Fish.GetComponent<Rigidbody2D>();
        rb_J.gravityScale = 0.2f;
        rb_J.linearDamping = 2f;
        rb_F.gravityScale = 0.1f;
        rb_F.linearDamping = 4f;
        Fish.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Hook_UWControl.instance.hasFish)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, 0.01f);
            Fish.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb_J.AddForce(Vector2.up, ForceMode2D.Impulse);
            }

            timer += Time.deltaTime;
            if (Fish.transform.position.y > -0.6f)
            {
                rb_F.gravityScale = 0.1f;
            }
            else if (Fish.transform.position.y <= -0.6f)
            {
                rb_F.gravityScale = 0f;
            }
            if (timer > Random.Range(3f, 5f) && Fish.transform.position.y < 0.4f)
            {
                rb_F.AddForce(Vector2.up * Random.Range(1f, 3f), ForceMode2D.Impulse);
                timer = 0f;
            }

            dis = JudgeBar.transform.position.y - Fish.transform.position.y;
            if (-0.05f <= dis && dis <= 0.05f)
            {
                dragging = true;
            }
            else
            {
                dragging = false;
            }
        }
    }
}
