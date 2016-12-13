PLAY SERVICES DEPENDENCY 

GreedyGame SDK needs play services dependency to run. Specifically the following components.
1. play-services-ads
2. play-services-location

If your game already has a dependency with the above then you just need to import the unity package you should not use the 
play services library or the aars provided.


If your game doesn't already have a play services dependency, then you can opt one of the follwoing 3 methods to add it.


Using Library : 

If you are on Unity version less than 5, which doesn't support AAR libraries, then you should be using the library.
Just copy the library inside "current-sdk" > PlayServicesLibrary folder to Assets > Plugins > Android and you are good to go.

Using AAR :

If your unity version supports AAR libraries, then copy the AAR's provided inside current-sdk > PlayServicesAAR to a folder inside Assets > Plugins > Android folder. ( Make sure that you set the target platform for all AARs to Android ).

Using Google's JAR resolver plugin.

If you have other components of play services and if you think that copying the library or AAR can be an issue then you should use Google's JAR resolver. 

1. clone or download the repo in github. 
https://github.com/googlesamples/unity-jar-resolver

2. Import the asset package you can find in the root folder.

3. Create a new folder named "Editor" inside Assets > Plugins > GreedyGame. Copy the GreedyDependency.cs file inside this folder. 

Note : If you have other libraries with dependencies to play services you should create a dependency.cs file for those as well. For more info on how to create the dependency.cs script please refer the github repo.
