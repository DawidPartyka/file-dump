import RPi.GPIO as GPIO
import time
import pigpio

GPIO.setmode(GPIO.BCM)
GPIO.setwarnings(False)

e = 2
x = 11
y = 9
hor = 2050
end = 2400

servo = pigpio.pi()
servo.set_servo_pulsewidth(e, hor)

time.sleep(2)
GPIO.setup(x,GPIO.OUT)
GPIO.setup(y,GPIO.OUT)

GPIO.output(x,GPIO.LOW)
GPIO.output(y,GPIO.LOW)

pulse = True

def pulselight(x):
    g = GPIO.LOW
    global pulse
    if pulse:
        g = GPIO.LOW
        pulse = False
    else:
        g = GPIO.HIGH
        pulse = True
    
    GPIO.output(x, g)
    
def changelight(x,y):
    GPIO.output(x, GPIO.HIGH)
    GPIO.output(y, GPIO.LOW)

up = True
s = 4
ang = hor
t = 1
while(True):
    if up:
        if ang < end:
            ang += s
            if t%5 == 0:
                print('zmiana światła')
                changelight(x, y)
                a = x
                x = y
                y = a
        else:
            changelight(11,9)
            time.sleep(3)
            servo.set_servo_pulsewidth(e, end)
            up = False
    else:
        GPIO.output(11, GPIO.LOW)
        if t%5 == 0:
            print('zmiana światła')
            pulselight(9)

        if ang > hor:
            ang -= s
        else:
            changelight(9,11)
            time.sleep(3)
            servo.set_servo_pulsewidth(e, hor)
            up = True
    
    servo.set_servo_pulsewidth(e, ang)
    
    t += 1
    if t > 200: t = 0
    
    time.sleep(0.15)
