### Overview
This tutorial project shows you how to download user's message attachments and save them to a local machine.

### RingCentral Connect Platform
RingCentral Developer Platform is a rich RESTful API platform with more than 70 APIs for business communication includes advanced voice calls, chat messaging, SMS/MMS and Fax.

### RingCentral Developer Portal
To setup a free developer account, click [https://developer/ringcentral.com](here)

### Clone - Setup - Run the project
```
$ git clone https://github.com/ringcentral-tutorials/message-store-download-content-csharp-demo
```

Open the "message-store-download-content-csharp-demo.sln" project with Microsoft Visual Studio IDE.

### Create an app
* Create an application at https://developer.ringcentral.com.
* Select `Server-only (No UI)` for the Platform type.
* Add the `ReadMessages` permission for the app.
* Copy the Client id and Client secret and add them to the `Program.cs` file.
```
RINGCENTRAL_CLIENTID=RINGCENTRAL_CLIENTSECRET=
```
* Add the account login credentials to the `Program.cs` file.
```
RINGCENTRAL_USERNAME=
RINGCENTRAL_PASSWORD=
RINGCENTRAL_EXTENSION=
```

### Make sure you have some content
* Login your RingCentral account from RingCentral soft-phone.
* Send a few MMS message to your RingCentral number.
* Make a few phone calls to your RingCentral number and leave voice messages.
* Send a few fax messages to your RingCentral number.

### Run the demo
Run the demo app from Visual Studio

### RingCentral Developer Portal
To setup a free developer account, click [here](https://developer/ringcentral.com)

## RingCentral .Net SDK
The SDK is available at https://github.com/ringcentral/ringcentral.net
