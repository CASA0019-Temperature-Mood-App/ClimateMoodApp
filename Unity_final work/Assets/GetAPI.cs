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
        //Find dropdown button object
        dropdown = GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>();
        //Listen button event
        dropdown.onValueChanged.AddListener(delegate{
        DropdownValueChanged(dropdown);
        });
        
        
    }
    void Update(){
    }

    //set url
    void DropdownValueChanged(TMP_Dropdown change){
      Debug.Log(change.value);
      switch(change.value){
        case 0:
        URL="https://api.open-meteo.com/v1/forecast?latitude=51.51234365839393&longitude=-0.10980647263477246&current=temperature_2m,weather_code&daily=temperature_2m_max,temperature_2m_min";
        cityname = "London";
        citynum=0;
        getdata();
        break;
        case 1:
        URL="https://api.open-meteo.com/v1/forecast?latitude=43.65209752362991&longitude=-79.38314938362068&current=temperature_2m,weather_code&daily=temperature_2m_max,temperature_2m_min";
        cityname = "Toronto";
        citynum=1;
        getdata();
        break;
        case 2:
        URL="https://api.open-meteo.com/v1/forecast?latitude=25.177455444370608&longitude=55.25081250594853&current=temperature_2m,weather_code&daily=temperature_2m_max,temperature_2m_min";
        cityname = "Dubai";
        citynum=2;
        getdata();
        break;
        case 3:
        URL="https://api.open-meteo.com/v1/forecast?latitude=31.22365534951532&longitude=121.483771751325&current=temperature_2m,weather_code&daily=temperature_2m_max,temperature_2m_min";
        cityname = "Shanghai";
        citynum=3;
        getdata();
        break;
        case 4:
        URL="https://api.open-meteo.com/v1/forecast?latitude=-33.87731938974719&longitude=151.1708060262957&current=temperature_2m,weather_code&daily=temperature_2m_max,temperature_2m_min";
        cityname = "Sydney";
        citynum=4;
        getdata();
        break;
        case 5:
        cityname="Local";
        citynum=5;
        pushLocal();
        break;
        default:
        break;

      }
    }

  //send request to api
  private IEnumerator GetData(string Url,string cityname,float local){
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
    StartCoroutine(GetData(URL,cityname,localtemp));
  }


  void ValueReceived(string value, string board) {
    //Debug.Log(value);
  }

  void pushLocal(){
        temp = localtemp;
        camerafront = changecamera.camerafront;
        if(camerafront){
          GameObject.FindWithTag("ARface").SendMessage("pushlocal");
        }else{
          GameObject.FindWithTag("ARimage").SendMessage("pushlocal");
        }
  }

  //receive data from arduino
  void DataReceived(string data,UduinoDevice uduinoBoard){
    Debug.Log(data);
    localtemp=float.Parse(data);
    if(citynum==5){
      pushLocal();
      cityname="Local";
    }
  
  }



}
