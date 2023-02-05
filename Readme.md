# MIDI Controller for Line6 DL4 MkII

This is an attemp to make the DL4 MkII a little bit more usable while playing.

With the unit on a pedalboard, it is a pain to try selecting delays, selecting reverbs is just completely impossible... Until soon!

## Introduction

This projects aims to deliver all the selection system of the DL4 MkII from any computer. This application follows the available MIDI commands shown in the [DL4 MkII Manual](https://line6.com/data/6/0a020a3f177ca62c4820b92c4e/application/pdf/DL4%20MkII%20Owner's%20Manual%20-%20English%20.pdf).

The application splits the delay and reverb selection systems allowing you to select each one without needing to navigate between complex menus.

<p align="center"><img src="/Build/MidiControl/Resources/Original/DL4MkII Control.gif" alt="DL4MkII CONTROL"></p>

## Features

 - MIDI channel control for choosing where the events from the panel are sent.
 - Divided reverb and delay sections allowing you to visualize the changes for both effects simultaneously.
 - Preset bypass by pressing the currently active footswitch like in the real unit.
 - Preset selection via a number text box for now, in the future it will be possible to label presets.
 - Local preset storing via serialized configuration structures on the computer.
 - Storing of the the current configuration by holding the illuminated footswitch like in the real unit.
 - Footswitch control to select between the three lowest presets for now.
