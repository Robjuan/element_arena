//This script is a product of Twisted Webbe and its use is determined by the agreement with the Unity Asset Store.

using System.Collections.Generic;
using UnityEngine;

public class RandomizeMaterial : MonoBehaviour {

    public List<Material> materialsList;

    // Use this for initialization
    void Start () {

        //This script merely finds the count for the number of materials in the list and chooses a random material to place on the object.
        //The concept can be reused to create an object that randomly replaces itself with a prefab from a list.

        int materialsCount = materialsList.Count;

        Material randomizedMaterial = materialsList[Random.Range(0, materialsCount)];
        GetComponent<Renderer>().sharedMaterial = randomizedMaterial;
	}
	
}
