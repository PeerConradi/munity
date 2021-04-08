# Setup a Simulation

To use a simulation you first need to create a room for the simulation.
When creating a room also the first slot will be created. This first slot will have the highest possible rank as owner.
After requesting the creation of a new room the API will return an result containing a token to access the slot.

Every slot has one token that can be used to enter it, you can also use an userId and Password to enter a room.

First check if the API you are talking to allows to create new Simulations by calling a GET-REquest on ```/api/Simulation/CanCreateSimulation```.
This will return a __string__ that contains ```true``` if it is possible to create new rooms or ```false``` if you cant create a new room.

## Create the simulation

To create a new simulation make a Post Request on ```/api/Simulation/CreateSimulation```. Containing the following json body:

```json
{
  "name": "My Simulation Name",
  "adminPassword": "MASTERPASSWORD",
  "userDisplayName": "Name of first Slot"
}
```

The __name__ parameter is the display Name of the simulation itself.

The __adminPassword__ can be used to access different functions later and will be need to delete the room manually.

The __userDisplayName__ is the DisplayName for the first Slot. Note that you will later be able to create Roles like Germany, Chair etc. so the
Displayname can be your Firstname or any other form of Tag.

Here is an example of the Result:

```json
{
  "simulationId": 123,
  "simulationName": "My Simulation Name",
  "firstUserId": "ahw28-KAwkd-12d2qd-kNwe2",
  "firstUserPassword": "1424",
  "firstUserToken": "Aj§jfkdApfkekIFKDF832FOLFe"
}
```

You should directly save the token as you need it for any other interaction with the API.


## Creating Slots

For every person that should independently access a simulation you should create a slot. It is possible that the same person uses
one slot for more than one device. The API can work with multiple connections on the same slot but it may causes some bugs.

The devices all share the same access token.


