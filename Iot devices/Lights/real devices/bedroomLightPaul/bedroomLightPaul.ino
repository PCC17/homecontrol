#include <ESP8266WiFi.h>
#include <WiFiUdp.h>


#define LightPin 4


const char* ssid     = "Camerloher2";
const char* password = "E7yLwS3S5J";

WiFiUDP Server;
unsigned int UDPPort = 51234;

const char server_ip[]="10.0.0.255";

const int packetSize = 7;
byte packetBuffer[packetSize];

const String turnOnSignal = "#0001";
const String turnOffSignal = "#0002";
const String sendIAmHereSiganl = "#0003";
const String serverCommand ="+iamh+";
const String stationName="Schlafzimmer Paul";



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
    Server.read(packetBuffer, packetSize);
    String data = ""; 
    for(int i = 0; i < packetSize; i++) {
      data += (char)packetBuffer[i];
    }
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
    if (data == sendIAmHereSiganl)
    {
      sendStationName();
      delay(1000);
      sendStationName();
    }
    else
    {
      char b[data.length()];
      data.toCharArray(b, data.length());
      Serial.println(data.length());
      if(b[1] == 'a')
      {
        String str= "";
        str += b[2];
        str += b[3];
        str += b[4];
        str += b[4];
        Serial.println(str);
        int val = str.toInt();
        analogWrite(LightPin, val);
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
