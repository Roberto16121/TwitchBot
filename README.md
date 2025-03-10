# TwitchBot
<a id="readme-top"></a>


<!-- PROJECT LOGO -->
<br />
<div align="center">

  <h3 align="center">Twitch Bot</h3>

  <p align="center">
    An easy to use Twitch Bot application
  </p>
</div>



<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

This is my own vision of what a Twitch Bot application should be and I wanted to creat this project to better my coding skills and to enhance my streaming experince.

Why did I make this application:
* The interface should be simple without eye catching designs
* You should have total control on what do Modules and Commands do


This is an outgoing project and it doesn't represent the final product

<p align="right">(<a href="#readme-top">back to top</a>)</p>



### Built With


* .NET 8.0
* WPF
* SQLite

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- GETTING STARTED -->
## Getting Started

How to get the app up and running :

### Prerequisites

* .NET 8.0
* OBS Studio (if you want to use the WebSocket integration)
* Twitch Account

### Setup

Configure TwitchAPI:
* Obtain your Access Token, Client Id from [Twitch Token Generator](https://twitchtokengenerator.com/)
* Obtain your User Id from [User Id](https://www.streamweasels.com/tools/convert-twitch-username-%20to-user-id/)

Update the TwitchConfig.json
```
{
  "Username": "YourTwitchUsername",
  "AccessToken": "YourAccessTokenHere",
  "ClientId": "YourClientIDHere",
  "BroadcasterId": "YourUserIDHere"
}
```

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- USAGE EXAMPLES -->
## Usage

Module Tab : 
* Create a new Module
* Set the properties as needed
FOR OBS ACTION :
OBS Setup
* In OBS create a new Scene named : TwitchBot
* Add all of your media (Media Sources) and Hide them
* Add the TwitchBot Scene in your main Scenes
OBS ACTION Setup:
* Set the Source name as the media you want to play from the TwitchBot Scene
* Set the Duration (how much should it be playing) in seconds

* Statistics : (To be developed)
* Settings : (To be developed)

  ![](https://github.com/Roberto16121/TwitchBot/blob/master/_Github/Demonstation.gif)

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- ROADMAP -->
## Roadmap
- [x] Implement Modules (actions based on keywords)
  - [x] Create Sound Module (play a specific sound)
  - [x] Create Obs Module (show and hide Sources from Obs)
- [ ] Implement Statistics
  - [x] Create User based statistics (messages sent, view time, etc)
  - [x] Create Module based statistics (Used count, most used by, etc)
  - [ ] Implement Sorting
  - [ ] Implement Filters
- [ ] Implement Commands (similar to modules but used for moderation and twitch interactions)
  - [ ] Implement reply option
  - [ ] Implement song request queue
- [ ] Revamp Cooldown
  - [ ] Cooldown between Modules being used
  - [ ] Add a Queue for Modules to be played (user set dimention)
- [ ] Add Timer Events
- [ ] Implement Settings
    - [ ] Section for OBS WebSocket properties and Scene Name
    - [ ] Section for miscellaneous (list of users to ignore in the Viewer in Chat, etc)


<p align="right">(<a href="#readme-top">back to top</a>)</p>

