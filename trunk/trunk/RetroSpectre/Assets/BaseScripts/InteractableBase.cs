using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Yarn;
using Yarn.Unity;
using Yarn.Unity.Attributes;

public interface IInteractable
{
    public void Interact()
    {

    }

    public void Highlight()
    {

    }

    public void Unhighlight()
    {

    }
}

public class InteractableBase : MonoBehaviour, IInteractable
{
    public enum InteractionType {Item, NPC};

    [Header("Interactable Setup")]
    [Tooltip("Item interactions will be added to inventory.\nNPC interactions do not.")]
    public InteractionType interactionType;

    [Header("Optional to leave blank.")]
    [Tooltip("The Interactable NPC/Object that this object will \"take\".\nLeave blank if nothing does.")]
    public GameObject[] QuestItems = new GameObject[3];
    [Tooltip("Add the dialogue/text that should appear when interacting with this object.\nLeave blank for none.")]
    public DialogueReference DialogueYarnScript;
    [Tooltip("Check this box if you want to check for bools to be used later.")]

    [HideInInspector]
    public DialogueRunner DR;
    [HideInInspector]
    public InMemoryVariableStorage VarStorage;
    private Material[] HighlightedMaterials = new Material[2];
    private Material[] UnhighlightedMaterials = new Material[2];
    private string currentNode = "";

    public UnityEvent OnInteractedWith;
    public UnityEvent OnDialogueEnded;

    [Space(10)]
    [Header("--- DO NOT TOUCH BELOW ITEMS---")]
    public Material Outline;


    public virtual void VariableModifiers()
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

    //All functions below this point are not necessary to be touched unless you're getting fancy.
    async public virtual void Interact()
    {
        if(DialogueYarnScript.nodeName == "ItemGot")
        {
            VarStorage.SetValue("$itemName", gameObject.name);
            VarStorage.TryGetValue("$itemName", out string itemName);
        }

        if (DialogueYarnScript.nodeName != "")
        {
            await DR.StartDialogue(DialogueYarnScript.nodeName);
        }
        if (interactionType == InteractionType.Item)
        {
            HUDHandler.HUD.AddItem(gameObject);
        }

        if(OnInteractedWith != null)
        {
            OnInteractedWith.Invoke();
        }

        for (int i = 0; i < QuestItems.Length; i++)
        {
            if (QuestItems[i] != null && HUDHandler.HUD.Inventory.Contains(QuestItems[i]))
            {
                GameObject.Destroy(QuestItems[i]);
            }
        }

        VariableModifiers();
    }
    
    private void Awake()
    {
        HighlightedMaterials[0] = GetComponent<MeshRenderer>().material;
        HighlightedMaterials[1] = Outline;
        UnhighlightedMaterials[0] = GetComponent<MeshRenderer>().material;
        UnhighlightedMaterials[1] = GetComponent<MeshRenderer>().material;

        DR = FindFirstObjectByType<DialogueRunner>();
        VarStorage = FindFirstObjectByType<InMemoryVariableStorage>();
    }

    private void Update()
    {

        DR.onDialogueComplete.AddListener(onDialogueEnded);
        
        if(DR.Dialogue.CurrentNode != null)
            currentNode = DR.Dialogue.CurrentNode;
    }

    private void onDialogueEnded()
    {
        if(currentNode != "" && currentNode == DialogueYarnScript.nodeName)
            OnDialogueEnded?.Invoke();
    }

    public void Highlight()
    {
        gameObject.GetComponent<MeshRenderer>().materials = HighlightedMaterials;
    }
    
    public void Unhighlight()
    {
        gameObject.GetComponent<MeshRenderer>().materials = UnhighlightedMaterials;
    }

    public void DestroySelf()
    {
        GameObject.Destroy(gameObject);
    }
}
