
#include <Arduino.h>
#include <WiFi.h>

const char* ssid = "Wifi_Name";
const char* password = "Password";
const uint16_t port = port_number;
const char* host = "hostIP";



void setup() {
  Serial.begin(9600);
  Serial.println("Connecting to Wi-Fi...");

  
  WiFi.begin(ssid, password);
  
  Serial.println("Connecting ...");
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(WiFi.status());
    Serial.print(".");
    
  }

  Serial.println("");
  Serial.print("WiFi connected with IP: ");
  Serial.println(WiFi.localIP());
}

void loop() {
  Serial.println("Connecting to server...");

  WiFiClient client;

  if (!client.connect(host, port)) {
    Serial.println("Connection to host failed");
    delay(800);
    return;
  }

  Serial.println("Connected to server!");

  delay(1000);
  client.println("Hello from ESP32!");

  Serial.println("Message sent to server.");

  delay(1000);
  
  String response = client.readString();
  Serial.print("Response from server: ");
  Serial.println(response);

  // You can add more processing here

  delay(5000);  // Adjust the delay based on your requirements
}
