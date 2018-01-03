# StrayResourceFinder
Tool used with GameMaker: Studio 1 to find unreferenced resources.

## Features
* Search starts at the first room, and then expanding as it finds more and more references, preventing self referencing objects from showing up
* Option to include all rooms if your game relies on `room_goto_next()`

## Limitations
* Currently does not read from a rooms creation code
* Can not detect scripts launched using `gml_pragma()`
* Does not look for fonts, timelines or paths.

Only tested on GameMaker: Studio 1 v1.99.551!
