IDE:

The app was written in visual studio 2012, so easiest way to get it running (if you don't already have VS2012) will be to download the express version:
http://www.microsoft.com/visualstudio/eng/downloads#d-2012-express 
and choose “Visual Studio 2012 Express for Windows Desktop”.

Database:

The app uses entity framework to model the database.  All that’s required for database creation is SQL server express, which must be running before the app is started.

Before opening the solution in VS2012:

DELETE the following files and folders:
...\NameReader\packages
...\NameReader\NameReader\packages.config
...\NameReader\NameReaderTests\packages.config

Before pressing F5:

The solution uses Entity framework, HTMLAgility pack and NUnit.  In the VS2012 solution explorer, remove the references for Entity framework, HTMLAgilityPack
and packages.config from the NameReader project.  Also remove HTMLAgilityPack, nunit.framework and packages.config from NameReaderTests.
When all that is removed, use NuGet package manager to install Entity Framework and HTMLAgilityPack in the NameReader project and (if desired)
Nunit and HTMLAgilityPack in the NameReaderTests project.

The solution contains a file called “App_Resources.resx”, in this file are the entries for:
bing account key
bing news service URL
bing user name
open calais API key

Enter your bing account key, bing user name and open calais API key in the appropriate rows of the “Value” column.
