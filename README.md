A game for [Procedural Death Jam](http://proceduraldeathjam.com). It's a space game with randomly generated levels based off of a game I played as a child called Solar Jetman.

[Entry](http://proceduraldeathjam.com/pdj-entry/galaxy-explorer) yay! Has precompiled binaries for both OSX and Windows.

<b>Compile:</b><br>
To run you'll need the [Torque2D](https://github.com/GarageGames/Torque2D). For Windows/Mac there are projects for VS/XCode set up already. For Linux, I haven't tested it. Sorry if you're a Linux user but if I have time I'll figure it out and add the appropriate files. 

<b>Make sure to add LevelGen.cpp and LevelGen.h to the project.</b>

After it's compiled replace the <i>modules/</i> where the binary is with the one here. And replace main.cs with the one here. As of right now I don't think I changed anything but I might.

<b>Controls:</b><br>
W - move forward/engines<br>
A - turn left<br>
D - turn right<br>
space - shoot <br>
escape - pauses<br>
The rest is explained in game.

If the controls go crazy press escape to pause. I don't know if it's me or the engine but I don't have time to fix this.

<b>Random:</b><br>
I decided on Torque2D for this because there was only a week. It's easy to code something simple that would be a billion times more complicated in the other engine I usually work in. Anyway, I tried to make everything as obvious as possible in the scripts/C++ code. You should be able to modify things fairly easily. I haven't coded in this engine in well over a year so some of the things I'm doing probably aren't the way that they should be done. Probably even a year ago this was also true. 

The level generation is using automata under a certain rule set. If you google it you'll find very nice looking caves generated using a very similar, if not identical algorithm. I have no idea what I did differently from those people but even on the same scale this looks mostly like someone who played a very bad game of Tetris. 

I had a lot of grand ideas that I wanted to implement and started chopping off whole systems as the deadline approached. I also took A LOT of shortcuts towards the end. This code is a disaster so please don't think I always code like this. I know it's a mess. 

<b>Credits:</b><br>
I haven't finalized the very inconsistent and random art work I've found on the internet because I can't draw. Once I do I'll provide names/links to all of it. Most of it thus far was found on [OpenGameArt](opengameart.org). 

Music and sound effects are by [Solar Jetmen](solarjetmen.com). 

Thanks for playing!

