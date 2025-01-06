Here's the translated README.md file:

---

# bridgerv2  
A cool logic game where you skillfully build bridges to win.

## How the Gameplay Works  
The game board consists of piers. Each player, Red and Blue, has 42 piers each.  
![Approach2](https://github.com/user-attachments/assets/8e3091a1-dd24-409b-acaa-de1cf77c1ad8)  

Green controls the two outermost columns, while Red owns the two outermost rows of the matrix.  
The goal for Green is to connect the top and bottom columns in a straight line using bridges. Red, on the other hand, aims to create a continuous connection between the two outermost rows.  

Bridges can only be placed on the white cells. These bridges are connected to the piers, forming a pathway. When one player's connection is complete, that player wins.  

In theory, it might look something like this (note: some early considerations for AI are included):  
![Rectangle](https://github.com/user-attachments/assets/86819cc5-8b2b-4b95-a01d-c8f11f4fab12)  

Now, let's discuss the game mechanics.  
The challenge lies in developing an algorithm to detect whether a player has won and another algorithm capable of playing on par with a human (while also being time-efficient). Below is a messy compilation of all considerations in a single image:  
![Approach](https://github.com/user-attachments/assets/7f9d8bdf-07e2-4e1d-a9b7-e6f3a793bba7)  

This can also be represented as a matrix:  
```
- 1 - 1 - 1 - 1 - 1 - 
2 0 2 0 2 0 2 0 2 0 2 
- 1 0 1 0 1 0 1 0 1 - 
2 0 2 0 2 0 2 0 2 0 2 
- 1 0 1 0 1 0 1 0 1 - 
2 0 2 0 2 0 2 0 2 0 2 
- 1 0 1 0 1 0 1 0 1 - 
2 0 2 0 2 0 2 0 2 0 2 
- 1 2 1 0 1 0 1 0 1 - 
2 0 2 0 2 0 2 0 2 0 2 
- 1 0 1 0 1 0 1 0 1 - 
2 2 2 2 2 0 2 0 2 0 2 
- 1 - 1 - 1 - 1 - 1 - 
```  

0 = empty cell / unoccupied field / space for bridges  
1 = Green player  
2 = Red player / AI  
– = theoretically never occupied  

---
