// Uduino settings
#include <Uduino_Wifi.h>
Uduino_Wifi uduino("uduinoBoard"); // Declare and name your object
#include <U8g2lib.h> // Add U8g2 library
#include <Servo.h>
#include <DHT.h>

#define DHTPIN 2  // The DHT22 data pin is GPIO2
#define DHTTYPE DHT22

DHT dht(DHTPIN, DHTTYPE);
U8G2_SH1106_128X64_NONAME_F_HW_I2C u8g2(U8G2_R0, /* reset=*/ U8X8_PIN_NONE);
Servo myservo;
float temperature;
float valueOne;
int valueTwo;

void setup()
{
  Serial.begin(9600);
  uduino.addCommand("tempdata", getdata);
  dht.begin();
  u8g2.begin();
  u8g2.setFont(u8g2_font_ncenB08_tr);
  myservo.attach(0);
  

  // Optional function,  to add BEFORE connectWifi(...)
  uduino.setPort(4222);   // default 4222

  uduino.useSendBuffer(true); // default true
  uduino.setConnectionTries(35); // default 35
  uduino.useSerial(true); // default is true

  // mendatory function
  uduino.connectWifi("CE-Hub-Student", "casa-ce-gagarin-public-service");
  //uduino.connectWifi("mate40pro", "88888888");
  //uduino.connectWifi("Chacewifi", "88888888");
}

void loop()
{
  uduino.update();

  if (uduino.isConnected()) {
    float temperature = dht.readTemperature();
    uduino.println(temperature);
    uduino.delay(5000);
  }else{
    float temperature = dht.readTemperature();
    String tempStr = String(temperature) + " C";
    u8g2.clearBuffer();
    u8g2.drawStr(50, 20, "Local");
    u8g2.drawStr(50, 40, tempStr.c_str());
    u8g2.sendBuffer();
     if (temperature >= -15 && temperature < 0) {
     myservo.write(199);
  } else if (temperature >= 0 && temperature < 15) {
     myservo.write(135);
  } else if (temperature >= 15 && temperature < 30) {
     myservo.write(75);
  } else if (temperature >= 30 && temperature <= 45) {
     myservo.write(30);
  } 
    uduino.println(0);
    uduino.delay(5000);
  }
}

void getdata(){
  int parameters = uduino.getNumberOfParameters(); 
  if(parameters > 0) {
  valueOne = uduino.charToInt(uduino.getParameter(0)); 
  valueTwo = uduino.charToInt(uduino.getParameter(1)); 
  }
  u8g2.clearBuffer();
  String tempStr = String(valueOne) + " C";
    switch(valueTwo){
    case 0:
    u8g2.drawStr(50, 20, "London");
    break;
    case 1:
    u8g2.drawStr(50, 20, "Toronto");
    break;
    case 2:
    u8g2.drawStr(50, 20, "Dubai");
    break;
    case 3:
    u8g2.drawStr(50, 20, "Shanghai");
    break;
    case 4:
    u8g2.drawStr(50, 20, "Sydney");
    break;
    case 5:
    u8g2.drawStr(50, 20, "Local");
    break;
    default:
    break;
  }
  u8g2.drawStr(50, 40, tempStr.c_str());  // Write string
  u8g2.sendBuffer();
  if (valueOne >= -15 && valueOne < 0) {
     myservo.write(199); 
  } else if (valueOne >= 0 && valueOne < 15) {
     myservo.write(135); 
  } else if (valueOne >= 15 && valueOne < 30) {
     myservo.write(75); 
  } else if (valueOne >= 30 && valueOne <= 45) {
     myservo.write(30); 
  } 
}
