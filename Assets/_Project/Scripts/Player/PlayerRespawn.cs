using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerRespawn : MonoBehaviour
{
       
    //Transform of spawning point
    public Transform RespawnPoint;
    
    [Header("Player Parent")]
    public Transform MyParent;
    private Transform _playerTrans;

    //IEnumerator will be called after ... sec. to spawn player and replay the game.
    //Reload the previous and further scenes 
    //Get scene info from the object that attacks player and reload the scene
    public IEnumerator Respawn(PlayerController playerController, PlayerTakeDamage playerTakeDamage, Scene sceneToLoadAsync)
    {
        //play black animation about ... sec. so that player won't see when camera is moving to player after they respawn back
        //
        //
        
        yield return new WaitForSecondsRealtime(5f);
        
        //move player back to the Player scene (PlayerForest, PlayerClownHouse) by moving them to be the child of an obj
        //inside Player scene so that Player gameObject won't be deleted after the sceneToLoadAsync is UnloadSceneAsync
        transform.parent = MyParent;
        transform.parent = null;
        
        //check sceneToLoadAsync != gameObject.scene because I to avoid loading scene from FallFromHeight, Collide wit Water condition
        if (sceneToLoadAsync != gameObject.scene)
        {
            if (sceneToLoadAsync != null && SceneManager.GetSceneByName(sceneToLoadAsync.name).isLoaded)
            {
                //check this because there's an error says scene to unload async is invalid
                SceneManager.UnloadSceneAsync(sceneToLoadAsync);
            }
            SceneManager.LoadSceneAsync(sceneToLoadAsync.name, LoadSceneMode.Additive);
        }
       
        //reposition player
        transform.position = RespawnPoint.position;
        
        //turn on the scripts, collider and set isKinematic to let player be able to move and take damage again
        playerController.enabled = true;
        playerTakeDamage.enabled = true;
        var collider = GetComponent<Collider>();
        collider.enabled = true;
        transform.GetComponent<Rigidbody>().isKinematic = false;
    }
    
    
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag(TagManager.RespawnPoint))
        {
            //get player's transform
            _playerTrans = transform;
            RespawnPoint = col.transform;
            RespawnPoint.position = transform.position;
        }
    }
    
}
