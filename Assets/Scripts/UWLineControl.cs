using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UWLineControl : MonoBehaviour
{
    public Transform hookPoint;      // 拖入你的鱼钩对象（或者鱼钩上专门绑线的一个子物体节点）喵

    public float surfaceX = -0.25f;
    public float surfaceY = 2.5f;      // 设为海平面的高度（一般是0，或者根据你的场景调整）喵
    public float lineWidth = 0.03f;  // 线的宽度（像素风通常要很细）喵
    public Color lineColor; // 线的颜色（白色或浅灰色比较像鱼线）喵

    private LineRenderer lr;

    void Start()
    {
        // 初始化 LineRenderer 组件配置 喵
        lr = GetComponent<LineRenderer>();

        // 设置线的宽度
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;

        // 设置只有2个点：起点（水面）和终点（鱼钩）
        lr.positionCount = 2;

        // 设置材质使用最基础的 Sprites-Default，这样线是实心的，不会发粉红光 喵
        lr.material = new Material(Shader.Find("Sprites/Default"));

        // 设置颜色
        lr.startColor = lineColor;
        lr.endColor = lineColor;
    }

    void LateUpdate() // 使用 LateUpdate 保证在鱼钩移动完之后再画线，防止线抖动 喵
    {
        if (hookPoint == null) return;

        // --- 计算起点 (水面) ---
        // 这里假设船是时刻跟着鱼钩在水面上方移动的（因为是侧视角游戏）
        // 所以起点的 X 坐标 = 鱼钩的 X 坐标，Y 坐标 = 固定的海平面高度
        Vector3 startPos = new Vector3(surfaceX, surfaceY, 0);

        // --- 计算终点 (鱼钩) ---
        // 为了美观，你可以让终点稍微往上偏移一点点，不要直接插在鱼钩中心
        Vector3 endPos = hookPoint.position + new Vector3(0, 0.1f, 0);

        // --- 画线 ---
        lr.SetPosition(0, startPos); // 点0：水面
        lr.SetPosition(1, endPos);   // 点1：鱼钩
    }
}
