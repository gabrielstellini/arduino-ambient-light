# An affordable Arduino LED backlight solution

This project was created to make a backlight solution using RGB LEDs and an arduino on the windows platform. The solution will detect a display's width and height values, and calculate the average colour on the display.

### Requirements
- arduino
- non-digital rgb cable
- windows PC or Processing for code compilation

### Setup & Installation

1) Upload the .ino file to your arduino
2) Connect the pins as follows:

    | Colour        | Pin           | 
    | ------------- |:-------------:| 
    | Red      | 9 |
    | Green     | 10      | 
    | Blue | 11      |
    | Power | Power source    |

    For power I modified the original USB power that came with the LED strip


3) Download and run the provided compiled exe (or compile and run in the Processing environment). The program should show a box with the average colour detected on-screen

4) [Optional] Add the exe as a shortcut in the startup directory and hide the window

### Credits

 Rajarshi Roy and James Bruce for some of the code
