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

public class servo2 : MonoBehaviour
{
    double temp;
    List<double> dailymaxtemp;
    List<double> dailymintemp;
    List<string> time;
    string cityname;
    int weather_code;
    string weather;
    int citynum;
    public int servoPin = 13;
    [Range(-100, 180)]
    public int servoAngle = 0;
    int prevServoAngle = 0;
    float tempangle;
    public float initialvalue = 0;
    float xvalueD;
    
    

    public LineChart lineChart;
    public GameObject face1;
    public GameObject face2;
    public GameObject raineffect;
    public GameObject snowffect;
    public GameObject cloudsffect;
    public TextMeshPro Citytext;
    public TextMeshPro Temptext;
    public TextMeshPro Weathetext;
    public GameObject pointer;
    void Start(){
        UduinoManager.Instance.pinMode(servoPin , PinMode.Servo);
        xvalueD =(float)20.541;
        PushData();
    }
    void Update(){
        if (servoAngle != prevServoAngle) 
        {
            UduinoManager.Instance.analogWrite(servoPin, servoAngle);
            prevServoAngle = servoAngle;
        }

    }
    private void Readvalue(){
        temp = GetAPI.temp;
        cityname = GetAPI.cityname;
        weather_code = GetAPI.weather_code;
        time = GetAPI.time;
        dailymaxtemp = GetAPI.dailymaxtemp;
        dailymintemp = GetAPI.dailymintemp;
    }
    public void PushData(){
        Readvalue();
        Citytext.text = cityname;
        Temptext.text = temp.ToString();
        weathertype(weather_code);
        changeface();
        tempangle = initialvalue +(float)temp;
        pointer.transform.localRotation = Quaternion.Euler(xvalueD,-180,0);
        pointer.transform.Rotate(new Vector3(0f,tempangle,0f),Space.Self);
        UduinoManager.Instance.sendCommand("tempdata", temp,citynum);

        Chart();
    }

    public void Chart(){
        lineChart.ClearData();
        for (int i = 0; i < time.Count; i++)
        {
          lineChart.AddXAxisData(time[i]);
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
    Weathetext.text =weather;
  }

  void changeface(){
    face1.SetActive(false);
    face2.SetActive(false);
    if(temp<0){
      face2.SetActive(true);
    }else{
      face1.SetActive(true);
    }
  }
}