using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class zoom_in_out : MonoBehaviour
{
    Button Gaugebutton;
    Button Line_Chartbutton;
    public GameObject prefab1;
    public GameObject linechart;
    public static bool prefab1_iszoom = false;
    public static bool linechart_iszoom = false;

    public GameObject Funder0;
    public GameObject F0to15;
    public GameObject F15to30;
    public GameObject Fover30;

    double temp = GetAPI.temp;
    float PFxposition = (float)-0.23;
    float PFzposition = (float)0.02;
    float LFxposition = (float)0.28;
    float LFzposition = (float)0.02;

    void Start()
    {
        Gaugebutton = GameObject.Find("Gauge").GetComponent<Button>();
        Gaugebutton.onClick.AddListener(prefabbuttonclick);
        Line_Chartbutton = GameObject.Find("Line_Chart").GetComponent<Button>();
        Line_Chartbutton.onClick.AddListener(lineChartbuttonclick);

    }
    private void prefabbuttonclick(){
        if(!prefab1_iszoom){
            prefab1.transform.position+= new Vector3(PFxposition,0,PFzposition);
            Funder0.SetActive(false);
            F0to15.SetActive(false);
            F15to30.SetActive(false);
            Fover30.SetActive(false);
            prefab1_iszoom = true;
            if(linechart_iszoom){
                lineChartbuttonclick();
            }
            
        }else{
            prefab1.transform.position-= new Vector3(PFxposition,0,PFzposition);
            if(!linechart_iszoom||!prefab1_iszoom){
                face();
            }
            prefab1_iszoom = false;
        }
    }

    private void lineChartbuttonclick(){
            if(!linechart_iszoom){
            linechart.transform.position+= new Vector3(LFxposition,0,LFzposition);
            Funder0.SetActive(false);
            F0to15.SetActive(false);
            F15to30.SetActive(false);
            Fover30.SetActive(false);
            linechart_iszoom = true;
            if(prefab1_iszoom){
                prefabbuttonclick();
            }
        }else{
            linechart.transform.position-= new Vector3(LFxposition,0,LFzposition);
            if(!linechart_iszoom||!prefab1_iszoom){
                face();
            }
            linechart_iszoom = false;
        }
    }

    void face(){
        temp = GetAPI.temp;
        if(temp<0){
            Funder0.SetActive(true);
        }else if(temp>0&&temp<15){
            F0to15.SetActive(true);
        }else if(temp>15&&temp<30){
            F15to30.SetActive(true);
        }
        else if(temp>30){
            Fover30.SetActive(true);
        }
    }

    public void reset(){
        if(prefab1_iszoom){
            prefab1_iszoom = false;
            prefab1.transform.position-= new Vector3(PFxposition,0,PFzposition);
        }
        if(linechart_iszoom){
            linechart_iszoom = false;
            linechart.transform.position-= new Vector3(LFxposition,0,LFzposition);
        }

    } 
}
