using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class ExampleCollectQuestInteractable : InteractableBase, IInteractable
{
    [YarnCommand("GreyLaunch")]
    public void GreyLaunch()
    {
        gameObject.GetComponent<Animator>().Play("GreyLaunch");
    }

    //This function is called when this interactable is interacted with.
    public override void VariableModifiers()
    {
        //Add your custom bools here (multiple examples of code given below)---------

        /* Setting a simple bool into the Dialogue System's Variable Storage---------
            
            VarStorage.SetValue("$hasItem", true);

        */

        /* Getting an existing variable from Variable Storage and setting it---------
            
            VarStorage.TryGetValue("ItemsCollected", out int itemCollected);
            VarStorage.SetValue("ItemsCollected", itemCollected++);

        */

        //---------------------------------------------------------------------------
    }
}
