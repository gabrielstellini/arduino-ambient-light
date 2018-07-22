# Ambient light using Arduino and Windows

An ambient light solution designed for Windows PCs.

### Requirements
- arduino
- non-digital rgb cable
- windows PC

### Setup & Installation

1) Upload the .ino file to your arduino
2) Connect the pins as follows:

    | Colour        | Pin           | 
    | ------------- |:-------------:| 
    | Red      | 9 |
    | Green     | 10      | 
    | Blue | 11      |
    | Power | Power source    |

    


3) Download and run the provided compiled exe (or compile and run in the Processing environment).

4) [Optional] Add the exe as a shortcut in the startup directory under the shell:startup directory

### Additional notes

The power provided by the arduino does not have enough amperage to drive the LEDs. The LEDs that were used for this project came with a cable which allowed control via a remote - this was modified to provide power to the LEDs.

The application also doesn't work with fullscreen directx games, as these connect directly to the gpu and stop the app from functioning. Similarly, the app doesn't work on the "run as admin screen" and lock screen.
