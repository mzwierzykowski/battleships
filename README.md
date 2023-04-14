# Warships

# Introduction
This repository contains game that enables user to play single sided Warships game against ships placed by the computer.
Repository contains two projects:
* warships-api is a backend application written in .NET 7 responsible for generating the board and tracking current game state,
* warships-client is web application written in React that allows user to interact with the game and is responsible for communication with backend server.

Board size and enemy fleet composition are driven by configuration 
Default values are 10x10 board and fleet containing:
* Battleship (size: 5 tiles, count: 1),
* Destroyer (size: 4 tiles, count: 2),

Which are reflected in appsettings.json sections of Warships.API project:
```json
  "BoardDimension": {
    "Width": 10,
    "Height": 10
  },
  "FleetConfiguration": {
    "Blueprints": [
      {
        "Type": "Battleship",
        "Size": 5,
        "Count": 1
      },
      {
        "Type": "Destroyer",
        "Size": 4,
        "Count": 2
      }
    ]
  }
```
By adding more objects to 'Blueprints' array we can introduce more types of ships to the game or change the existing ones.
Board size can also be adjusted and (hopefully :) ) should be reflected accordingly on the warships-client frontend application.

Warships.sln also contains couple Unit tests projects covering entire application logic.

# Running the game
There are two ways of running the game after clonning repository.

## Run as docker containers
Prerequisites:
* Docker desktop

Repository contains docker-compose.yml file which builds and sets up both applications as separate docker containers.
Docker desktop is required to use this approach.

After clonning the repository open you favorite terminal in the main repository folder and run command:
```
docker-compose up
```
This will build two images (warships_api and warships_client) and start a container with running application for each of them in your local docker instance.
After build is done visit:
```
http://localhost:3000
```
to start a new game.

## Run manually as self hosted applications
Prerequisites:
* .NET 7 SDK
* nodejs v16.14.2

### 1. Starting backend.
After clonning the repository open terminal in the main repository folder and navigate to:
```
cd warships-api/Warships.API
```
and then run:
```
dotnet run Warships.API.csproj
```
Optionally you can use Visual Studio 2022 to open Warships.sln solution, set Warships.API as a startup project and then start new debugging session to run the app.


### 2. Starting frontend
After clonning the repository open terminal in the main repository folder and navigate to:
```
cd warships-client
```
and run:
```
npm start
```
after successful build you can open your web browser and navigate to:
```
http://localhost:3000
```
to start a new game.
