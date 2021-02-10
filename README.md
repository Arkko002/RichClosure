
RichClosure
===========
[![Build Status](https://travis-ci.com/Arkko002/RichClosure.svg?branch=master)](https://travis-ci.com/Arkko002/RichClosure)

Cross platform protocol analyzer written in C#.

## Table of Contents
1. [Table of Contents](#table-of-contents)
2. [Screenshots](#screenshots)
3. [Description](#description)   
4. [Getting Started](#getting-started)
    * [Prerequisites](#prerequisites)
    * [Installation](#installation)
5. [Usage](#usage)
6. [Documentation](#documentation)

## Screenshots
//TODO Style screenshots with CSS to make them smaller
![alt text](https://i.imgur.com/8wy0g4p.png) *Network adapter selection*


![alt text](https://i.imgur.com/eaL34Gq.png) *GUI*


![alt text](https://i.imgur.com/4YfofVD.png) *Searching the list*


![alt text](https://i.imgur.com/akT0jbv.png) *Quick filter context menu*


![alt text](https://i.imgur.com/lFvoozi.png) *Quick filter result*

## Description
RichClosure is a protocol analyzer written in C#. It comes with two GUI front-ends written in WPF and AvaloniaUI.
It provides the user with ability to sniff packets on selected network interfaces, view their headers and internal data,
and filter them based on their properties.

## Getting Started

### Prerequisites 

RichClosure uses raw sockets for packet sniffing, therefore it requires administrative privileges
to run.

### Installation

#### Building from source

You need .NET 5 SDK to build the source. After installing .NET 5 you can build the whole solution with

```dotnet build richClosure.sln```

The program can be then accessed through one of the GUI exes.

#### Docker

TODO

## Usage


## Documentation

TODO