// Declare some variables
String tempCPU = "";
String tempGPU = "";
String myStr = "";
char myChar = 0;
bool strBool = false;
bool estCon = false;

// Declare reset function
void(* resetFunc) (void) = 0;

// Screen dimensions
#define SCREEN_WIDTH  128
#define SCREEN_HEIGHT 128

// ~ You can use any (4 or) 5 pins
// Pin numbers changed from original to match recommended from manufacturer
#define SCLK_PIN 13
#define MOSI_PIN 11
#define DC_PIN   7
#define CS_PIN   10
#define RST_PIN  8

#include <Adafruit_GFX.h>
#include <Adafruit_SSD1351.h>
#include <SPI.h>

#define BLACK           0x0000
#define WHITE           0xFFFF

// Display
Adafruit_SSD1351 tft = Adafruit_SSD1351(SCREEN_WIDTH, SCREEN_HEIGHT, &SPI, CS_PIN, DC_PIN, RST_PIN);

void setup() {
  
  // Start serial port at 9600 bps
  Serial.begin(9600);
  delay(100);

  // Start display
  tft.begin();
  delay(100);
  tft.fillScreen(BLACK);

  // Wait for serial port to connect. Needed for native USB port only
  tft.print("Waiting for serial port");
  while (!Serial) {
    ;
  }

  // Recieve then send a synbol to establish contact with program
  tft.fillScreen(BLACK);
  tft.setCursor(0,0);
  tft.print("Waiting for contact\nwith computer program");
  delay(100);
  establishContact();
  tft.fillScreen(BLACK);
  tft.setCursor(0,0);
}

void loop() {
  if (Serial.available() > 0) {

    // Read first character in buffer
    myChar = Serial.read();

    // Check if a reset is requested from computer
    checkReset();

    // Check if Arduino has been disconnected
    if (myChar == '_') {
      tft.setCursor(0,0);
      tft.fillScreen(BLACK);
      tft.print("Disconnected");
    }
    
    // Check if last value is collected (must be BEFORE adding myChar to myStr to keep > out of myStr)
    if (myChar == '>') {
      valPrint(myStr, 't', "GPU ", 0, 10);
      myStr = "";
      strBool = false;
      return;
    }

    // Check if first value is collected (must be BEFORE adding myChar to myStr to keep ) out of myStr)
    if (myChar == ')') {
      tft.fillScreen(BLACK);
      valPrint(myStr, 't', "CPU ", 0, 0);
      myStr = "";
      return;
    }

    // Check if adding characters to string (must be HERE)
    if (strBool) {
      myStr += myChar;
      return;
    }

    // Check if beginning of string (must be AFTER adding myChar to myStr to string to keep < out of myStr)
    if (myChar == '<') {
      strBool = true;
    }
  }
  
  /*
  if ((myChar != '0') && (!Serial.available())) {
    Serial.println(myChar); //Print received string to Computer
    myChar = '0';
  }
  */
}

void establishContact() {
  while (!estCon) {
    myChar = Serial.read();
    if (myChar == '!')
    {
      estCon = true;
      Serial.println('&');
    }
    checkReset();
  }
}

void checkReset() {
  if (myChar == '@') {
    resetFunc();
  }
}

// Function to attempt a elegant display printing solution to having varying component values and units
// Valid param characters: t, f, p
void valPrint(String val, char param, String comp, int cur1, int cur2) {
  switch (param) {
    case 't':
      tft.setCursor(cur1, cur2);
      tft.print(comp + "Temp" + char(58) + " " + val + char(247) + "C");
      break;
      
    default:
      break;
  }    
}
