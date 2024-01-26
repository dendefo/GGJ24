using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BackgroundHandler : MonoBehaviour
{
    [SerializeField] SpriteRenderer ImageColor;
    [SerializeField] Gradient ColorProgression;
    [SerializeField] float Range = 100;
    [SerializeField] Transform CameraTransform;

    private float Ratio;

    // Update is called once per frame
    void Update()
    {
        Ratio = CameraTransform.position.y / Range;

        if (Ratio < 0) Ratio = 0;
        if (Ratio > 1) Ratio = 1;

        ImageColor.color = ColorProgression.Evaluate(Ratio);
    }
}
