# TeaAsAService
Track your Brews!
So just for a change the other day at work we were discussing who's turn it was to make a brew. A discussion I'm sure goes on in businesses across the globe.
Being a Dev I started to wonder if anyone had ever thought about ways to track your daily brew intake, and who turn it was to make the next one. A quick google revealed nothing, so i thought it might be a fun idea to make one. 

Spec Proposal 

So the first simple step is RESTful API that with a couple of methods on it

HadBrew - Post to this when you have had a brew. If you send an identifier up to it then it will just increment your brew count for the day (reset at midnight), if you don't send an identifier it will create one and send it you back.

BrewedUp Post to this when someone gets a round in. Send it the round identifier and who made it and how many they made. If no round identifier  is send a new one will be created and sent back.

Brews - Get method you send an identifier  to that will give you back you current brew stats

NextRound - Get method to see who's been getting the round in over a time period.

Tech

So, as a mainly c# guy I was thinking of writing the API in netcore 2.2 c#. Not sure where to host it or what DB to use yet, need something fast and hopefully free.

Then the first client I was thinking of making was a command line app to run in WLS/Linux or power shell, call Tea++ that you just call every time you have a brew or pass an arg to when someone gets a round in

And that's kind of it. Hopefully I can make a start on actually writing it next week

All ideas welcome!

