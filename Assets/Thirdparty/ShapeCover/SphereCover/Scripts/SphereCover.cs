//This script is a product of Twisted Webbe and its use is determined by the agreement with the Unity Asset Store.

using UnityEngine;
using System.Collections.Generic;


public class SphereCover : MonoBehaviour
{
    //These are the sizes of the objects as if they were a sphere. Depending on what you want to do you can place larger or smaller radius sizes. Test for perfect placement.
    public float mainObjectRadius, objectsToPlaceRadius;

    //This is the object you want to place. If you want to add variety, you could write a custom script and use a list with a Random selection, or use a randomizer as shown in the Colorful Sphere example.
    public GameObject objectToPlaceRadiusPrefab;

    //This is the percent chance that you want your object to spawn. Anything 100 or higher will always spawn.
    public float spawnChance;

    //Each Row of objects are placed in Rings.
    private float ringRadius, ringCircumference;

    //This determines where to spawn objects, from which row to which row.
    public float fillFromPercentage, fillToPercentage;

    //Do you want the objects to point towards the center? Should they face random directions horizontally (compared to the surface)?
    public bool pointTowardsCenter, randomizeHorizontalRotation;

    //A quick list to see all objects spawned, if you need it for some reason.
    public List<GameObject> clonedObjectsList = new List<GameObject>();

    //Some private variables for looping.
    private int clonedObjectCount, numberOfOuterLoops, numberOfObjectsForThisLoop;

    //The gameobject being spawned for this loop.
    private GameObject thisClonedObject;

    public void Start()
    {
        ProjectileBase thisProj = GetComponentInChildren<ProjectileBase>();
        if (!thisProj)
        {
            thisProj = GetComponent<ProjectileBase>();
        }

        // here we need to know how big we want the surface objects to be
        // maximum will be hardcoded for aesthetics
        // inverse of temp is size (min temp = max size)

        // prefabs are built to be the max size for their range while unscaled.
        // prefab selection is - any surface rock for that size or SMALLER
        // medium is max radius 1. (size 10, mod scale 0.2). 
        // medium has bounds.extends.magnitude = 0.3211165

        mainObjectRadius = thisProj.GetRadius();

        spawnChance = 100 - ((thisProj.thermals.Temperature / 50f) * 100);

        //Debug.Log(mainObjectRadius);
        CoverThatSphere();
    }

    public void CoverThatSphere()
    {

        //How many rows are necessary to cover the surface of the object, taking into account the object sizes
        int v = Mathf.CeilToInt((Mathf.PI * mainObjectRadius) / (objectsToPlaceRadius * 2));
        int numberOfRows = v;

        //Determines the first and last rows to run the spawning loops through.
        int startRow = Mathf.FloorToInt(numberOfRows * fillFromPercentage * 0.01f);
        int endRow = Mathf.CeilToInt(numberOfRows * fillToPercentage * 0.01f);

        //MATH! It equally balances the space between rows. Keep in mind that this and the later code to space between objects does not necessarily an exact distance diagonally between objects.
        float spaceBetweenRows = (2 * Mathf.PI * mainObjectRadius) / numberOfRows;

        //Starting loop for objects
        for (int rowNumber = startRow; rowNumber <= endRow; rowNumber++)
        {
            //This math finds the height of each row as determined by loop and row spacing.
            float rowAngle = (rowNumber * spaceBetweenRows) / mainObjectRadius * 0.5f;
            
            // yPosition will be what we offset to push it up/down
            float yPosition = mainObjectRadius * Mathf.Cos(rowAngle);

            //Creates the row and positions it, and sets the parent.
            GameObject thisRow = new GameObject();
            thisRow.transform.position = new Vector3(transform.position.x, transform.position.y + yPosition, transform.position.z);
            thisRow.transform.parent = transform;
            thisRow.name = "Row" + rowNumber.ToString();

            //This determines the radius of the circle that is the "ring" for this height.
            float radiusFromLoopHeight = Mathf.Sqrt((mainObjectRadius * mainObjectRadius) - (yPosition * yPosition));

            //The top and bottom rows should always have a single object to top them off. This may not space as nicely with the neighboring rows.
            if (rowNumber == 0 || rowNumber == numberOfRows)
            {
                numberOfObjectsForThisLoop = 1;
            }
            else
            {
                // we should be able to set a prefab and have this get the prefab size 

                //Getting the number of objects for this loop.
                numberOfObjectsForThisLoop = Mathf.FloorToInt((2 * Mathf.PI * radiusFromLoopHeight) / (objectsToPlaceRadius * 2));
            }

            //Similar to the row spacing code above, this determines the space between objects in this row.
            float spaceBetweenObjects = (2 * Mathf.PI * radiusFromLoopHeight) / numberOfObjectsForThisLoop;

            //Starting loop for objects around the row/ring.
            for (int ringObjectNumber = 1; ringObjectNumber <= numberOfObjectsForThisLoop; ringObjectNumber++)
            {
                //Setting these variables beforehand so that they can still be used at the end of the for loop.
                float xPosition = 0;
                float zPosition = 0;
                float thisAngle = 0;

                //The first and last objects (at the poles of the sphere) are set this way as to avoid NaN errors.
                if (rowNumber == 0)
                {
                    thisAngle = 0;

                    xPosition = 0;
                    yPosition = mainObjectRadius;
                    zPosition = 0;
                }
                else if (rowNumber == numberOfRows)
                {
                    thisAngle = 0;

                    xPosition = 0;
                    yPosition = -mainObjectRadius;
                    zPosition = 0;
                }
                else
                {
                    //More math to determine the exact position of this object around the ring/row.
                    thisAngle = ((ringObjectNumber) * spaceBetweenObjects) / radiusFromLoopHeight;
                    xPosition = radiusFromLoopHeight * Mathf.Sin(thisAngle);
                    zPosition = radiusFromLoopHeight * Mathf.Cos(thisAngle);
                }


                //This is the spawnChance variable. Any float between 0 and 100 will work, above that it just acts as 100% all the time.
                if (Random.Range(0, 100) <= spawnChance && spawnChance != 0)
                {
                    //Finally! Let's spawn that object!
                    GameObject thisClonedObject = Instantiate(objectToPlaceRadiusPrefab, new Vector3(transform.position.x + xPosition, (yPosition + transform.position.y), transform.position.z + zPosition), Quaternion.identity, thisRow.transform);
                    Debug.Log(thisClonedObject.GetComponent<Renderer>().bounds.extents.magnitude);
                    if (pointTowardsCenter)
                    {
                        //If the objects need to point to the center of the sphere then a shell object is made so as to make it easier to adjust the rotation of the objects.
                        GameObject thisClonedObjectShell = new GameObject();
                        thisClonedObjectShell.transform.position = thisClonedObject.transform.position;
                        thisClonedObjectShell.transform.parent = thisClonedObject.transform.parent;
                        thisClonedObject.transform.parent = thisClonedObjectShell.transform;

                        //If you choose to randomize the Horizontal Rotation it will act as if the surface of the sphere is the horizontal axis for it.
                        if (randomizeHorizontalRotation)
                        {
                            thisClonedObject.transform.localEulerAngles = new Vector3(Random.Range(0, 360), 90, -90);
                        }
                        else
                        {
                            thisClonedObject.transform.localEulerAngles = new Vector3(0, 90, -90);
                        }

                        //Now we point the entire shell towards the center of the sphere.
                        thisClonedObjectShell.transform.LookAt(transform.position);

                        //Name it!
                        thisClonedObject.name = transform.name + " Object" + (clonedObjectsList.Count + 1).ToString();
                        thisClonedObjectShell.name = thisClonedObject.name + " Shell";
                        //Add it to the list for reference/easy parsing.
                        clonedObjectsList.Add(thisClonedObject);
                    }
                    else
                    {
                        //Same as immediately above!
                        thisClonedObject.name = transform.name + " Object" + (clonedObjectsList.Count + 1).ToString();
                        clonedObjectsList.Add(thisClonedObject);
                    }
                }
            }
        }

    }

}