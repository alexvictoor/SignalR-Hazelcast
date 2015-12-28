# SignalR-Hazelcast
A simple SignalR backplane prototype relying on an [Hazelcast](https://hazelcast.com/) cluster.    
This backplane leverages on Hazelcast which brings scalability and resiliency. If you want to know more about Hazelcast and the recently opensourced Hazelcast C# client, check out [Hazelcast documentation](http://docs.hazelcast.org/docs/latest/manual/html/csharpclient.html).   
This backplane relies on Hazelcast 3.6-EA3 since Hazelcast 3.6 has not been released officially yet.  

## How to use this backplane
No nuget package deployed yet but you can build dthe backplane from sources quite easily. Check out the [samples](https://github.com/alexvictoor/SignalR-Hazelcast/blob/master/FirstSignalRSelfHost/Program.cs) for a configuration example.

## How to run the demo
This repository contains a demo application based on Owin, SignalR and obviously Hazelcast!   
To run it, first you need an Hazelcast cluster. Download Hazelcast 3.6-EA3 from [Hazelcast website](http://hazelcast.org/download/). Then the simplest way to start a 3 nodes cluster is to launch 3 times the following command line:

    java -jar hazelcast-3.6-EA3.jar
    
This will start 3 hazelcast nodes, with default configuration, listening for connections on port 5701, 5702 and 5703.    
Then you can start from visual studio the owin self hosted processes of the solution Sample folder. 
VisualStudio should open a browser window with URL [http://localhost:52682](http://localhost:52682/). This is a simple javascript client that connects to the first Owin process. Open URL [http://localhost:52682/index2.html](http://localhost:52682/index2.html) to get a second Javascript client, connected to the second owin process.  
That's all folks!

Once the application is running you can shutdown 1 or 2 hazelcast nodes, after a few seconds the owin processes should connect to the remaining node alive. 
