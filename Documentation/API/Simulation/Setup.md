# Setup a Simulation

To use a simulation you first need to create a room for the simulation.
When creating a room also the first slot will be created. This first slot will have the highest possible rank as owner.
After requesting the creation of a new room the API will return an result containing a token to access the slot.

Every slot has one token that can be used to enter it, you can also use an userId and Password to enter a room.

## Creating Slots

For every person that should independently access a simulation you should create a slot. It is possible that the same person uses
one slot for more than one device. The API can work with multiple connections on the same slot but it may causes some bugs.

The devices all share the same access token.