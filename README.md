# Overview: 
[ADD ME]

# XshapeAPI
XshapeAPI is the API interface for my final year project in RP. It is a API server hosted on azure that is accessible to the web(No longer running). It allows for our companion apps to access the Microsoft Asure SQL cloud database. It also has the functionality to send emails when the stock is low.

## Thoughts and comments:
This really was one of the highlights for me in this project because it was the first time i am touching C# with no prior experience working with dotnet core framework. I would not have been able to do this without the support from my peers and lecturers.

One of the challenges i faced was due to how the database was set up. The database had a many-to-many(m2m) relationship. But this was an issue because i was not able to find a way to quary data with m2m relations using Entity Framework Core's LINQ query syntax. To solve this i had combined the LINQ query syntax with the LINQ method syntax just to be able to get m2m datas. which is like using a knife and a scissors to cut the same paper. but hey if it works it works. looking back at this with google and ai. it was probably due to how i define the sql tables and their relation that cause the issue. But as my first dot net core code i think it's a good learning experiance. 

Writing this about 2 years later because i realise that this repo does not have a readme, so some information might be off. Writing based on what i can remember

# deepstream_test5_app
This was my C200 special project module that was carried over to my final year project. Read more about it here: 
https://github.com/Jim-Chiew/Incorporate-MQTT-In-NVIDIADeepStreamSDK

# Demo Video: 
https://www.youtube.com/watch?v=vynn-RF62mg
