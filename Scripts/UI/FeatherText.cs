//\===========================================================================================
//\ File: FeatherText.cs
//\ Author: Morgan James
//\ Brief: Sets the text object that this script is attached to be equal to the amount of feathers the player has.
//\===========================================================================================

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FeatherText : MonoBehaviour {

    public void Update()
    {
        GetComponent<Text>().text = "Feathers: " + SaveManager.Instance.state.Feathers.ToString();
    }
}
