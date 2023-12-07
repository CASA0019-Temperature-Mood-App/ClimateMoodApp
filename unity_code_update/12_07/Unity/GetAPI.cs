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
using UnityEngine.SocialPlatforms;

public class GetAPI : MonoBehaviour
{
      public class Current
    {
        public string time { get; set; }
        public int interval { get; set; }
        public double temperature_2m { get; set; }
        public int weather_code { get; set; }
    }

    public class CurrentUnits
    {
        public string time { get; set; }
        public string interval { get; set; }
        public string temperature_2m { get; set; }
        public string weather_code { get; set; }
    }

    public class Daily
    {
        public List<string> time { get; set; }
        public List<int> weather_code { get; set; }
        public List<double> temperature_2m_max { get; set; }
        public List<double> temperature_2m_min { get; set; }
    }

    public class DailyUnits
    {
        public string time { get; set; }
        public string weather_code { get; set; }
        public string temperature_2m_max { get; set; }
        public string temperature_2m_min { get; set; }
    }

    public class Root
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double generationtime_ms { get; set; }
        public int utc_offset_seconds { get; set; }
        public string timezone { get; set; }
        public string timezone_abbreviation { get; set; }
        public double elevation { get; set; }
        public CurrentUnits current_units { get; set; }
        public Current current { get; set; }
        public DailyUnits daily_units { get; set; }
        public Daily daily { get; set; }
    }


  public static double temp;
  public static float localtemp;
  public static List<double> dailymaxtemp;
  public static List<double> dailymintemp;
  public static List<string> time;
  public static int weather_code;
  public static int citynum = 0;
  public static float xvalueD = (float)20.541;
  private TMP_Dropdown dropdown;
  private string URL;
  public static string cityname;
  public static bool camerafront;
  //GameObject zoomobject;

  void Awake(){
    UduinoManager.Instance.OnDataReceived += DataReceived;
  }
    void Start(){
        //UduinoManager.Instance.OnDataReceived += DataReceived;
        dropdown = GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(delegate{
        DropdownValueChanged(dropdown);
        });
        // zoomobject = findinactiveobjectName("zoombutton");
        
    }
    void Update(){
      //UduinoManager.Instance.OnDataReceived += DataReceived;
      //testwifi();
    }

    void DropdownValueChanged(TMP_Dropdown change){
      Debug.Log(change.value);
      switch(change.value){
        case 0:
        cityname="Local";
        citynum=0;
        pushLocal();
        break;
        case 1:
        URL="https://api.open-meteo.com/v1/forecast?latitude=51.541776123647395&longitude=-0.005828282697914817&current=temperature_2m,weather_code&daily=weather_code,temperature_2m_max,temperature_2m_min";
        cityname = "London";
        citynum=1;
        getdata();
        break;
        case 2:
        URL="https://api.open-meteo.com/v1/forecast?latitude=39.91002736793717&longitude=116.39694154457653&current=temperature_2m,weather_code&daily=weather_code,temperature_2m_max,temperature_2m_min";
        cityname = "Beijing";
        citynum=2;
        getdata();
        break;
        case 3:
        URL="https://api.open-meteo.com/v1/forecast?latitude=38.89697955298303&longitude=-77.03655378636529&current=temperature_2m,weather_code&daily=weather_code,temperature_2m_max,temperature_2m_min";
        cityname = "Washington";
        citynum=3;
        getdata();
        break;
        case 4:
        URL="https://api.open-meteo.com/v1/forecast?latitude=55.75320382676515&longitude=37.62040600688334&current=temperature_2m,weather_code&daily=weather_code,temperature_2m_max,temperature_2m_min";
        cityname = "Moscow";
        citynum=4;
        getdata();
        break;
        case 5:
        URL="https://api.open-meteo.com/v1/forecast?latitude=35.682093616671004&longitude=139.7668859932433&current=temperature_2m,weather_code&daily=weather_code,temperature_2m_max,temperature_2m_min";
        cityname = "Tokyo";
        citynum=5;
        getdata();
        break;
        default:
        break;

      }
    }

  private IEnumerator GetDatas(string Url,string cityname,float local){
    using(UnityWebRequest webRequest = UnityWebRequest.Get(Url)){
      yield return webRequest.SendWebRequest();
      if(webRequest.result == UnityWebRequest.Result.ConnectionError){
        Debug.LogError(webRequest.error);
      }else{
        var text =webRequest.downloadHandler.text;
        Root root = JsonConvert.DeserializeObject<Root>(text);
        temp = root.current.temperature_2m;
        dailymaxtemp = root.daily.temperature_2m_max;
        dailymintemp = root.daily.temperature_2m_min;
        time = root.daily.time;
        weather_code = root.current.weather_code;
        //UduinoManager.Instance.sendCommand("tempdata", temp,citynum);
        camerafront = changecamera.camerafront;
        if(camerafront){
          GameObject.FindWithTag("ARface").SendMessage("PushData");
        }else{
          GameObject.FindWithTag("ARimage").SendMessage("PushData");
        }
      }
    }
  }

  public void getdata(){
    StartCoroutine(GetDatas(URL,cityname,localtemp));
  }


  void ValueReceived(string value, string board) {
    Debug.Log(value);
  }

  void pushLocal(){
        temp = localtemp;
        //UduinoManager.Instance.sendCommand("tempdata", temp,citynum);
        camerafront = changecamera.camerafront;
        if(camerafront){
          GameObject.FindWithTag("ARface").SendMessage("pushlocal");
        }else{
          GameObject.FindWithTag("ARimage").SendMessage("pushlocal");
        }
  }

  void DataReceived(string data,UduinoDevice uduinoBoard){
    Debug.Log(data);
    localtemp=float.Parse(data);
    if(citynum==0){
      pushLocal();
      cityname="Local";
    }
  
  }

  // GameObject findinactiveobjectName(string name)
  // {
  //   Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
  //   for (int i = 0; i < objs.Length; i++)
  //   {
  //       if (objs[i].hideFlags == HideFlags.None)
  //       {
  //           if (objs[i].name == name)
  //           {
  //               return objs[i].gameObject;
  //           }
  //       }
  //   }
  //   return null;
  // }

}
