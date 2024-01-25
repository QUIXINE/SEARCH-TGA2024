# Problems
## Clown House
1. Puzzle 3
   - Ferris Wheel Problem_Box gets off without a trace
2. Puzzle 5
   - if player jump the model will be over clamp head 
3. Conveyor Belt
   - if OnTriggerExit(), IsMoving = false, every objs will stop
   - Problem: it assigns 1 obj to all 10, and if there are more than 1 obj, only 1 obj got assign to array
   - Problem: clown house puzzle 4, IsMoving = false while wood is on conveyor belt
4. PlayerController
   - PushAndPull can grab more than 1 obj
     - try check the the raycast along with the Vector.Dot (to check which side player faces to then the obj on that 
     side can be grabbed and ban another side), also play reaching animation
```c# Clown House's Puzzle 4 Woods Pool
//always spawn 3 woods, use 3 woods to push each other until the 1st wood gets to other side.
//the floor mechanic has no conveyor belt. put all woods in array, if 1st wood already got to another side then
//replace it with 4th wood

class Pool
{
    GameObj[] woods;
    float _nextTimeSpawn;   //time used to move wood to pos inside the releaser 1 by 1, used to
                            //wait for making a space between wood
    Update()
   {
        if(woods[1,2,3].position == transform.position)
       {
           if(_nextTimeSpawn <= ...)
           {
                if(woods[0].transform.position.x >= ...)//position is beyond conveyor belt
                {
                    woods[3].transform.position = ... //pos inside the releaser
                }
               else if (woods[0].transform.position == transform.position)
               {
                   woods[0].transform.position = ... //pos inside the releaser
               }
           }
           else if(_nextTimeSpawn <= ...)
           {
               woods[1].transform.position = ... //pos inside the releaser
           }
           else if(_nextTimeSpawn <= ...)
           {
               woods[2].transform.position = ... //pos inside the releaser
           }
       }
        
   
   }

}


```

## Forest
1. Puzzle 8
   - can’t do as requirement: can pass through doll, pull, push, but when doll is on water player can stand on it


# Plan
1. Puzzle 4 if use destructible obj, try to break it and make it back to normal shape
