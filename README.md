# My Singing Monsters Memory Game Solver
 
Only works on Windows
Currently just image detection.

How to use:

Put image in MSM memory game solver\bin\Debug\net6.0

Rename file to the filename of the image, include filetype at the end

Change screenWidth and screenHeight

Change the divisor in CompareImages() if you want.
Higher the number makes it faster, but less accurate.

You can also change the float value in 
ExhaustiveTemplateMatching etm = new ExhaustiveTemplateMatching(0.95f);
The float value represents the percentage of similarity to check.

Output is in MSM memory game solver\bin\Debug\net6.0\output