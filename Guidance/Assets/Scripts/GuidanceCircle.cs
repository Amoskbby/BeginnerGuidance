using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuidanceCircle : MonoBehaviour
{
    public Image Target;

    private Vector3[] corners = new Vector3[4];

    private Vector4 center;

    private float radius;

    private Material material;

    private float currentRadius;

    public float ShrinkTime = 2f;

    private Canvas canvas;

    private GuidanceEventPenetrate guidanceEventPenetrate;

    /// <summary>
    /// 世界坐标到画布坐标
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="world"></param>
    /// <returns></returns>
    private Vector2 WorldToCanvasPos(Canvas canvas, Vector3 world)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
            world, canvas.GetComponent<Camera>(), out position);
        return position;
    }

    private void Awake()
    {
        guidanceEventPenetrate = GetComponent<GuidanceEventPenetrate>();
        if (guidanceEventPenetrate != null)
        {
            guidanceEventPenetrate.SetImage(Target);
        }
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        // 获取在世界坐标的四个角
        Target.rectTransform.GetWorldCorners(corners);
        // 通过距离 计算圆的半径
        radius = Vector2.Distance(WorldToCanvasPos(canvas, corners[0]), WorldToCanvasPos(canvas, corners[2])) / 2f;
        float x = corners[0].x + ((corners[3].x - corners[0].x) / 2f);
        float y = corners[0].y + ((corners[1].y - corners[0].y) / 2f);

        Vector3 centerWorld = new Vector3(x, y, 0);
        Vector2 center = WorldToCanvasPos(canvas, centerWorld);

        Vector4 centerMat = new Vector4(center.x, center.y);
        material = GetComponent<Image>().material;
        material.SetVector("_Center", centerMat);

        RectTransform canvasRectTransform = canvas.transform as RectTransform;
        if (canvasRectTransform != null)
        {
            canvasRectTransform.GetWorldCorners(corners);
            foreach (var corner in corners)
            {
                currentRadius = Mathf.Max(Vector2.Distance(center, WorldToCanvasPos(canvas, corner)), currentRadius);
            }
        }
        material.SetFloat("_Slider", currentRadius);
        curRadius = radius;
        maxRadius = 2 * radius;


    }

    private float shrinkVelocity = 0f;
    private bool inverse = false;
    private float maxRadius;
    private float curRadius;
    private void Update()
    {
        //float newRadius = Mathf.SmoothDamp(currentRadius, this.radius, ref shrinkVelocity, ShrinkTime);
        //if (!Mathf.Approximately(newRadius, currentRadius))
        //{
        //    currentRadius = newRadius;
        //    material.SetFloat("_Slider", currentRadius);
        //}
        Target.rectTransform.GetWorldCorners(corners);

        float x = corners[0].x + ((corners[3].x - corners[0].x) / 2f);
        float y = corners[0].y + ((corners[1].y - corners[0].y) / 2f);

        Vector3 centerWorld = new Vector3(x, y, 0);
        Vector2 center = WorldToCanvasPos(canvas, centerWorld);

        Vector4 centerMat = new Vector4(center.x, center.y);
        if (material != null)
        {
            material.SetVector("_Center", centerMat);
        }

        if (Mathf.Abs(maxRadius - curRadius) < 0.1f)
        {
            inverse = true;
        }
        if(Mathf.Abs(radius - curRadius) < 0.1f)
        {
            inverse = false;
        }

        if (inverse)
        {
            curRadius = Mathf.SmoothDamp(curRadius, radius, ref shrinkVelocity, ShrinkTime);
        }
        else
        {
            curRadius = Mathf.SmoothDamp(curRadius, maxRadius, ref shrinkVelocity, ShrinkTime);
        }

        material.SetFloat("_Slider", curRadius);
    }
}
