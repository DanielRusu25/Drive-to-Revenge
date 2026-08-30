using System.Collections.Generic;
using UnityEngine;

public class CurrentCarManager : MonoBehaviour, IDataPersistence
{
    //public GameData.Car currentCar;
    public GameData.Car[] mainCars;
    public int currentCarIndex;
    public int currentMoney;
    public int numberOfCars;

    public void LoadData(GameData data)
    {
        this.mainCars = data.mainCars;
        this.currentCarIndex = data.mainCars[0].carIndex;
        this.currentMoney = data.currentMoney;
        this.numberOfCars = data.mainCars.Length - 1;
    }

    public void SaveData(GameData data)
    {
        data.mainCars = this.mainCars;
        data.mainCars[this.currentCarIndex] = this.mainCars[0];
        data.currentMoney = this.currentMoney;
    }
}
