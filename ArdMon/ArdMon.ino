/* ArdMon.ino
    Sketch that allows the Arduino to receive formatting and computer information
    to present on an Arduino compatible display.

    Written by William Schaffer
    Created: 9/30/2021
    Last Modified: 7/12/2022
*/

#include <Arduino.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1351.h>
#include <SPI.h>

// Declare some variables
String myStr;
bool strBool = false;
bool estCon = false;
int const labelMax = 10; // Arbitarily assigned maximum number of labels allowed to be assigned in display
int nOff = 10000; // Used to determine how many loops of a function until display is turned off
int labelCount; // Number of labels passed from program
int labelIndex; // Corresponds to the current value passed to Arduino

// Declare formatting variable arrays
String labelText[labelMax];
char labelDataType[labelMax]; // Not variable data type
String labelFontName[labelMax]; // To be replaced
float labelFontSize[labelMax]; // To be replaced
String labelColor[labelMax]; // Change to uint16_t or other variable later
int labelX[labelMax];
int labelY[labelMax];

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
  disp.setCursor(0, 64);

  // Wait for serial port to connect. Needed for native USB port only
  disp.print("Waiting for serial port");
  while (!Serial)
  {
    ;
  }

  // Recieve then send a synbol to establish contact with program
  disp.fillScreen(backColor);
  disp.setCursor(0, 64);
  disp.print("Waiting for contact\nwith computer program");
  delay(100);
  establishContact();
  disp.fillScreen(backColor);
}

void loop()
{
  if (Serial.available() > 0)
  {
    // Read string until '|'
    myStr = Serial.readStringUntil('|');

    // Check string for reset or disconnect
    checkString();

    // Check if display format is to be configured
    if (myStr.indexOf("config") >= 0)
    {
      configDisp();
      labelIndex = 0;
      return;
    }

    // Check if any labels have been established
    if (labelCount > 0)
    {
      // Reset screen if the current label is 0
      if (labelIndex == 0) disp.fillScreen(backColor);
      
      // Print each passed value
      valPrint(myStr, labelIndex);
      labelIndex++;
    }

    // Reset label index if it reaches the max
    if (labelIndex >= labelCount)
    {
      labelIndex = 0;
    }
  }
}

// Recieves information for display formatting
void configDisp()
{
  // Flush
  Serial.flush();

  // Send handshake character to computer
  Serial.println('*');

  // Declare needed variables for configuration
  int labelParam = 1;
  labelCount = 0;
  long unsigned colorTemp;

  // Clear all previously acquired configurations
  memset(labelText, 0, sizeof(labelText));
  memset(labelDataType, 0, sizeof(labelDataType)); // Not variable data type
  memset(labelFontName, 0, sizeof(labelFontName)); // To be replaced
  memset(labelFontSize, 0, sizeof(labelFontSize)); // To be replaced
  memset(labelColor, 0, sizeof(labelColor));
  memset(labelX, 0, sizeof(labelX));
  memset(labelY, 0, sizeof(labelY));

  // Loop
  while (labelCount < labelMax)
  {
    if (Serial.available() > 0)
    {
      // Read string until '|'
      myStr = Serial.readStringUntil('|');

      // Check string for reset or disconnect
      checkString();

      // Check if configuration is done
      if (myStr.indexOf("end") >= 0) break;

      // Store label information
      switch (labelParam)
      {
        case 1:
          labelText[labelCount] = myStr;
          labelParam++;
          break;
        case 2:
          labelDataType[labelCount] = myStr[0];
          labelParam++;
          break;
        case 3:
          labelFontName[labelCount] = myStr;
          labelParam++;
          break;
        case 4:
          labelFontSize[labelCount] = myStr.toFloat();
          labelParam++;
          break;
        case 5:
          // colorTemp = stroul(myStr, NULL, 16); Come back to this
          labelColor[labelCount] = myStr;
          labelParam++;
          break;
        case 6:
          labelX[labelCount] = myStr.toInt();
          labelParam++;
          break;
        case 7:
          labelY[labelCount] = myStr.toInt();
          labelParam = 1;
          labelCount++;
          break;
        default:
          break;
      }
    }
  }
}

// Function to attempt a elegant display printing solution to having varying component values and units, n is current label to be printed
void valPrint(String value, int n)
{
  // Apply label properties
  disp.setCursor(labelX[n], labelY[n]);
  disp.setTextColor(WHITE);

  // Get string ready to print
  String label = labelText[n];
  switch (labelDataType[n])
  {
    case 'l':
      label += value + '%';
      break;
    case 's':
      label += value + "GB";
      break;
    case 't':
      label += value + char(247) + "C";
      break;
    default:
      break;
  }

  // Print string
  disp.print(label);
}

// Check if arduino is disconnected from ACDD program or should reset
// May need to pass a bool in the future
void checkString()
{
  // Check if Arduino has been disconnected
  if (myStr.indexOf("disconnect") >= 0)
  {
    disp.setCursor(0, 64);
    disp.fillScreen(BLACK);
    disp.print("Disconnected");
    dispOffTimer();
  }
  else if (myStr.indexOf('@') >= 0)
  {
    resetFunc();
  }
}

// Waits for computer program to send symbol to establish contact and confirm correct program
void establishContact()
{
  int n = 0;

  while (!estCon)
  {
    n++;
    myStr = Serial.readString();
    if (myStr.indexOf('!') >= 0)
    {
      estCon = true;
      Serial.println('&');
    }
    checkString();
    if (n >= nOff) dispDisable();
  }
}

// Timer for turning the display off, may be removed if not used anywhere else
void dispOffTimer()
{
  int n = 0;

  while (dispOn && (n <= nOff))
  {
    myStr = Serial.readString();
    checkString();
    n++;
  }

  dispDisable();
}

// Sets the display to off
void dispDisable()
{
  dispOn = !dispOn;
  disp.enableDisplay(dispOn);

  // To be replaced, trap in a loop here
  while (true)
  {
    myStr = Serial.readString();
    checkString();
  }
}
