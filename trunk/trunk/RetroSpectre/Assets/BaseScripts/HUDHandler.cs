using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HUDHandler : MonoBehaviour
{
    public static HUDHandler HUD { get; private set; }

    public Transform HUDCamsParent;
    public Transform HUDItemsParent;

    public Camera[] HUDCams = new Camera[3];
    public Vector3[] HUDPos = new Vector3[3];
    public GameObject[] Inventory = new GameObject[3];

    // Start is called before the first frame update
    void Start()
    {
        if(HUD != null && HUD != this)
        {
            Destroy(this);
            return;
        }

        HUD = this;
        DontDestroyOnLoad(this);

        for(int i = 0; i < HUDCamsParent.childCount; ++i)
        {
            HUDCams[i] = HUDCamsParent.GetChild(i).GetComponent<Camera>();
        }

        for (int i = 0; i < HUDItemsParent.childCount; ++i)
        {
            HUDPos[i] = HUDItemsParent.GetChild(i).transform.position;
        }
    }

    public void AddItem(GameObject item)
    {
        for (int i = 0; i < Inventory.Length; ++i)
        {
            if( Inventory[i] != null )
            {
                continue;
            }
            else
            {
                Inventory[i] = item;
                item.transform.position = HUDPos[i];
                item.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                break;
            }
        }
    }
}
