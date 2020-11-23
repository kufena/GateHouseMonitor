# Raspberry Pi Network and House Monitor.

This project implements monitor for my house which checks an important IP address, and takes the temperature using an MP9808 device over I2C.
The result of this is sent to an AWS lambda function which stores the result in an S3 bucket with an object name bearing the date of the observation.
It should do this every 15 minutes.

## GateHouseModel

A tiny shared project for JSON compatibility.

## GateHouseMonitor

Intended to run on a Raspberry Pi 2 under Ubuntu 20.04.
At the moment, it will check an important IP address via a DNS lookup, and then take the temperature via the device.
This is written using C# and .NET 5.0 which I managed to install ok using the Arm32 binaries.
Pete Gallagher has a write up of how to install it here:

  https://www.petecodes.co.uk/install-and-use-microsoft-dot-net-5-with-the-raspberry-pi/

## GateHouseLambda

An AWS Lambda behind an API gateway.  Use a POST to send a JSON object containing the model.
At the moment, it requires a {type} parameter, and the only one it responds to is GateHouseMonitor.
Uses SAM/Cloudformation to deploy.

## python

This is a little bit of cron job python that does much the same as the monitor - checks a domain
name (no temperature stuff) but it then simply logs a status to a file.  The clever bit is that
should the status change, ok to bad, or bad to ok, it will send an SMS to a number given via
an environment variable, using AT commands to the FONA 3G device.

## To Come

- I hope to extend the lambda to take a different set of data from a things network application.

- Add logic to call the 'reset' on the router should the internet go down.
