//\===========================================================================================
//\ File: ObjectPooler.cs
//\ Author: Morgan James
//\ Brief: A static class that can pool objects together. Heavily inspired from https://www.youtube.com/watch?v=tdSmKaJvCoA&t=753s.
//\===========================================================================================

using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
	[System.Serializable]
	public class Pool
	{
		public string m_Tag;//How to find the pool.
		public GameObject m_Prefab;//What the pool consists of.
		public int m_Size;//how large the pool is.
	}

	public List<Pool> m_Pools;//The list of current pools.
	public Dictionary<string, Queue<GameObject>> m_PoolDictionary;//The dictionary of all the pools.

	//Simple Singleton pattern.
	public static ObjectPooler instance;

	private void Awake()
	{
		//Make sure there is only one object pooler.
		if (instance == null)
			instance = this;
	}

	private void Start()
	{
		//Create a dictionary of pools in case we want to pool more than one object.
		m_PoolDictionary = new Dictionary<string, Queue<GameObject>>();

		foreach (Pool pool in m_Pools)//For every type of object pool.
		{
			//Create a queue of game objects.
			Queue<GameObject> objectPool = new Queue<GameObject>();	

			for (int iI = 0; iI < pool.m_Size; iI++)
			{
				//Create all the objects to the amount specified.
				GameObject obj = Instantiate(pool.m_Prefab);

				//Set the parent to the gameobject this script is on to keep it neat.
				obj.transform.SetParent(this.transform);

				//Add the object to the back of the pools queue.
				objectPool.Enqueue(obj);
			}

			m_PoolDictionary.Add(pool.m_Tag, objectPool);//add the pool to the pool dictionary with the tag provided.
		}
	}

	//Enables an object from a pool and takes it out of the queue and moves it to the desired location.
	public GameObject SpawnFromPool(string a_sTag, Vector3 a_v3Pos, Quaternion a_qRotation)
	{
		//Early out if there is no such tag.
		if (!m_PoolDictionary.ContainsKey(a_sTag))
		{
			Debug.LogWarning("tag doesn't exist");
			return null;
		}

		//Take the object out of the queue.
		GameObject objectToSpawn = m_PoolDictionary[a_sTag].Dequeue();

		//Enable the object.
		objectToSpawn.SetActive(true);

		//Move the object to the desired location.
		objectToSpawn.transform.position = a_v3Pos;

		//Set the objects rotation to the desired rotation.
		objectToSpawn.transform.rotation = a_qRotation;

		return objectToSpawn;//Return the object so you can manipulate it after spawning it.
	}

}
