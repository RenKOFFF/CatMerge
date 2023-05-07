using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private void Awake()
    {
        UpdateSafeArea();
    }

    private void UpdateSafeArea()
    {
        var safeArea = Screen.safeArea;
        var safeAreaRectTransform = GetComponent<RectTransform>();

        var anchorMin = safeArea.position;
        var anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        safeAreaRectTransform.anchorMin = anchorMin;
        safeAreaRectTransform.anchorMax = anchorMax;
    }
}
