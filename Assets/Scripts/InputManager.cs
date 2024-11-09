using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera arCam;
    [SerializeField] private ARRaycastManager _raycastManager;
    [SerializeField]
    private GameObject crossHair;

    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private Touch touch;
    private Pose pose;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    // void Update()
    // {
    //     CrossHairCalculation();
    //     touch = Input.GetTouch(0);

    //     if (Input.touchCount < 0 || touch.phase != TouchPhase.Began)
    //     {
    //         return;
    //     }

    //     if (IsPointerOverUI(touch)) return;
    //     Instantiate(DataHandler.Instance.GetFurniture(), pose.position, pose.rotation);
    // }

    void Update()
    {
        CrossHairCalculation();

        if (Input.touchCount > 0) // Ensure that there is at least one touch
        {
            touch = Input.GetTouch(0); // Get the first touch

            if (touch.phase == TouchPhase.Began) // Check for touch start
            {
                if (IsPointerOverUI(touch)) return; // Skip if touch is over UI elements

                Instantiate(DataHandler.Instance.GetFurniture(), pose.position, pose.rotation); // Place furniture in AR
            }
        }
    }


    bool IsPointerOverUI(Touch touch)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(touch.position.x, touch.position.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    void CrossHairCalculation()
    {
        Vector3 origin = arCam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));
        Ray ray = arCam.ScreenPointToRay(origin);
        if (_raycastManager.Raycast(ray, _hits))
        {
            pose = _hits[0].pose;
            crossHair.transform.position = pose.position;
            crossHair.transform.eulerAngles = new Vector3(90, 0, 0);
        }
    }
}
