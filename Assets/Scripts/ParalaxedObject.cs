using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[ExecuteInEditMode]
public class ParalaxedObject : MonoBehaviour
{
    public Vector3 InitialPosition;
    public BackgroundManager BackgroundManager;

#if UNITY_EDITOR
    void Update()
    {
        if (Selection.activeTransform == this.transform)
        {
            InitialPosition = transform.position - new Vector3(BackgroundManager.Camera.position.x, BackgroundManager.Camera.position.y, 0) * (transform.position.z/100);
        }
    }
#endif
}
