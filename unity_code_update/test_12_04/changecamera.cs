using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class changecamera : MonoBehaviour
{
    public ARTrackedImageManager ARimage;
    public ARFaceManager ARface;
    private GameObject ARobject;
    public ARCameraManager cameraManager{
        get =>m_CameraManager;
        set =>m_CameraManager=value;
    }
    [SerializeField]
    ARCameraManager m_CameraManager;
    public static Boolean camerafront = true;

    void Start(){
        //ARface=GetComponent<ARFaceManager>();
        //ARimage=GetComponent<ARTrackedImageManager>();
    }
    
    

    public void cameraswitch(){
        Debug.Assert(m_CameraManager != null, "camera manager cannot be null");
        CameraFacingDirection newfacingdirection;
        switch(m_CameraManager.requestedFacingDirection){
            case CameraFacingDirection.User:
            camerafront=false;
            newfacingdirection = CameraFacingDirection.World;
            ARobject= GameObject.FindWithTag("ARface");
            Destroy(ARobject);
            ARface.enabled=false;
            break;
            case CameraFacingDirection.World:
            default:
            camerafront=true;
            newfacingdirection  =CameraFacingDirection.User;
            ARface.enabled=true;
            break;
        }
        cameraManager.requestedFacingDirection=newfacingdirection;
    }
}
