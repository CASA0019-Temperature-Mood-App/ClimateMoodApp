using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class zoom_in_out : MonoBehaviour
{
    // Start is called before the first frame update
    Button zoombutton1;
    Button zoombutton2;
    GameObject zoomobject;
    public GameObject prefab1;
    public GameObject linechart;
    bool prefab1_iszoom = false;
    bool linechart_iszoom = false;
    bool camerafront;
    void Start()
    {
        zoombutton1 = GameObject.Find("zoombutton1").GetComponent<Button>();
        zoombutton1.onClick.AddListener(prefabbuttonclick);
        zoombutton2 = GameObject.Find("zoombutton2").GetComponent<Button>();
        zoombutton2.onClick.AddListener(lineChartbuttonclick);
        // zoomobject = findinactiveobjectName("zoombutton");
        // camerafront = GetAPI.camerafront;
        // if(camerafront){
        //     zoomobject.gameObject.SetActive(true);
        // }else{
        //     zoomobject.gameObject.SetActive(false);
        // }
    }
    private void prefabbuttonclick(){
        if(!prefab1_iszoom){
            double Dyposition=-0.115;
            float Fyposition = (float)Dyposition;
            double Dzposition = 0.06;
            float Fzposition = (float)Dzposition;
            prefab1.transform.position= new Vector3(0,Fyposition,Fzposition);
            prefab1_iszoom = true;
        }else{
            double Dxposition = 0.2;
            float Fxposition = (float)Dxposition;
            double Dyposition = -0.1;
            float Fyposition = (float)Dyposition;
            prefab1.transform.position= new Vector3(Fxposition,Fyposition,0);
            prefab1_iszoom = false;
        }
    }

    private void lineChartbuttonclick(){
            if(!linechart_iszoom){
            double Dyposition = -0.01;
            float Fyposition = (float)Dyposition;
            double Dzposition= 0.05;
            float Fzposition = (float)Dzposition;
            linechart.transform.position= new Vector3(0,Fyposition,Fzposition);
            linechart_iszoom = true;
        }else{
            double Dxposition = -0.27;
            float Fxposition = (float)Dxposition;
            double Dyposition = 0.01;
            float Fyposition = (float)Dyposition;
            linechart.transform.position= new Vector3(Fxposition,Fyposition,0);
            linechart_iszoom = false;
        }
    }



//   GameObject findinactiveobjectName(string name)
//   {
//     Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
//     for (int i = 0; i < objs.Length; i++)
//     {
//         if (objs[i].hideFlags == HideFlags.None)
//         {
//             if (objs[i].name == name)
//             {
//                 return objs[i].gameObject;
//             }
//         }
//     }
//     return null;
//   }
}
