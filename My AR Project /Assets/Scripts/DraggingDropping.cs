using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class DraggingDropping : MonoBehaviour
{
    
    
    [SerializeField]
    ARRaycastManager m_RaycastManager;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    [SerializeField]
    GameObject m_ObjectToPlace;

    [SerializeField]
    private Camera ARCamera;

    private GameObject placedObject;

    private Vector2 touchPosition = default;

    private bool onTouchHold = false;

    

    void Awake() {
        m_RaycastManager = GetComponent<ARRaycastManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = ARCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;

                if(Physics.Raycast(ray, out hitObject))
                {
                    if (hitObject.transform.name.Contains("PlacedObject"))
                    {
                        onTouchHold = true; //we havent leave our finger
                    }
                }
            }
           
            //Leave finger trigger onTouchHold as false
            if(touch.phase == TouchPhase.Ended)
            {
                onTouchHold = false;
            }

        }

        //Move object to new position
        if (m_RaycastManager.Raycast(touchPosition, s_Hits, trackableTypes: TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = s_Hits[0].pose;

            if(placedObject == null)
            {
                placedObject = Instantiate(m_ObjectToPlace, hitPose.position, hitPose.rotation);

            }
            else 
            {
                if(onTouchHold)
                {
                    placedObject.transform.position = hitPose.position;
                    placedObject.transform.rotation = hitPose.rotation;
                }  
            }     
        }
    }
}


