<p align="center">
    <img src="http://i.imgur.com/fwSB5ij.png" />
</p>

# ScreenGun

The free screen recorder for Windows, based on FFMPEG. [Click here to download!](https://github.com/jeffijoe/screengun/releases/download/0.1.1/ScreenGun.zip)

![Recorder View](http://i.imgur.com/re5glAH.jpg)

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