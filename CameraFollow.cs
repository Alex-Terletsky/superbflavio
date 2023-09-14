using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour //Class to make the camera follow the Y position of the player while maintaining its current angle
{

    [SerializeField] private GameObject player;        
    private float offset;            
    private float xPos;
    private float yPos;
    private float zPos;
    //Declaring variables

    void Start() //Start method plays once to assign offsets and current positions
    {
        xPos = transform.position.x;
        zPos = transform.position.z;
        offset = transform.position.y - player.transform.position.y;
    }

    void Update() //Update method runs constantly to follow player position and move the camera accordingly
{
        yPos = player.transform.position.y;
        transform.position = new Vector3(xPos, yPos + offset, zPos); //Sets new position of camera
}
}