using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuidanceEventPenetrate : MonoBehaviour, ICanvasRaycastFilter
{
    private Image targetImage;
    public void SetImage(Image iamge)
    {
        this.targetImage = iamge;
    }
    /// <summary>
    /// 点击是否有效 是否渗透到下一层级
    /// </summary>
    /// <param name="sp"></param>
    /// <param name="eventCamera"></param>
    /// <returns></returns>
    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        if (targetImage == null) return true;
        bool res = RectTransformUtility.RectangleContainsScreenPoint(targetImage.transform as RectTransform,
            sp, eventCamera);

        return !res;
    }
}
