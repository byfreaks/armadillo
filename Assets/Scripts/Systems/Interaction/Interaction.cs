using System.Collections;
using System.Collections.Generic;

public abstract class Interaction
{
    public InteractableActor actor;
    public abstract void OnInteraction(InteractableActor actor);
}
