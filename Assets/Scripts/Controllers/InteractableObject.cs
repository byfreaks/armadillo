using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public Interaction interaction;
    [HideInInspector]
    public InteractableController controller;

    public bool CanBeInteractedWith { get => controller.canBeInteractedWith; }

    public void Interact(InteractableActor actor){
        interaction.OnInteraction(actor);
    }
}
