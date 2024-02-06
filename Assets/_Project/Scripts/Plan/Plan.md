# Problems
## Clown House
1. Puzzle 3
   - Ferris Wheel Problem_Box gets off without a trace
   - box sometime kinematic when on deliver lift. when press btn and the box is near, it makes the PusAndPull in PlayerController executes which make the box kinematic. 
PushANdPull only check when raycast hits obj that means when player is far from box and release R Ctrl, the kinematic is still true
2. Puzzle 4
   - still has no PlayerGetOnObj for crusher
3. Puzzle 5
   - if player jump the model will be over clamp head
   - messed up with ConveyorBelt.cs now can’t move wood along with it
4. Conveyor Belt
   - if OnTriggerExit(), IsMoving = false, every objs will stop
   - Problem: it assigns 1 obj to all 10, and if there are more than 1 obj, only 1 obj got assign to array
   - Problem: clown house puzzle 4, IsMoving = false while wood is on conveyor belt
5. PlayerController
   - PushAndPull can grab more than 1 obj
     - try check the the raycast along with the Vector.Dot (to check which side player faces to then the obj on that 
     side can be grabbed and ban another side), also play reaching animation



## Forest
1. Puzzle 7
   - make 2 wind zone, 1. for adding force to wood, 2. for stop velocity. (this method may look unnatural)
2. Puzzle 8
   - can’t do as requirement: can pass through doll, pull, push, but when doll is on water player can stand on it

# Plan
## Clown House
1. Puzzle 4 if use destructible obj, try to break it and make it back to normal shape
2. Puzzle 4
   - There are 3 boxes
   - **2 boxes will always be released**. 1st and 2nd will get out first. if any from both gets to another side, the 3rd boxes will replace it.
   - If the 2 boxes are in the pool, then reposition 1st box into the box releaser. 
   - After 1st box is repositioned for ...(2) sec., reposition 2nd box into the box releaser.
   - If the box falls down and collide with pool collider (isTriggered), rotates box to (0,0,0) and stop gravity
   - Put Position Checker (empty gameObj) that is used to compare position with the box that pass through it on the right side of the bridge
   - If any boxes passes through the Position Checker and other boxes are in the pool, reposition the 1st and 3rd boxes into the shaper
   - If a box collides with cutter machine, the box will be cut (to destructible obj).
   - After the box is cut, fades alpha of obj (if possible), then move it back to the pool (after it is broken or all animation is done)
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


# Solved
Clown House
- now, there has to be 2 btns for deliver lift because if not 2, player can fall down and die, right now has to find the way to notify  both 2 btns when any btn is pushed so that it won’t mess up like one btn still can be pushed while another can’t because if one btn is already pushed the another has to be disabled too (DeliverLiftButton.cs)
--> solved by access to another btn script then set up bool and target position of deliver lift in both instance