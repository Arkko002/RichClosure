# RichClosure


## Description
RichClosure is a packet analyzing tool written in C# using the WPF framework and implements the MVVM design patter. It allows user to capture packets from a selected adapter, view the packet's detail including hex and ASCII values. The main element of the GUI is packet list, which can be filtered using the search function, or by the use of context menu which includes creating conversation filters.

#### Search function
The search function uses queries provided by the user in the search box on the top right of the GUI. Search class uses reflection to find the data provided by the user in the main packet list and compares it with values provided in the query according to the provided comparision operators. It also allows user to query few values at once with the use of logical operators AND and OR.

#### Suported packet types:
- IPv4
- IPv6
- ICMP
- TCP
- UDP
- DHCP
- DNS
- HTTP
- TLS

## Screenshots

![alt text](https://i.imgur.com/8wy0g4p.png) *Network adapter selection*


![alt text](https://i.imgur.com/eaL34Gq.png) *GUI*


![alt text](https://i.imgur.com/4YfofVD.png) *Searching the list*


![alt text](https://i.imgur.com/akT0jbv.png) *Quick filter context menu*


![alt text](https://i.imgur.com/lFvoozi.png) *Quick filter result*
