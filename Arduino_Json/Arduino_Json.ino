
#include "WiFi.h"
#include <ArduinoJson.h>
StaticJsonDocument<200> json_doc;
char json_output[100];
DeserializationError json_error;
const char* Name;
const char* Msg;
  
void setup() {
  Serial.begin(9600);
  //字串轉JSON
  json_doc["name"] = "Person01";
  json_doc["msg"] = "Hellow!";
  serializeJson(json_doc, json_output);
  Serial.println( "string to json:" ); 
  Serial.println( json_output ); 
 
  //JSON轉字串
  json_error = deserializeJson(json_doc, json_output);
  if (!json_error) {
    Name = json_doc["name"];
    Msg = json_doc["msg"];
    Serial.println( "json to string:" ); 
    Serial.println( "Name:" ); 
    Serial.println(Name);
    Serial.println( "msg:" ); 
    Serial.println(Msg);
  }
}
  
void loop() {
    
}
