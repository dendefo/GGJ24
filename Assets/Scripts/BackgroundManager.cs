using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class BackgroundManager : MonoBehaviour
{
    [SerializeField] ParalaxedObject[] ParalaxedObjects;
    
    [SerializeField] public Transform Camera;



#if UNITY_EDITOR
    // Update is called once per frame
    void OnValidate()
    {
        foreach (ParalaxedObject Obj in ParalaxedObjects) 
        {
            if (Selection.activeTransform != Obj.transform) Obj.transform.position = Obj.InitialPosition + new Vector3(Camera.position.x, Camera.position.y,0) * (Obj.InitialPosition.z/100);

        }
    }
#endif
}
