using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_UWControl : MonoBehaviour
{
    Transform Hook;

    public float LimitLeft;
    public float LimitRight;
    public float LimitUp;
    public float LimitDown;

    // Start is called before the first frame update
    void Start()
    {
        Hook = GameObject.FindGameObjectWithTag("Hook").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LimitLeft < Hook.position.x && Hook.position.x < LimitRight)
        {
            this.transform.position = new Vector3(Hook.position.x, this.transform.position.y, -10);
        }
        if (LimitDown < Hook.position.y && Hook.position.y < LimitUp)
        {
            this.transform.position = new Vector3(this.transform.position.x, Hook.position.y, -10);
        }
    }
}
