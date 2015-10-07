using UnityEngine;
using System.Collections;

public class ArmRotate : MonoBehaviour {

    public int rotationOffset = 0;
    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; //position of mouse - position of character
        difference.Normalize();  //normalize the vector (Sum of the vector will be equal to 1)

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; //finding the angle between the x axis and vector from (0,0) to (2,2), and converting it to degrees
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset);
    }

}
