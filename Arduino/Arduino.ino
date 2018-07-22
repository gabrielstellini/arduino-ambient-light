const byte numChars = 32;
char receivedChars[numChars];

int RedPin = 9; //Red pin 9 has a PWM
int GreenPin = 10; //Green pin 10 has a PWM
int BluePin = 11; //Blue pin 11 has a PWM

char seperator = ',';

boolean newData = false;

void setup() {
    Serial.begin(2000000);
    Serial.println("<Arduino is ready>");

    pinMode(RedPin, OUTPUT);
    pinMode(GreenPin, OUTPUT);
    pinMode(BluePin, OUTPUT);
}

void loop() {
    recvWithStartEndMarkers();
    showNewData();
}

void recvWithStartEndMarkers() {
    static boolean recvInProgress = false;
    static byte ndx = 0;
    char startMarker = '<';
    char endMarker = '>';
    char rc;
 
    while (Serial.available() > 0 && newData == false) {
        rc = Serial.read();

        if (recvInProgress == true) {
            if (rc != endMarker) {
                receivedChars[ndx] = rc;
                ndx++;
                if (ndx >= numChars) {
                    ndx = numChars - 1;
                }
            }
            else {
                receivedChars[ndx] = '\0'; // terminate the string
                recvInProgress = false;
                ndx = 0;
                newData = true;
            }
        }

        else if (rc == startMarker) {
            recvInProgress = true;
        }
    }
}

void showNewData() {
    if (newData == true) {
//        Serial.print("This just in ... ");
//        Serial.println(receivedChars);
        newData = false;

        int red = 255;
        int blue = 255;
        int green = 255;


        
        sscanf(receivedChars, "%d,%d,%d", &red, &green, &blue);
        
        
        red = 255-red;
        green= 255-green;
        blue = 255-blue;

        analogWrite (RedPin, red);
        analogWrite (GreenPin, green);
        analogWrite (BluePin, blue);

//        Serial.print(red);
    }
}


