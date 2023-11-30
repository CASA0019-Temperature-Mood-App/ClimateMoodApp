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


public class servo : MonoBehaviour
{
    private Button button;

    private GameObject buttonactive;
    public GameObject face1;
    public GameObject face2;
    public GameObject effect1;

    public int servoPin = 13;
    [Range(-100, 180)]
    public int servoAngle = 0;
    private int prevServoAngle = 0;
    public int citynum = 1;
    private double temp;

    public TextMeshPro Citytext;
    //public GameObject Citytext;
    
    public TextMeshPro Temptext;
    //public GameObject Temptext;
    public TextMeshPro Weathetext;

    public GameObject pointer;
    
    public float initialvalue = 0;
    private float xvalueD;
    private float tempangle;

    private string URL;
    public LineChart lineChart;
    int count = 0;
    private string cityname;

    private string weather;





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






    void Start() {
        buttonactive = findinactiveobjectName("Buttoncity");
        buttonactive.gameObject.SetActive(true);
        UduinoManager.Instance.pinMode(servoPin , PinMode.Servo);
        //Citytext = GameObject.Find("city");
        //Temptext = GameObject.Find("temp");
        button = GameObject.Find("Button").GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        xvalueD =(float)20.541;
        cityname = "London";
        //URL = "https://api.open-meteo.com/v1/forecast?latitude=51.541776123647395&longitude=-0.005828282697914817&current=temperature_2m&forecast_days=1";
        URL="https://api.open-meteo.com/v1/forecast?latitude=51.541776123647395&longitude=-0.005828282697914817&current=temperature_2m,weather_code&daily=weather_code,temperature_2m_max,temperature_2m_min";
        refresh();
    }

    void Update()
    {
        if (servoAngle != prevServoAngle) 
        {
            UduinoManager.Instance.analogWrite(servoPin, servoAngle);
            prevServoAngle = servoAngle;
        }
    }

    void OnClick(){
		servochange();
	  }

    public void servochange(){
        citynum ++;
        if(citynum==6){
            citynum = 1;
        }
        Debug.Log(citynum);
        
          switch(citynum){
            case 1:
            servoAngle = 20;
            URL="https://api.open-meteo.com/v1/forecast?latitude=51.541776123647395&longitude=-0.005828282697914817&current=temperature_2m,weather_code&daily=weather_code,temperature_2m_max,temperature_2m_min";
            cityname = "London";
            refresh();
            Debug.Log(URL);
            break;
            case 2:
            servoAngle = 40;
            URL="https://api.open-meteo.com/v1/forecast?latitude=39.91002736793717&longitude=116.39694154457653&current=temperature_2m,weather_code&daily=weather_code,temperature_2m_max,temperature_2m_min";
            cityname = "Beijing";
            refresh();
            Debug.Log(URL);
            break;
            case 3:
            servoAngle = 60;
            URL="https://api.open-meteo.com/v1/forecast?latitude=38.89697955298303&longitude=-77.03655378636529&current=temperature_2m,weather_code&daily=weather_code,temperature_2m_max,temperature_2m_min";
            cityname = "Washington";
            refresh();
            Debug.Log(URL);
            break;
            case 4:
            servoAngle = 80;
            URL="https://api.open-meteo.com/v1/forecast?latitude=55.75320382676515&longitude=37.62040600688334&current=temperature_2m,weather_code&daily=weather_code,temperature_2m_max,temperature_2m_min";
            cityname = "Moscow";
            refresh();
            Debug.Log(URL);
            break;
            case 5:
            servoAngle = 100;
            URL="https://api.open-meteo.com/v1/forecast?latitude=35.682093616671004&longitude=139.7668859932433&current=temperature_2m,weather_code&daily=weather_code,temperature_2m_max,temperature_2m_min";
            cityname = "Tokyo";
            refresh();
            Debug.Log(URL);
            break;
            default:
            break;
        }
    }

  private IEnumerator GetDatas(string Url,string cityname){
    using(UnityWebRequest webRequest = UnityWebRequest.Get(Url)){
      yield return webRequest.SendWebRequest();
      if(webRequest.result == UnityWebRequest.Result.ConnectionError){
        Debug.LogError(webRequest.error);
      }else{
        //Root root= JsonMapper.ToObject<Root>(webRequest.downloadHandler.text);
        //double temp = root.current.temperature_2m;
        //Citytext.text = cityname;
        //Temptext.text = temp.ToString();
        //tempangle = -(0 +(float)temp);
        //pointer.transform.localRotation = Quaternion.Euler(0,tempangle,0);
        

        var text =webRequest.downloadHandler.text;
        Root root = JsonConvert.DeserializeObject<Root>(text);
        //Debug.Log(text);
        //double temp = root.current.temperature_2m;
        temp = root.current.temperature_2m;
        changeface();
        List<double> dailymaxtemp = root.daily.temperature_2m_max;
        List<double> dailymintemp = root.daily.temperature_2m_min;
        List<string> time = root.daily.time;
        Debug.Log(dailymaxtemp);
        int weather_code = root.current.weather_code;
        weathertype(weather_code);
        Citytext.text = cityname;
        Temptext.text = temp.ToString();
        //tempangle = -(initialvalue +(float)temp);
        tempangle = initialvalue +(float)temp;
        pointer.transform.localRotation = Quaternion.Euler(xvalueD,-180,0);
        pointer.transform.Rotate(new Vector3(0f,tempangle,0f),Space.Self);


        //lineChart.RemoveData();
        lineChart.ClearData();
        for (int i = 0; i < dailymaxtemp.Count; i++)
        {
          lineChart.AddXAxisData(time[i]);
          lineChart.AddData(0, dailymaxtemp[i]);
          lineChart.AddData(1, dailymintemp[i]);
        }
        
      }
    }
    
  }

  void refresh(){
    StartCoroutine(GetDatas(URL,cityname));
  }

  void weathertype(int Wcode){
    effect1.SetActive(false);
    if(Wcode==0){
      weather = "Clear sky";
    }else if(Wcode==1||Wcode==2||Wcode==3){
      weather="Partly cloudy";
    }else if(Wcode==45||Wcode==48){
      weather="Fog";
    }else if(Wcode==51||Wcode==53||Wcode==55||Wcode==56||Wcode==57){
      weather="Drizzle";
    }else if(Wcode==61||Wcode==63||Wcode==65||Wcode==66||Wcode==67||Wcode==80||Wcode==81||Wcode==82){
      weather="Rain";
      effect1.SetActive(true);
    }else if(Wcode==71||Wcode==73||Wcode==75||Wcode==77||Wcode==85||Wcode==86){
      weather="Snow";
    }else if(Wcode==95){
      weather="Thunderstorm";
    }else if(Wcode==96||Wcode==99){
      weather="Thunderstorm with slight";
    }
    Weathetext.text =weather;
    //Debug.Log(weather.ToString());
  }

  GameObject findinactiveobjectName(string name)
  {
    Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
    for (int i = 0; i < objs.Length; i++)
    {
        if (objs[i].hideFlags == HideFlags.None)
        {
            if (objs[i].name == name)
            {
                return objs[i].gameObject;
            }
        }
    }
    return null;
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

