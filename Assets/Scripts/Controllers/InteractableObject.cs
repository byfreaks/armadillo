using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public Interaction interaction;

    public void Interact(InteractableActor actor){
        interaction.OnInteraction(actor);
    }
}
