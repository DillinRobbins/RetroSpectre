using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExampleInteractable : InteractableBase, IInteractable
{
    //This function is called when this interactable is interacted with.
    public override void VariableModifiers()
    {
        //Add your custom bools here-----------------------------------------------------
        
        VarStorage.SetValue("$hasCube", true);

        //-------------------------------------------------------------------------------
    }
}
