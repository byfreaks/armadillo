using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableActor : MonoBehaviour
{

    public enum ActorType{
        Player,
        NPC
    }

    public ActorType actorType;
    public GameObject actorObject;
    List<InteractableObject> objectsWithinRange = new List<InteractableObject>();
    InteractableObject closestObject;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.TryGetComponent<InteractableObject>(out var obj)){
            objectsWithinRange.Add(obj);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.TryGetComponent<InteractableObject>(out var obj)){
            objectsWithinRange.Remove(obj);
        }
    }

    private void Update() {
        
        float shortest = 999;

        foreach(InteractableObject obj in objectsWithinRange){
            var distance = Vector2.Distance(transform.position, (Vector2)obj.transform.position);

            if(distance < shortest){
                closestObject = obj;
                shortest = distance;
            }
        }

        if(InputController.MeleeAttack(ICActions.keyDown) && closestObject){
            closestObject.interaction.OnInteraction(this);
        }
    }

    private void OnDrawGizmos() {
        foreach(InteractableObject obj in objectsWithinRange){
            Debug.DrawRay(transform.position, (Vector2)obj.transform.position - (Vector2)transform.position, obj==closestObject ? Color.green : Color.red);
        }
    }

}