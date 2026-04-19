using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MultipleImageTrackingManager : MonoBehaviour
{
    [SerializeField] List<GameObject> prefabsToSpawn = new List<GameObject>();

    private ARTrackedImageManager _trackedImageManager;

    private Dictionary<string, GameObject> _arObjects = new Dictionary<string, GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _trackedImageManager = GetComponent<ARTrackedImageManager>();
        if( _trackedImageManager == null ) return;
        _trackedImageManager.trackablesChanged.AddListener(OnImagesTrackedChanged);
        _arObjects = new Dictionary<string, GameObject>();
        SetUpSceneElements();
    }

	private void OnDestroy()
	{
		_trackedImageManager.trackablesChanged.RemoveListener(OnImagesTrackedChanged);
	}

    private void OnImagesTrackedChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventargs)
    {
        foreach(var trackedImage in eventargs.added)
        {
            UpdateTrackedImage(trackedImage);
        }
		foreach (var trackedImage in eventargs.updated)
		{
			UpdateTrackedImage(trackedImage);
		}
		foreach (var trackedImage in eventargs.removed)
		{
			UpdateTrackedImage(trackedImage.Value);
		}
	}

    private void UpdateTrackedImage(ARTrackedImage trackedImage)
    {
        if (trackedImage == null) return;
        if ( trackedImage.trackingState is UnityEngine.XR.ARSubsystems.TrackingState.Limited or UnityEngine.XR.ARSubsystems.TrackingState.None)
        {
            _arObjects[trackedImage.referenceImage.name].gameObject.SetActive(false);
            return;
        }
        _arObjects[trackedImage.referenceImage.name].gameObject.SetActive(true);
        _arObjects[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
        _arObjects[trackedImage.referenceImage.name].transform.rotation = trackedImage.transform.rotation;
    }

	private void SetUpSceneElements()
    {
        foreach( var prefab in prefabsToSpawn )
        {
            var arObject = Instantiate( prefab, Vector3.zero, Quaternion.identity );
            arObject.name = prefab.name;
            arObject.gameObject.SetActive( false );
            _arObjects.Add(arObject.name, arObject );
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
