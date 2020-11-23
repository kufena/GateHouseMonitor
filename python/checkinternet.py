import os
import serial
import dns
import dns.resolver
import json
import unicodedata
from datetime import datetime

#unicodedata.normalize('NFKD', title).encode('ascii', 'ignore')
#READ THE DOMAIN AND TELEPHONE NUMBER FROM THE ENVIRONMENT.

domain = os.environ['CHECK_DOMAIN']
telephone = os.environ['CHECK_TELEPHONE']

#NORMALIZE TELEHPONE NUMBER SO THAT IT'S ASCII
telephone = unicodedata.normalize('NFKD', telephone).encode('ascii', 'ignore')

print(domain)
print(telephone)

# ENDBYTES FOR SENDING THE SMS
endbytes = bytearray()
endbytes.append(0x1A)
endbytes.append(0x1B)

#A FUNCTION TO SEND THE SMS
def sendSMS(telnum, txt):
  ser = serial.Serial("/dev/ttyUSB2",115200,timeout=float(2000))
  print(ser.name)

  #CHECK STATUS - ALTHOUGH WE DON'T CHECK IT!
  ser.write(b'AT\r')

  print(ser.readline())
  print(ser.readline())

  #SEND TEXT
  ser.write(b'AT+CMGS="')
  ser.write(telnum)
  ser.write(b'"\r\r')
  ser.write(txt);
  ser.write(b'\r');
  ser.write(endbytes)

  print(ser.readline())
  print(ser.readline())

  ser.close()

def myconverter(o):
    if isinstance(o, datetime):
        return o.__str__()

# OPEN AND READ THE JSON STATUS INFORMATION FROM THE LAST GO ROUND
f = open("/root/var/checkinternet/lock",'r');
lock = json.load(f)
print (lock)
f.close()

# RESOLVE OUR DNS ENQUIRY, CATCHING THE ERROR IF THERE IS ONE
error = False
ips = {}
try:
  ips = dns.resolver.resolve(domain,'A')
except:
  print ("Error from resolver")
  error = True

# DECISIONS DECISIONS
if (error or len(ips) == 0):
  # WE'VE ERRORED - ONLY SEND SMS IF THIS IS A CHANGE OF STATUS THOUGH
  if (lock['status'] != "bad"):
    print("No ips found")
    sendSMS(bytes(telephone), b'Failed SMS lookup, but we are still running.')
  lock['status'] = "bad"
  lock['ips'] = []
else:
  # WE WIN!  BUT IF THIS IS A CHANGE OF STATUS THEN SEND A TEXT
  if (lock['status'] == "bad"):
    sendSMS(bytes(telephone), b'SMS lookup on pi worked ok, so we are back baby!')
  lock['status'] = "ok"
  iplist = []
  for ipval in ips:
    ipvaltext = ipval.to_text()
    print(ipvaltext)
    iplist.append(ipvaltext)
  lock['ips'] = iplist

# UPDATE NUMBER OF GOES WE'VE DONE
lock['count'] = lock['count'] + 1
lock['timestamp'] = datetime.now()

# WRITE THE STATUS DATA.
f = open("/root/var/checkinternet/lock",'w');
json.dump(lock, f, default=myconverter)
f.close()
