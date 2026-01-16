using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CardTracker : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private GameObject cubePrefab;

    private GameObject spawnedCube;
    private bool isCubeVisible = false;

    void Awake()
    {
        if (trackedImageManager == null)
            trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            SpawnCubeOnImage(trackedImage);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateCubePosition(trackedImage);
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            HideCube();
        }
    }

    void SpawnCubeOnImage(ARTrackedImage trackedImage)
    {
        if (trackedImage.referenceImage.name == "MyCard" && !isCubeVisible)
        {
            if (spawnedCube == null)
            {
                spawnedCube = Instantiate(cubePrefab, trackedImage.transform);
                spawnedCube.transform.localPosition = Vector3.zero;
                spawnedCube.transform.localRotation = Quaternion.identity;
            }
            else
            {
                spawnedCube.transform.SetParent(trackedImage.transform);
                spawnedCube.transform.localPosition = Vector3.zero;
                spawnedCube.transform.localRotation = Quaternion.identity;
                spawnedCube.SetActive(true);
            }
            isCubeVisible = true;
        }
    }

    void UpdateCubePosition(ARTrackedImage trackedImage)
    {
        if (trackedImage.referenceImage.name == "MyCard" && isCubeVisible)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                spawnedCube.transform.SetParent(trackedImage.transform);
                spawnedCube.transform.localPosition = Vector3.zero;
                spawnedCube.transform.localRotation = Quaternion.identity;
                spawnedCube.SetActive(true);
            }
            else
            {
                spawnedCube.SetActive(false);
            }
        }
    }

    void HideCube()
    {
        if (spawnedCube != null)
        {
            spawnedCube.SetActive(false);
            isCubeVisible = false;
        }
    }
}