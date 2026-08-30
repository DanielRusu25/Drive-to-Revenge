using System;
using System.Collections.Generic;
using UnityEngine;

public class CarSwapper : MonoBehaviour, IDataPersistence
{
    public CurrentCarManager currentCarManager;

    [Serializable]
    public class WheelContainer
    {
        public Mesh leftWheel;
        public Mesh rightWheel;
    }
    public List<WheelContainer> wheelContainer;

    [Serializable]
    public class PhisicalCarComponents
    {
        [Header("Body")]
        public MeshFilter bodyMeshFilter;
        public MeshRenderer bodyMeshRenderer;
        public MeshCollider bodyMeshCollider;

        [Serializable]
        public class Wheel
        {
            [Tooltip("True = Right side  \n False = Left side ")]
            public bool rightSide;
            [Tooltip("True = Front side  \n False = Rear side ")]
            public bool frontSide;
            public GameObject wheelWithMesh;
            public GameObject wheelWithCollider;

            [HideInInspector] public MeshFilter wheelMeshFilter;
            [HideInInspector] public MeshRenderer wheelMeshRenderer;
            [HideInInspector] public Transform wheelColliderTransform;
            [HideInInspector] public MeshCollider wheelMeshCollider;



            public void Initialize()
            {
                if (wheelWithMesh != null)
                {
                    wheelMeshFilter = wheelWithMesh.GetComponent<MeshFilter>();
                    wheelMeshRenderer = wheelWithMesh.GetComponent<MeshRenderer>();
                }

                if (wheelWithCollider != null)
                {
                    wheelColliderTransform = wheelWithCollider.transform;
                    wheelMeshCollider = wheelWithCollider.GetComponent<MeshCollider>();
                }

            }
        }

        [Space(10)]
        public List<Wheel> wheels;
    }
    public PhisicalCarComponents phisicalCarComponents;

    [Serializable]
    public class VisualCarContainer
    {
        [Tooltip("Used only to ease the use of the inspector")]
        public string nameOfCar;

        [Header("Body")]
        public Mesh bodyMesh;
        public string bodyHexColor;

        [Space(10)]
        [Header("Wheels")]
        public Mesh leftWheelMesh;
        public Mesh rightWheelMesh;
        [Serializable]
        public class WheelCoords
        {
            public float wheelX;
            public float wheelY;
            public float wheelFrontZ;
            public float wheelRearZ;
        }

        public WheelCoords wheelCoords;
    }
    public List<VisualCarContainer> visualCarContainers;

    public int currentShowCarIndex;

    public void Start()
    {
        foreach (var wheel in phisicalCarComponents.wheels)
            wheel.Initialize();


        currentShowCarIndex = currentCarManager.currentCarIndex;
        ChangeCar(currentShowCarIndex);
    }

    public void ChangeCar(int index)
    {
        index--;
        //Change the body
        ChangeMesh(phisicalCarComponents.bodyMeshFilter, visualCarContainers[index].bodyMesh);
        if (phisicalCarComponents.bodyMeshCollider != null)
            ChangeCollider(phisicalCarComponents.bodyMeshCollider, visualCarContainers[index].bodyMesh);
        ChangeColor(phisicalCarComponents.bodyMeshRenderer, visualCarContainers[index].bodyHexColor);

        //Change the wheels
        for (int i = 0; i < phisicalCarComponents.wheels.Count; i++)
        {
            if (phisicalCarComponents.wheels[i].rightSide)
                ChangeMesh(phisicalCarComponents.wheels[i].wheelMeshFilter, visualCarContainers[index].rightWheelMesh);
            else
                ChangeMesh(phisicalCarComponents.wheels[i].wheelMeshFilter, visualCarContainers[index].leftWheelMesh);

            ChangeWheelPosition(phisicalCarComponents.wheels[i], visualCarContainers[index].wheelCoords);
        }
    }

    public void ChangeMesh(MeshFilter meshFilterOfObject, Mesh newMesh)
    {
        meshFilterOfObject.mesh = newMesh;
    }

    public void ChangeCollider(MeshCollider meshColliderOfObject, Mesh newMesh)
    {
        meshColliderOfObject.sharedMesh = newMesh;
    }

    public void ChangeColor(MeshRenderer meshRendererOfObject, string newHexColor)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(newHexColor, out color))
            meshRendererOfObject.material.color = color;
        else
            Debug.LogWarning("Invalid hex color code: " + newHexColor);
    }

    public void ChangeWheelPosition(PhisicalCarComponents.Wheel wh, VisualCarContainer.WheelCoords whCords)
    {
        Vector3 newPosition = new Vector3();
        newPosition.y = whCords.wheelY;
        if (wh.rightSide)
            newPosition.x = whCords.wheelX;
        else
            newPosition.x = -whCords.wheelX;


        if (wh.frontSide)
            newPosition.z = whCords.wheelFrontZ;
        else
            newPosition.z = whCords.wheelRearZ;

        wh.wheelColliderTransform.localPosition = newPosition;
    }

    public void LoadVisualContainers()
    {
        for (int i = 0; i < visualCarContainers.Count; i++)
        {
            //Load the name
            visualCarContainers[i].nameOfCar = currentCarManager.mainCars[i + 1].name;

            //Load the body color
            visualCarContainers[i].bodyHexColor = currentCarManager.mainCars[i + 1].visualUpgrades.bodyHexColor;

            //Load the wheel coords
            visualCarContainers[i].wheelCoords.wheelX = currentCarManager.mainCars[i + 1].visualUpgrades.wheelCoords.wheelX;
            visualCarContainers[i].wheelCoords.wheelY = currentCarManager.mainCars[i + 1].visualUpgrades.wheelCoords.wheelY;
            visualCarContainers[i].wheelCoords.wheelFrontZ = currentCarManager.mainCars[i + 1].visualUpgrades.wheelCoords.wheelFrontZ;
            visualCarContainers[i].wheelCoords.wheelRearZ = currentCarManager.mainCars[i + 1].visualUpgrades.wheelCoords.wheelRearZ;

            //Load the wheel meshes
            int wheelIndex = currentCarManager.mainCars[i + 1].visualUpgrades.wheelId;
            this.visualCarContainers[i].leftWheelMesh = wheelContainer[wheelIndex].leftWheel;
            this.visualCarContainers[i].rightWheelMesh = wheelContainer[wheelIndex].rightWheel;
        }
    }

    public void LoadData(GameData data)
    {

        for (int i = 0; i < visualCarContainers.Count; i++)
        {

            //Load wheel index
            int wheelIndex = data.mainCars[i + 1].visualUpgrades.wheelId;
            this.visualCarContainers[i].leftWheelMesh = wheelContainer[wheelIndex].leftWheel;
            this.visualCarContainers[i].rightWheelMesh = wheelContainer[wheelIndex].rightWheel;

            //Load Wheel Coords
            this.visualCarContainers[i].wheelCoords.wheelX = data.mainCars[i + 1].visualUpgrades.wheelCoords.wheelX;
            this.visualCarContainers[i].wheelCoords.wheelY = data.mainCars[i + 1].visualUpgrades.wheelCoords.wheelY;
            this.visualCarContainers[i].wheelCoords.wheelFrontZ = data.mainCars[i + 1].visualUpgrades.wheelCoords.wheelFrontZ;
            this.visualCarContainers[i].wheelCoords.wheelRearZ = data.mainCars[i + 1].visualUpgrades.wheelCoords.wheelRearZ;

            //Load body color
            this.visualCarContainers[i].bodyHexColor = data.mainCars[i + 1].visualUpgrades.bodyHexColor;

        }

    }

    public void SaveData(GameData data)
    {
        //Find the wheel index of each car
        for (int i = 0; i < visualCarContainers.Count; i++)
        {
            VisualCarContainer car = visualCarContainers[i];
            int matchedIndex = -1;

            for (int j = 0; j < wheelContainer.Count; j++)
            {
                if (wheelContainer[j].leftWheel == car.leftWheelMesh &&
                    wheelContainer[j].rightWheel == car.rightWheelMesh)
                {
                    matchedIndex = j;
                    break;
                }
            }

            //Save the wheel Id
            data.mainCars[i + 1].visualUpgrades.wheelId = matchedIndex;

            //Save the wheel coords
            data.mainCars[i + 1].visualUpgrades.wheelCoords.wheelX = car.wheelCoords.wheelX;
            data.mainCars[i + 1].visualUpgrades.wheelCoords.wheelY = car.wheelCoords.wheelY;
            data.mainCars[i + 1].visualUpgrades.wheelCoords.wheelFrontZ = car.wheelCoords.wheelFrontZ;
            data.mainCars[i + 1].visualUpgrades.wheelCoords.wheelRearZ = car.wheelCoords.wheelRearZ;

            //Save the body hex color
            data.mainCars[i + 1].visualUpgrades.bodyHexColor = visualCarContainers[i].bodyHexColor;
        }

    }


}
