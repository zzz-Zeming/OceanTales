using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class OceanControl : MonoBehaviour
{
    float timer;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (2f <= timer && timer <= 5f || 8f <= timer && timer <=10f)
        {
            transform.Translate(Vector2.down * Time.deltaTime * 0.01f);
        }
        else
        {
            transform.Translate(Vector2.up * Time.deltaTime * 0.01f);
        }
        if (timer >= 10f)
        {
            timer = 0;
        }
        transform.Translate(Vector2.left * Time.deltaTime * 0.1f);
        if (transform.position.x <= -1.4f)
        {
            transform.position = new Vector3(transform.position.x + 0.7f, transform.position.y, transform.position.z);
        }
    }
}
