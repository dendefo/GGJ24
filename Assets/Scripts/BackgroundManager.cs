using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class BackgroundManager : MonoBehaviour
{
    [SerializeField] ParalaxedObject[] ParalaxedObjects;
    
    [SerializeField] public Transform Camera;



    
    // Update is called once per frame
    void Update()
    {
        foreach (ParalaxedObject Obj in ParalaxedObjects) 
        {
            if (Selection.activeTransform != Obj.transform) Obj.transform.position = Obj.InitialPosition + new Vector3(Camera.position.x, Camera.position.y,0) * (Obj.InitialPosition.z/100);

        }
    }
}
