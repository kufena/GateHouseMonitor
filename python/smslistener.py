import os
import serial
#import dns
#import dns.resolver
#import json
#import unicodedata
#from datetime import datetime
import atlib

ser = serial.Serial('Com18',115200)

ser.write(b'ATI\r');
res = atlib.read_til_OK(ser)

if (res['status']):
    for s in res['lines']:
      print(s)
else:
    print('didnt work properly');

atlib.sendSMS(ser, b'07515359541',b'all good here')
