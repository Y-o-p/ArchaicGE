# ArchaicGE
ArchaicGE is a game engine in C# that uses the console to create two dimensional and three dimensional [ASCII](https://en.wikipedia.org/wiki/ASCII) games.

## Getting Started
In the repository there are examples. ```Program3D.cs``` is the default example. There is also a 2D example if you uncomment ```Program2D.cs``` and comment out the other example.

## Features
* 3D rendering
* 2D rendering
* Mesh importer
* Input class(\*)
* Diffuse lighting

\* The current input class relies on the ```Console.ReadKey(bool)``` method which means that there is no way to detect if a key has been pressed, released, or held. There is no mouse input.

## License
This project is created under the MIT license; refer to [LICENSE](LICENSE) for more information.

## Acknowledgments
* [interl0per](https://github.com/interl0per/Console-Graphics) inspired me to create a three dimensional ASCII engine myself. The repository also helped me figure out how to change the console size and the fastest way to output to the console.
