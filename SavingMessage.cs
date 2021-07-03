using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

//Class to control the message that appears in the scene to show the player whcih unit files have just been saved
public class SavingMessage : MonoBehaviour
{
    TextMeshProUGUI messageText = null;
    Animator anim = null;

    //Get references to text and animator, alert the user if either are missing
    //I considered using the RequireComponent() atribute to force having both components but I wanted to eb able to attach this to an existing UI object with the animator set up
    //and the attribute adds its own blank versions of the components to the object (which causes an error for the TMP Pro component)
    private void Awake()
    {
        try
        {
            messageText = GetComponent<TextMeshProUGUI>();
            anim = GetComponent<Animator>();
        }
        catch (NullReferenceException)
        {
            Debug.Log("One or both of the TMP and Animator components are missing from the SavingMessage object, SavingMessage cannot function without these components");
        }
    }

    //Build string to show which units have been saved, update the message text and start the fading animation
    public void SetSavingText(List<int> idList)
    {
        idList.Sort();
        string newText = "";
        //Need a slightly different string building function if there is only one unit being saved
        if(idList.Count < 2)
        {
            int id = idList[0] + 1;
            newText = "Unit " + id.ToString() + " Changes Saved";
        } 
        else
        {
            newText = "Units ";
            int i = 0, id = 0;
            for(i = 0; i < idList.Count - 1; i++)
            {
                id = idList[i] + 1;
                newText = newText + id.ToString() + ", ";
            }
            id = idList[i] + 1;
            newText = newText + "and " + id.ToString() + " Changes Saved";
        }
        messageText.text = newText;
        anim.SetTrigger("Fading");
    }
}
