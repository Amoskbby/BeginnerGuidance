using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuidanceRect : MonoBehaviour
{
    public Image Target;
    private Vector4 center;
    private Vector3[] corners = new Vector3[4];
    private float width;
    private float height;
    private Canvas canvas;
    private Material material;

    public Vector2 WorldToCanvasPos(Canvas canvas, Vector3 worldPos)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
            worldPos, canvas.GetComponent<Camera>(), out position);
        return position;
    }

    private void Awake()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        Target.rectTransform.GetWorldCorners(corners);

        Vector3 worldPos = corners[0] + (corners[2] - corners[0]) / 2f;
        center = WorldToCanvasPos(canvas, worldPos);

        width = (WorldToCanvasPos(canvas, corners[2]) - WorldToCanvasPos(canvas, corners[0])).x / 2f;
        height = (WorldToCanvasPos(canvas, corners[2]) - WorldToCanvasPos(canvas, corners[0])).y / 2f;
        RectTransform canvasRectTransform = canvas.transform as RectTransform;
        if (canvasRectTransform != null)
        {
            canvasRectTransform.GetWorldCorners(corners);
            foreach (var corner in corners)
            {
                //curWidth = Mathf.Max(Mathf.Abs(center.x - WorldToCanvasPos(canvas, corner).x), curWidth);
                //curHeight = Mathf.Max(Mathf.Abs(center.y - WorldToCanvasPos(canvas, corner).y), curHeight);
            }
        }
        curHeight = Screen.height / 2f;
        curWidth = Screen.width / 2f;
        material = transform.GetComponent<Image>().material;
        if (material != null)
        {
            material.SetVector("_Center", center);
            material.SetFloat("_SliderX", curWidth);
            material.SetFloat("_SliderY", curHeight);
        }

    }
    private float curHeight;
    private float curWidth;
    private float shrinkVelocityX = 0;
    private float shrinkVelocityY = 0;
    public float ShrinkTime = 0.5f;
    private void Update()
    {
        float newWidth = Mathf.SmoothDamp(curWidth, width, ref shrinkVelocityX, ShrinkTime);
        float newHeight = Mathf.SmoothDamp(curHeight, height, ref shrinkVelocityY, ShrinkTime);

        if (!Mathf.Approximately(newHeight, curHeight))
        {
            curWidth = newWidth;
            material.SetFloat("_SliderX", curWidth);
        }

        if (!Mathf.Approximately(newHeight, curHeight))
        {
            curHeight = newHeight;
            material.SetFloat("_SliderY", curHeight);
        }
    }

}
