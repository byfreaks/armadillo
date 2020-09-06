using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum InteractableType{
    Actor,
    Object
}

public class InteractableController : MonoBehaviour
{
    public InteractableType interactableType = InteractableType.Object;

    protected GameObject interactionArea;
    public Vector2 areaSize;
    public Vector2 areaOffset;

    private void Start() {
        switch(interactableType){
            case InteractableType.Actor: interactionArea = CreateActor(); break;
            case InteractableType.Object: interactionArea = CreateObject(); break;
        }
    }

    GameObject CreateActor(){
        var actorArea = SetupInteractionArea(out BoxCollider2D ibc, out Rigidbody2D irb); 
        ibc.size = areaSize;
        ibc.offset = areaOffset;
        actorArea.AddComponent<InteractableActor>().actorObject = this.gameObject;;
        return actorArea;
    }

    GameObject CreateObject(){
        var area = SetupInteractionArea(out BoxCollider2D ibc, out Rigidbody2D irb); 
        ibc.size = areaSize;
        ibc.offset = areaOffset;
        area.AddComponent<InteractableObject>();
        return area;
    }

    public InteractableObject GetObject(){
        return interactionArea.GetComponent<InteractableObject>();
    }

    GameObject SetupInteractionArea(out BoxCollider2D ibc, out Rigidbody2D irb){
        interactionArea = new GameObject("interactionArea");
        interactionArea.layer = (int)SystemLayer.Area;

        interactionArea.transform.SetParent(this.transform);
        interactionArea.transform.localPosition = Vector3.zero;
        
        ibc = interactionArea.AddComponent<BoxCollider2D>();
        ibc.isTrigger = true;

        irb = interactionArea.AddComponent<Rigidbody2D>();
        irb.bodyType = RigidbodyType2D.Kinematic;
        irb.useFullKinematicContacts = true;

        return interactionArea;
    }
}
