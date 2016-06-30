<p align="center">
    <img src="http://i.imgur.com/fwSB5ij.png" />
</p>

# ScreenGun

The free screen recorder for Windows, based on FFMPEG.

![Recorder View](http://i.imgur.com/re5glAH.jpg)

# Quickstart


* [Download it](https://github.com/jeffijoe/screengun/releases/download/0.1.1/ScreenGun.zip)
* Extract somewhere and run `ScreenGun.exe`
* Press "New Recording"
* Select the area you wish to record, amd press the **Record**:

  ![Record button](http://i.imgur.com/OY0iT1e.png)

* To stop recording, click on the ScreenGun icon in the system tray:

  ![Icon](http://i.imgur.com/UGRARNG.png)

# What is it?

ScreenGun is a very simple tool to make quick screen recordings in excellent quality at the expense of framerate.

**ScreenGun is suitable for**

* Quick demos
* Showing your parents how to send an email
* Small tutorials

**ScreenGun is NOT suitable for**

* Game recordings
* .. other things requiring a high framerate

# The techy stuff

ScreenGun uses Window's GDI+ to take screenshots as fast as it can (basically the same as `PrtScr`). It timestamps these frames internally and generates a textfile to be fed to the `ffmpeg` concat demuxer. For audio, ScreenGun uses NAudio to record the selected microphone. It also records the cursor.

# Author

Jeff Hansen - [@Jeffijoe](https://twitter.com/Jeffijoe)