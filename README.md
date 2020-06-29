# TeaAsAService
Track your Brews!
So just for a change the other day at work we were discussing who's turn it was to make a brew. A discussion I'm sure goes on in businesses across the globe.
Being a Dev I started to wonder if anyone had ever thought about ways to track your daily brew intake, and who turn it was to make the next one. A quick google revealed nothing, so i thought it might be a fun idea to make one. 

![.NET Core](https://github.com/beakersoft/TeaAsAService/workflows/.NET%20Core/badge.svg)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Spec Proposal 

So the first simple step is RESTful API that with a couple of methods on it

`HadBrew` - Post to this when you have had a brew. If you send an identifier up to it then it will just increment your brew count for the day (reset at midnight), if you don't send an identifier it will create one and send it you back.

`BrewedUp` Post to this when someone gets a round in. Send it the round identifier and who made it and how many they made. If no round identifier  is send a new one will be created and sent back.

`Brews` - Get method you send an identifier  to that will give you back you current brew stats

`NextRound` - Get method to see who's been getting the round in over a time period.

Tech

So, as a mainly c# guy I was thinking of writing the API in netcore 3.1 c#. The database back end is on MySql, need to work out a way to do the data migrations. Might do them manually in prod

Then the first client I was thinking of making was a command line app to run in WLS/Linux or power shell, call Tea++ that you just call every time you have a brew or pass an arg to when someone gets a round in

All ideas welcome!

# Dev Installation

First of all, clone the project as normal, 

git clone git@github.com:beakersoft/TeaAsAService.git

To run this project we need a MySql server running to host our database on. Luckly this is easy to create using a docker container. Open a terminal and from the build folder just run 

`docker-compose up -d`

to start a MySql docker instance. The connection string in the application should be setup to use this instance once its started, and the database will created on first load and seeded with a test user we can use to test the API

Test user id is 7EmMT6n3f0i/YniN6osJXQ== password TestPassword123*      

Once the container is running you can exec into it so you can run sql commands directly on the database. In the terminal run

`docker exec -t -i container_mysql_name /bin/bash`

Once you are in the container, run 

`mysql -uroot -pPassw0rd1*`

From there you can run standard MySql commands