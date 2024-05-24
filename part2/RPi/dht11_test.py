#dht11_test.py
import adafruit_dht
import time
import RPi.GPIO as GPIO
import board
log_num = 0
sensor_pin = 18

GPIO.setmode(GPIO.BCM)
GPIO.setup(sensor_pin, GPIO.IN)
dhtDevice = adafruit_dht.DHT11(board.D18)
while (True):
    try:
        temp = dhtDevice.temperature
        humid = dhtDevice.humidity
        print(f'{log_num} - Temp : {temp}c / Humid : {humid}%')
        log_num += 1
        time.sleep(2)
    except RuntimeError as ex:
        print(ex.args[0])
    except KeyboardInterrupt:
        break

dhtDevice.exit()