# Python dns resolve and error SMS service

This file runs on a Raspberry Pi 2 under cron, every minute.
It checks a domain for existence, and on error will send an SMS.

We use the serial port implementation and AT codes to drive a FONA 3G board,
which sends texts, makes calls and does some GPS stuff.  We just use it to
send a text message if the status changes from ok to bad or bad to ok.

To get this to work you need to set the environment variables CHECK_DOMAIN
and CHECK_TELEPHONE.
