//\===========================================================================================
//\ File: Shapes.cs
//\ Author: Lily Raeburn for the research into deep copying and applying it to everything on the player but the shape.
//\ Author: Morgan James for the bit manipulation to do with the save state of the model.
//\ Brief: Changes the background in accordance with the shop and unlocks.
//\===========================================================================================

using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

public class Shapes : MonoBehaviour
{
	private int currentModelIndex;

	[Header("Player")]
	[SerializeField]
	private GameObject player;

    [Header("Player Models")]
	[SerializeField]
    private GameObject[] playerModels;

    IEnumerator Start()
    {
        //Wait for the save manager instance to load.
        while (SaveManager.Instance == null)
        {
            yield return null;
        }

        //Set the player model.
        UpdatePlayerModel(SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedShape));

		//Set the new index number to check against.
		currentModelIndex = SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedShape);
	}

	public void Update()
	{
		//Checks if a new shape is selected.
		if (currentModelIndex != SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedShape))
		{
			//Sets the shape to the newly selected one.
			UpdatePlayerModel(SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedShape));

			//Set the new index number to check against.
			currentModelIndex = SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedShape);
		}
	}

    public void UpdatePlayerModel(int a_iModelIndex)
    {
		if (playerModels.Length > 0 && a_iModelIndex < playerModels.Length)
        {
            //Changed player model.
            GameObject newModel = playerModels[a_iModelIndex];

			//Copy the new sprite renderer.
			player.GetComponent<SpriteRenderer>().GetCopyOf(newModel.GetComponent<SpriteRenderer>());

			//Resetting the sorting layer since the deep copy doesn't catch it for some reason.
			player.GetComponent<SpriteRenderer>().sortingLayerName = "Player";

            //Remove the collider component and sprite renderer.
            RemoveCollider();

            //If the new component has a polygon collider copy it to th player.
            //and do the same for the other two types of colliders we are using.
            if (newModel.GetComponent<PolygonCollider2D>() != null)
            {
				player.AddComponent<PolygonCollider2D>(newModel.GetComponent<PolygonCollider2D>());
            }
            else if (newModel.GetComponent<BoxCollider2D>() != null)
            {
				player.AddComponent<BoxCollider2D>(newModel.GetComponent<BoxCollider2D>());
            }
            else
            {
				player.AddComponent<CircleCollider2D>(newModel.GetComponent<CircleCollider2D>());
            }
		}
    }

    //Removing the collision components and sprite renderer from the player.
    private void RemoveCollider()
    {
        Destroy(player.GetComponent<PolygonCollider2D>());
        Destroy(player.GetComponent<BoxCollider2D>());
        Destroy(player.GetComponent<CircleCollider2D>());
    }
}

public static class ExtensionMethods
{
    // Attempting to perform a 'deep copy' of one component on a prefab to the player at runtime
    // StackOverflow is love, StackOverflow is life
    // https://answers.unity.com/questions/530178/how-to-get-a-component-from-an-object-and-add-it-t.html
    public static T GetCopyOf<T>(this Component a_component, T a_other) where T : Component
    {
        Type type = a_component.GetType();
        if (type != a_other.GetType()) return null;

        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] pInfos = type.GetProperties(flags);
        foreach (var pInfo in pInfos)
        {
            if (pInfo.CanWrite)
            {
                try
                {
                    pInfo.SetValue(a_component, pInfo.GetValue(a_other, null), null);
                }
                catch { }
            }
        }

        FieldInfo[] fInfos = type.GetFields(flags);
        foreach (var fInfo in fInfos)
        {
            fInfo.SetValue(a_component, fInfo.GetValue(a_other));
        }

        return a_component as T;
    }

    public static T AddComponent<T>(this GameObject a_go, T a_toAdd) where T : Component
    {
        return a_go.AddComponent<T>().GetCopyOf(a_toAdd) as T;
    }
}