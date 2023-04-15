using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ARSessionOrigin))]
public class ArPlaneCtrl : MonoBehaviour
{
    public const float HEIGHT_LIMIT = -0.1F;
    public Color activatedColor, disabledColor;
    public Material activatedMaterial, disabledMaterial;
    public Text debug;

    [SerializeField] Transform _product;
    private bool _modelIsPlaced;

    ARPlaneManager _arPlaneManager;
    ARRaycastManager _raycastManager;
    List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    Touch _touch;
    TrackableId _trackableId;


    #region Unity life cycle

    void OnEnable()
    {
        //debug.text += "\n***** On Enable()";
        _arPlaneManager = GetComponent<ARPlaneManager>();
        _raycastManager = GetComponent<ARRaycastManager>();

        _arPlaneManager.planesChanged += OnPlaneChanged;
    }


    bool TouchIsOverUi()
    {
        // Check if there is a touch
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began 
            // Check if finger is over a UI element
            && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
    }


    void Update()
    {
        // If the player has not touched the screen, we are done with this update.
        if (Input.touchCount < 1) return;

        _touch = Input.GetTouch(0);
        if (!EventSystem.current.IsPointerOverGameObject(_touch.fingerId))
        {
            if (_raycastManager.Raycast(_touch.position, _hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = _hits[0].pose;

                // determine the hit plane
                _trackableId = _hits[0].trackableId;
                var hitPlane = _arPlaneManager.GetPlane(_trackableId);

                if (hitPlane == null)
                    return;

                if (hitPlane.GetComponent<MeshCollider>().enabled)
                {
                    if (!_modelIsPlaced)
                        PlaceModel(hitPose.position, hitPlane);
                }
            }
        }
    }

    private void PlaceModel(Vector3 pos, ARPlane plane)
    {
        var finalPos = new Vector3(pos.x, pos.y + 0.02f, pos.z);
        _product.position = finalPos;
       /* _product.LookAt(Camera.main.transform);
        Quaternion rota = Quaternion.Euler(_product.localRotation.x,
                             plane.normal.y,
                            _product.localRotation.z);
        _product.localRotation = rota;
        _product.gameObject.SetActive(true);*/
        _modelIsPlaced = true;
    }

    private void OnDisable()
    {
        //if(debug != null) debug.text += "\n***** On Disable()";
        _arPlaneManager.planesChanged -= OnPlaneChanged;
    }

    #endregion

    float _planesTotalSize;

    void OnPlaneChanged(ARPlanesChangedEventArgs args)
    {
        // estimate the total size of all planes
        _planesTotalSize = 0;
        foreach(ARPlane p in _arPlaneManager.trackables)
        {
            _planesTotalSize += p.size.x * p.size.y;
        }
        if (_planesTotalSize > 6 && _arPlaneManager.enabled && _modelIsPlaced)
            DisableLookingForArPlanes();

        //debug.text = "\n## OnPlaneAdded() number of ArPlanes = " + _allPlanes.Count;
        foreach (var plane in args.added)
        {
            if (plane.transform.position.y > HEIGHT_LIMIT
                || plane.alignment == PlaneAlignment.HorizontalDown
                || _modelIsPlaced)
                MakePlaneInvisible(plane);
        }

    }


    void TogglePlaneVisibility(bool show)
    {
        foreach (var plane in _arPlaneManager.trackables)
        {
            var rend = plane.GetComponent<MeshRenderer>();
            if (show && plane.GetComponent<MeshCollider>().enabled)
            {
                rend.material = activatedMaterial;
            }
            else
                rend.material = disabledMaterial;
        }
    }


    // change the color of an ARPlane whose MeshCollider is disabled
    void ChangeColorOfArPlane(ARPlane plane, Color color)
    {
        Material mat = plane.GetComponent<Renderer>().material;
        mat.SetColor("_PlaneColor", color);
    }


    void MakePlaneInvisible(ARPlane plane)
    {
        plane.GetComponent<MeshCollider>().enabled = false;
        plane.GetComponent<MeshRenderer>().enabled = false;
        //ChangeColorOfArPlane(plane, disabledColor);
    }


    void DisableLookingForArPlanes()
    {
        TogglePlaneVisibility(false);
        _arPlaneManager.enabled = false;
        //debug.text += "\n* _arPlaneManager Disabled";
    }

}