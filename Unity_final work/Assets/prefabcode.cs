using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Net;
using System.Security.Policy;
using Newtonsoft.Json;
using XCharts.Runtime;
public class prefabcode : MonoBehaviour
{
    double temp = GetAPI.temp;
    List<double> dailymaxtemp;
    List<double> dailymintemp;
    List<string> time;
    string cityname = GetAPI.cityname;
    int weather_code;
    string weather;
    int citynum = GetAPI.citynum;

    float tempangle;
    //public float initialvalue = 0;
    float xvalueD;
    Boolean camerafront;
    
    

    

    public LineChart lineChart;
    public GameObject face_under0;
    public GameObject face_0to15;
    public GameObject face_15to30;
    public GameObject face_over30;
    public GameObject raineffect;
    public GameObject snowffect;
    public GameObject cloudsffect;
    public TextMeshPro Citytext;
    public TextMeshPro Temptext;
    public TextMeshPro Weathetext;
    public GameObject pointer;
    public float localtemp = GetAPI.localtemp;

    void Awake(){
      temp = GetAPI.temp;
      cityname = GetAPI.cityname;
      weather_code = GetAPI.weather_code;
      time = GetAPI.time;
      dailymaxtemp = GetAPI.dailymaxtemp;
      dailymintemp = GetAPI.dailymintemp;
      camerafront = changecamera.camerafront;
      citynum = GetAPI.citynum;

    }
    void Start(){
      PushData();

    }
    void Update(){
      
    }

    //get data from GetAPI
    private void Readvalue(){
        temp = GetAPI.temp;
        cityname = GetAPI.cityname;
        weather_code = GetAPI.weather_code;
        time = GetAPI.time;
        dailymaxtemp = GetAPI.dailymaxtemp;
        dailymintemp = GetAPI.dailymintemp;
        camerafront = changecamera.camerafront;
        citynum = GetAPI.citynum;
    }

    //control AR prefab
    public void PushData(){
        Readvalue();
        //Push temperature data to UI text object
        Temptext.text = temp.ToString()+"°C";
        Debug.Log(temp.ToString());
        //weather effect
        weathertype(weather_code);
        //emoji
        changeface();
        //move gauge pointer
        pointer.transform.localRotation = Quaternion.Euler(0,0,0);
        if(temp<=0){
          tempangle = (float)67.5;
        }else if(temp>0&&temp<=15){
          tempangle = (float)22.5;
        }else if(temp>15&&temp<=30){
          tempangle = (float)-22.5;
        }else if(temp>30){
          tempangle = (float)-67.5;
        }
        if(!camerafront){
          tempangle =(-tempangle);
        }
        pointer.transform.Rotate(new Vector3(0f,tempangle,0f),Space.Self);

        //send data to arduino
        UduinoManager.Instance.sendCommand("tempdata", temp,citynum);
        Chart();
        GameObject.FindWithTag("ARface").SendMessage("reset");
    }

    public void pushlocal(){
        citynum = GetAPI.citynum;
        temp = GetAPI.temp;
        cityname = GetAPI.cityname;
        raineffect.SetActive(false);
        snowffect.SetActive(false);
        cloudsffect.SetActive(false);
        //Citytext.text = cityname;

        Temptext.text = temp.ToString("#0.00")+"°C";
        changeface();
        pointer.transform.localRotation = Quaternion.Euler(0,0,0);
        if(temp<=0){
          tempangle = (float)67.5;
        }else if(temp>0&&temp<=15){
          tempangle = (float)22.5;
        }else if(temp>15&&temp<=30){
          tempangle = (float)-22.5;
        }else if(temp>30){
          tempangle = (float)-67.5;
        }
        if(!camerafront){
          tempangle =(-tempangle);
        }
        pointer.transform.Rotate(new Vector3(0f,tempangle,0f),Space.Self);
        UduinoManager.Instance.sendCommand("tempdata", temp,citynum);
        GameObject.FindWithTag("ARface").SendMessage("reset");
    }

    public void Chart(){
        DateTime original = DateTime.Now;
        lineChart.ClearData();
        for (int i = 0; i < dailymaxtemp.Count; i++)
        {
          DateTime result = original.AddDays(i);
          //print(result);
          lineChart.AddXAxisData(result.ToString("MM-dd"));
          lineChart.AddData(0, dailymaxtemp[i]);
          lineChart.AddData(1, dailymintemp[i]);
        }
    }

  void weathertype(int Wcode){
    raineffect.SetActive(false);
    snowffect.SetActive(false);
    cloudsffect.SetActive(false);
    if(Wcode==0){
      weather = "Clear sky";
    }else if(Wcode==1||Wcode==2||Wcode==3){
      weather="Partly cloudy";
      cloudsffect.SetActive(true);
    }else if(Wcode==45||Wcode==48){
      weather="Fog";
    }else if(Wcode==51||Wcode==53||Wcode==55||Wcode==56||Wcode==57){
      weather="Drizzle";
    }else if(Wcode==61||Wcode==63||Wcode==65||Wcode==66||Wcode==67||Wcode==80||Wcode==81||Wcode==82){
      weather="Rain";
      raineffect.SetActive(true);
    }else if(Wcode==71||Wcode==73||Wcode==75||Wcode==77||Wcode==85||Wcode==86){
      weather="Snow";
      snowffect.SetActive(true);
    }else if(Wcode==95){
      weather="Thunderstorm";
    }else if(Wcode==96||Wcode==99){
      weather="Thunderstorm with slight";
    }
    // if(citynum==0){
    //   Weathetext.text =weather;
    // }
    // Weathetext.text =weather;
  }

  void changeface(){
    face_under0.SetActive(false);
    face_0to15.SetActive(false);
    face_15to30.SetActive(false);
    face_over30.SetActive(false);
    if(camerafront){
        if(temp<=0){
            face_under0.SetActive(true);
        }else if(temp>0&&temp<=15){
            face_0to15.SetActive(true);
        }else if(temp>15&&temp<=30){
            face_15to30.SetActive(true);
        }
        else if(temp>30){
            face_over30.SetActive(true);
        }
    }
  }
}
