import RPi.GPIO as GPIO
import time

red_pin = 4
green_pin = 6
blue_pin = 5

GPIO.setmode(GPIO.BCM) # BCM은 핀 번호 Board는 보드번호
GPIO.setup(red_pin, GPIO.OUT)
GPIO.setup(green_pin, GPIO.OUT)
GPIO.setup(blue_pin, GPIO.OUT)

try:
    while (True):
        GPIO.output(red_pin, False)
        GPIO.output(green_pin, False)
        GPIO.output(green_pin, False)
        time.sleep(0.5)
    
        GPIO.output(red_pin, False)
        GPIO.output(green_pin, True)
        GPIO.output(green_pin, False)
        time.sleep(0.5)

        GPIO.output(red_pin, False)
        GPIO.output(green_pin, False)
        GPIO.output(green_pin, True)
        time.sleep(0.5)

        GPIO.output(red_pin, True)
        GPIO.output(green_pin, True)
        GPIO.output(green_pin, True)
        time.sleep(0.5)

except KeyboardInterrupt:
    GPIO.cleanup()