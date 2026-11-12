using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish1Control : MonoBehaviour
{
    public string fishName = "Fish1"; // 鱼的名字（必须和Prefab名或者数据库ID对应）喵
    public float moveSpeed = 2f;
    public Vector2 swimAreaMin; // 游动范围左下角 喵
    public Vector2 swimAreaMax; // 游动范围右上角 喵

    private Vector3 targetPosition;
    private bool isCaught = false; // 是否被抓住了 喵
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        PickNewTarget();
    }

    void Update()
    {
        // 如果被抓住了，就什么都不做，乖乖当鱼钩的子物体喵
        if (isCaught)
            return;

        // 向目标游动 喵
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // 简单的朝向翻转 喵
        if (targetPosition.x > transform.position.x) sr.flipX = false; // 向右游
        else sr.flipX = true; // 向左游

        // 到达目标后，选个新地方 喵
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            PickNewTarget();
        }
    }

    void PickNewTarget()
    {
        // 在设定范围内随机找个点 喵
        float randomX = Random.Range(swimAreaMin.x, swimAreaMax.x);
        float randomY = Random.Range(swimAreaMin.y, swimAreaMax.y);
        targetPosition = new Vector3(randomX, randomY, 0);
    }

    // 碰撞检测：遇到鱼钩 喵
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 只有没被抓且碰到的东西是鱼钩才触发 喵
        if (!isCaught && other.CompareTag("Hook"))
        {
            // 检查鱼钩是不是已经挂满鱼了（假设一次只能钓一条）喵
            Hook_UWControl hook = other.GetComponent<Hook_UWControl>();
            if (hook != null && !hook.hasFish)
            {
                GetCaught(other.transform, hook);
            }
        }
    }

    void GetCaught(Transform hookTransform, Hook_UWControl hookScript)
    {
        isCaught = true;

        // 1. 变成鱼钩的子物体，这样就会跟着鱼钩移动了！喵
        transform.SetParent(hookTransform);

        // 2. 归零坐标，把鱼挂在钩子正中间（或者稍微向下偏移一点）喵
        transform.localPosition = new Vector3(0, -2f, -1f);

        // 3. 旋转90度，做出一种“被钓起来垂着”的感觉 喵
        transform.localRotation = Quaternion.Euler(0, 0, -90);

        // 4. 告诉鱼钩脚本：你已经有鱼了 喵
        hookScript.CatchFish(fishName);
    }
}
