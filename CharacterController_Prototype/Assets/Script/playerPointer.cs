using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPointer : MonoBehaviour
{
    private Vector3 player2TargetPos;
    private RectTransform pointerRectTransform;

    private void Awake()
    {
        player2TargetPos = new Vector3(200, 45);
        pointerRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector3 toPosition = player2TargetPos;
    }
}
