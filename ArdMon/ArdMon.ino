// Declare some variables
String tempCPU = "";
String tempGPU = "";
String myStr = "";
char myChar = 0;
bool strBool = false;
bool estCon = false;
int n = 0; // Incrementing variable for turning off display
int nOff = 10000; // Used to determine how many loops of a function until display is turned off

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

// SSD1331 color definitions
#define BLACK           0x0000
#define BLUE            0x001F
#define RED             0xF800
#define GREEN           0x07E0
#define CYAN            0x07FF
#define MAGENTA         0xF81F
#define YELLOW          0xFFE0
#define WHITE           0xFFFF

// Colors to use
uint16_t textColor = WHITE;
uint16_t backColor = BLACK;

// Display
Adafruit_SSD1351 disp = Adafruit_SSD1351(SCREEN_WIDTH, SCREEN_HEIGHT, &SPI, CS_PIN, DC_PIN, RST_PIN);
bool dispOn = false;

void setup() {
  
  // Start serial port at 9600 bps
  Serial.begin(9600);
  delay(100);

  // Start display
  disp.begin();
  disp.setFont();
  disp.fillScreen(backColor);
  disp.setTextColor(textColor);
  disp.setTextSize(1);
  dispOn = true;

  // Cursor position for startup messages
  disp.setCursor(0,64);

  // Wait for serial port to connect. Needed for native USB port only
  disp.print("Waiting for serial port");
  while (!Serial) {
    ;
  }

  // Recieve then send a synbol to establish contact with program
  disp.fillScreen(backColor);
  disp.setCursor(0,64);
  disp.print("Waiting for contact\nwith computer program");
  delay(100);
  establishContact();
  disp.fillScreen(backColor);
  disp.setCursor(0,0);
}

void loop() {
  if (Serial.available() > 0) {

    // Read first character in buffer
    myChar = Serial.read();

    // Check if a reset is requested from computer
    checkReset();

    // Check if Arduino has been disconnected
    if (myChar == '_') {
      disp.setCursor(0,0);
      disp.fillScreen(BLACK);
      disp.print("Disconnected");
      dispOffTimer();
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
      disp.fillScreen(BLACK);
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


// Waits for computer program to send symbol to establish contact and confirm correct program
void establishContact() {
  while (!estCon) {
    n++;
    myChar = Serial.read();
    if (myChar == '!')
    {
      estCon = true;
      Serial.println('&');
    }
    checkReset();
    if (n >= nOff) dispDisable();
  }
  n = 0;
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
      disp.setCursor(cur1, cur2);
      disp.print(comp + "Temp" + char(58) + " " + val + char(247) + "C");
      break;
      
    default:
      break;
  }    
}

// Timer for turning the display off, may be removed if not used anywhere else
void dispOffTimer() {
  while (dispOn && (n <= nOff)) {
    myChar = Serial.read();
    checkReset();
    n++;
  }
  dispDisable();
}

// Sets the display to off
void dispDisable() {
  dispOn = !dispOn;
  disp.enableDisplay(dispOn);
  n = 0;
}
