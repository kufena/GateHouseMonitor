import os
import serial
import unicodedata
from datetime import datetime

def read_til_OK(ser: serial.Serial):
    results = []
    line = ser.readline()
    linestr = line.decode("ascii")
    while (not(linestr.startswith('OK') or (linestr.startswith('ERROR')))):
        results.append(linestr)
        line = ser.readline()
        linestr = line.decode("ascii")
    return dict([('status',not(linestr.startswith('ERROR'))), ('lines',results)])

def sendAT(ser: serial.Serial):
    ser.write(b'AT\r')
    res = read_til_OK(ser)
    return res['status']

#A FUNCTION TO SEND THE SMS
def sendSMS(ser: serial.Serial, telnum: bytes, txt: bytes):

    # ENDBYTES FOR SENDING THE SMS
    endbytes = bytearray()
    endbytes.append(0x1A)
    endbytes.append(0x1B)

    #CHECK STATUS - ALTHOUGH WE DON'T CHECK IT!
    if (sendAT(ser)):
        
        #SEND TEXT
        ser.write(b'AT+CMGS="')
        ser.write(telnum)
        ser.write(b'"\r\r')
        ser.write(txt);
        ser.write(b'\r');
        ser.write(endbytes)

        res = read_til_OK(ser)
        return res['status']
    else:
        return false