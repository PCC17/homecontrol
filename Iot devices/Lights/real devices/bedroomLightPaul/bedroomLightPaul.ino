#include <ESP8266WiFi.h>
#include <WiFiUdp.h>


#define LightPin 4


const char* ssid     = "Camerloher";
const char* password = "E7yLwS3S5J";

WiFiUDP Server;
unsigned int UDPPort = 51234;file:///d%3A/Cloud%20Storages/Nextcloud/Paul/Projekte/homecontrol/API/light/control-save.php

const char server_ip[]="10.0.0.255";



const String turnOnSignal = "10000";
const String turnOffSignal = "20000";
const String sendIAmHereSiganl = "0000000000";
const String serverCommand ="ima+";
const String stationName="Schlafzimmer-Paul";



boolean status = false;

void setup() {
  Serial.begin(115200);
  delay(10);

  Serial.println();
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);


  
  WiFi.begin(ssid, password); 
  
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  WiFi.mode(WIFI_STA);
  Serial.println("");
  Serial.println("WiFi connected");  
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());

  Server.begin(UDPPort);

  String ip_address = IPAddressToString(WiFi.localIP());
  Serial.println(ip_address);
  pinMode(LightPin, OUTPUT);

  for(int i = 0; i < 5; i++)
  {
    sendStationName();
    delay(1000);
  }
  digitalWrite(LightPin, LOW);
}

void loop() {
   handleUDPServer();
   delay(1);
}

void handleUDPServer() {
  int cb = Server.parsePacket();
 
  if (cb) {
    byte packetBuffer[cb];
    Server.read(packetBuffer, cb);
    String data = ""; 
    for(int i = 0; i < cb; i++) {
      data += (char)packetBuffer[i];
    }
    
    if (data == sendIAmHereSiganl)
    {
      sendStationName();
      delay(1000);
      sendStationName();
      delay(1000);
    }
    else
    {
      data = data.substring(5);
      Serial.println(data);
      
      if(data==turnOnSignal)
      {
        digitalWrite(LightPin, HIGH);
        status = true;
      }
      if(data==turnOffSignal)
      {
        digitalWrite(LightPin, LOW);
        status = false;
      }
      else
      {
        if(data.charAt(0) == '0')
        {
          String str= data.substring(1);
          Serial.println(str);
          int val = str.toInt();
          analogWrite(LightPin, val);
        }
      }
    }
    
  }
}

void sendStationName()
{
  
  Server.beginPacket(server_ip,UDPPort);
  String s = serverCommand+stationName;
  char copy[s.length()];
  s.toCharArray(copy, s.length()+1);
  Serial.println(s);
  Server.write(copy);
  Server.endPacket();
}


String IPAddressToString(IPAddress address)
{
 return String(address[0]) + "." + 
        String(address[1]) + "." + 
        String(address[2]) + "." + 
        String(address[3]);
}
