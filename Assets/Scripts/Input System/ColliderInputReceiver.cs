using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderInputReceiver : InputReceiver
{
    private Vector3 clickPosition;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse click
        {
            
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); // Cast a ray from the camera

            if (hit.collider != null) // Check if the raycast hit anything
            {
                GameObject selectedObject = hit.collider.gameObject;
                // Do something with the selected object (e.g., highlight it, display info)
                clickPosition = hit.point;
                OnInputReceived();
                Debug.Log("Selected object: " + selectedObject.name);
            }
        }
    }

    public override void OnInputReceived()
    {
        foreach (var handler in inputHandlers)
        {
            handler.ProcessInput(clickPosition, null, null);
        }
    }

}
