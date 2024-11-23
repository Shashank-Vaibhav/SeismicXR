using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SpawnAnchor : MonoBehaviour
{
    public ARPlaneManager planeManager;
    public PlaneClassification classificationFloor;
    public PlaneClassification classificationTable;
    public XRRayInteractor rayInteractor;
    public GameObject cube;
    public GameObject finalTerrain;
    private List<ARPlane> floorPlanes = new List<ARPlane>();
    private List<ARPlane> tablePlanes = new List<ARPlane>();
    public TextMeshProUGUI debugText;
    public LineRenderer rayInteractorLineRenderer;
    public Material rayInteractorLineMaterial;
    public Material currentLineRendererMat;
    //private List<MeshCollider> floorMeshColliders = new List<MeshCollider>();
    //private List<MeshCollider> tableMeshColliders = new List<MeshCollider>();
    private bool isTerrainPresent = false;
    private bool isCubePresent = false;
    private GameObject spawnedTerrain;
    private GameObject spawnedCube;

    private void OnEnable()
    {

        planeManager.planesChanged += GetFloorPlanes;
        planeManager.planesChanged += GetTablePlanes;
        
    }

    private void OnDisable()
    {

        planeManager.planesChanged -= GetFloorPlanes;
        planeManager.planesChanged -= GetTablePlanes;


    }

    private void Start()
    {
       
        rayInteractor.selectEntered.AddListener(SpawnCube);
        rayInteractor.selectEntered.AddListener(SpawnTerrain);
        rayInteractor.selectExited.AddListener(removeLineMat);
        
       
    }
    

  

    private void removeLineMat(SelectExitEventArgs arg0)
    {
        rayInteractorLineRenderer.material = currentLineRendererMat;
    }

    private void GetFloorPlanes(ARPlanesChangedEventArgs obj)
    {
        List<ARPlane> newPlane = obj.added;
        foreach (var item in newPlane)
        {
            if(item.classification == classificationFloor)
            { 
                floorPlanes.Add(item);
                //floorMeshColliders.Add(item.GetComponent<MeshCollider>());
            } 
        }
        debugText.text = $"FloorPlanes are added, planes count: {floorPlanes.Count}";
    }
    private void GetTablePlanes(ARPlanesChangedEventArgs obj)
    {
        List<ARPlane> newPlane = obj.added;
        foreach (var item in newPlane)
        {
            if (item.classification == classificationTable)
            {
                tablePlanes.Add(item);
                //tableMeshColliders.Add(item.GetComponent<MeshCollider>());
            }
        }
        debugText.text = $"TablePlanes are added, planes count: {tablePlanes.Count}";
    }

    private void SpawnCube(SelectEnterEventArgs arg0)
    {
        
        rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit rayHit);
        Pose hitPose = new Pose(rayHit.point, Quaternion.LookRotation(-rayHit.normal));
        foreach (var item in tablePlanes)
        {
            item.TryGetComponent<MeshCollider>(out MeshCollider meshCollider);
            if (rayHit.collider.name == meshCollider.name)
            {
                rayInteractorLineRenderer.material = rayInteractorLineMaterial;
                if (!isCubePresent)
                {
                    spawnedCube = Instantiate(cube, hitPose.position, Quaternion.identity);
                    isCubePresent = true;
                    spawnedCube.TryGetComponent<XRSimpleInteractable>(out XRSimpleInteractable simpleInteractable);
                    simpleInteractable.selectEntered.AddListener(DeactivateCube);
                }

            }
        }
    }
    private void SpawnTerrain(SelectEnterEventArgs arg0)
    {

        rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit rayHit);
        //Pose hitPose = new Pose(rayHit.point, Quaternion.LookRotation(-rayHit.normal));
        //Pose hitPose = new Pose(rayHit.point, Quaternion.identity);
        foreach (var item in floorPlanes)
        {
            item.TryGetComponent<MeshCollider>(out MeshCollider meshCollider);
            if (rayHit.collider.name == meshCollider.name)
            {
                rayInteractorLineRenderer.material = rayInteractorLineMaterial;
                if (!isTerrainPresent)
                {
                    spawnedTerrain = Instantiate(finalTerrain, item.center, Quaternion.identity);
                    isTerrainPresent = true;
                    spawnedTerrain.TryGetComponent<XRSimpleInteractable>(out XRSimpleInteractable simpleInteractable);
                    simpleInteractable.selectEntered.AddListener(DeactivateTerrain);
                }

            }
        }
    }
    private void DeactivateTerrain(SelectEnterEventArgs arg0)
    {
        Destroy(spawnedTerrain);
        isTerrainPresent = false;
    }
    private void DeactivateCube(SelectEnterEventArgs arg0)
    {
        Destroy(spawnedCube);
        isCubePresent = false;
    }
}
