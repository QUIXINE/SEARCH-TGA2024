# Rotation
1. Don't use this code to rotate clown's direction because whenget back to the same spot it will only rotate to stateManager.TargetPos.eulerAngles.y
   ``` 
   Quaternion rotation = Quaternion.Euler(0,stateManager.TargetPos.eulerAngles.y,0);
   stateManager.transform.rotation = Quaternion.Slerp(stateManager.transform.rotation, rotation, stateManager.RotationSpeed * Time.deltaTime);
   ```
2. Check rotation: [Link](https://discussions.unity.com/t/check-an-objects-rotation/42611) 

# Reloading Scene
1. If object is moved from one scene to another and the scene is reloaded that object will be deleted.

# Q&A
1. What does this code do? How does it work? 
   ```
   Quaternion rotation = Quaternion.LookRotation(dir);
   stateManager.transform.rotation = Quaternion.Slerp(stateManager.transform.rotation, rotation, stateManager.RotationSpeed * Time.deltaTime);
   Vector3 dir = stateManager.TargetPos.position - stateManager.transform.position;
   ```
