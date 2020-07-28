//This script is a product of Twisted Webbe and its use is determined by the agreement with the Unity Asset Store.

using UnityEngine;

public class CameraDrag : MonoBehaviour {
    public float rotationSpeed;


	
	// Update is called once per frame
	void Update () {

        //This merely checks for the mouse button being dowwn and rotates the camera according to the drag;
            if (Input.GetMouseButton(1))
            {

                float verticalAxis = -Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1);
                transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + (-verticalAxis * rotationSpeed), 0);
             }

     }
}
