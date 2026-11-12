using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartControl : MonoBehaviour
{
    public float floatStrength = 10f; // 浮动幅度
    public float floatSpeed = 2f;     // 浮动速度

    private Vector3 startPos;
    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // 利用 Sin 函数制造平滑的上下移动
        float newY = startPos.y + (Mathf.Sin(Time.time * floatSpeed) * floatStrength);
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
