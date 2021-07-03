using System.Collections.Generic;
using UnityEngine;
using System;

//Class that saves the stats of each unit to file at a set interval. Uses a dirty flag pattern to only update files if the associated unit has been modified, this makes the
//code more efficient because time is not spent saving files for units that have not been changed.
public class SavingSystem : MonoBehaviour
{
    [SerializeField]
    float secondsBetweenSaves = 60f;
    float lastSave = 0f;
    [SerializeField]
    float timeToNextSave = 0f; //Debug variable so I can keep track of how long until the next auto-save in the editor

    [SerializeField]
    bool enableAutosave = false;

    [SerializeField]
    string folderPath = "";

    //New value type to define a unit, the data in this struct is saved to a text file
    [Serializable]
    public struct Unit
    {
        [SerializeField]
        int ID, health, mana, attack, defence, magic, resistance, speed;

        [SerializeField]
        public Unit(int ID, int health, int mana, int attack, int defence, int magic, int resistance, int speed)
        {
            this.ID = ID;
            this.health = health;
            this.mana = mana;
            this.attack = attack;
            this.defence = defence;
            this.magic = magic;
            this.resistance = resistance;
            this.speed = speed;
        }

        //Enum representing the stats that define a unit
        public enum Stat { Health, Mana, Attack, Defence, Magic, Resistance, Speed };

        //Functions to get and set the stat field of each unit, rather than creating properties for each stat uses Stat enum to determine which stat to read/write
        public void UpdateStat(Stat stat, int newValue)
        {
            switch (stat)
            {
                case Stat.Health:
                    health = newValue;
                    break;
                case Stat.Mana:
                    mana = newValue;
                    break;
                case Stat.Attack:
                    attack = newValue;
                    break;
                case Stat.Defence:
                    defence = newValue;
                    break;
                case Stat.Magic:
                    magic = newValue;
                    break;
                case Stat.Resistance:
                    resistance = newValue;
                    break;
                case Stat.Speed:
                    speed = newValue;
                    break;
                default:
                    break;
            }
        }
        public int GetStat(Stat stat)
        {
            switch (stat)
            {
                case Stat.Health:
                    return health;
                case Stat.Mana:
                    return mana;
                case Stat.Attack:
                    return attack;
                case Stat.Defence:
                    return defence;
                case Stat.Magic:
                    return magic;
                case Stat.Resistance:
                    return resistance;
                case Stat.Speed:
                    return speed;
                default:
                    return 0;
            }
        }
    }

    [SerializeField]
    Unit[] unitsList = new Unit[numberOfUnits];
    const int numberOfUnits = 4;
    [SerializeField]
    List<int> updatedUnits = new List<int>();

    [SerializeField]
    UnitUI[] unitUis = new UnitUI[numberOfUnits];

    [SerializeField]
    SavingMessage sm = null;

    //Read all unit save files from disk and update the UI to show the saved data
    private void Awake()
    {
        //Build path from the base data path for the application to the folder storing the save files
        folderPath = Application.dataPath + "\\Save Files";

        //Check that the folder exists, if it doesn't create it anbd generate new default unit files
        if (FileIO.CheckForFolder(folderPath))
        {
            Debug.Log(numberOfUnits);
            unitsList = new Unit[numberOfUnits];
            for(int i = 0; i < numberOfUnits; i++)
            {
                unitsList[i] = new Unit(i, 0, 0, 0, 0, 0, 0, 0);
            }
            SaveUnits();
        }
        else
        {
            unitsList = LoadUnits();
        }

        for(int i = 0; i < unitUis.Length; i++)
        {
            unitUis[i].UpdateUnitUI(unitsList[i]);
        }
    }

    //Check to see if auto-save is enabled, if it is reduce the timer and if the save time has elapsed save the units and reset the timer
    void Update()
    {
        if (enableAutosave)
        {
            timeToNextSave = (lastSave + secondsBetweenSaves) - Time.time;
            if (Time.time > lastSave + secondsBetweenSaves)
            {
                SaveUnits();
            }
        }
        else
        {
            timeToNextSave = secondsBetweenSaves;
            lastSave = Time.time;
        }

    }

    //Only save units that have been updated since the last save, build list of unit IDs that have successfully been saved and then update the saving message to show user
    //which have been saved
    public void SaveUnits()
    {
        List<int> savedUnits = new List<int>();
        foreach(int index in updatedUnits)
        {
            string filePath = folderPath + "\\Unit" + (index + 1).ToString() + ".txt";
            FileIO.SaveJsonData(unitsList[index], filePath);
            savedUnits.Add(index);
        }
        if(savedUnits.Count > 0)
        {
            sm.SetSavingText(savedUnits);
        }
        updatedUnits = new List<int>();
        lastSave = Time.time;
    }

    //Read unit info from saved files and return array of unit data
    public Unit[] LoadUnits()
    {
        Unit[] units = new Unit[numberOfUnits];
        for(int i = 0; i < numberOfUnits; i++)
        {
            string filePath = $"{folderPath}\\Unit{i + 1}.txt";
            units[i] = FileIO.LoadJsonData<Unit>(filePath, true);
        }
        return units;
    }

    //Update the requested unit stat with the specified value, if the unit isn't already on the updated units list add it to it
    public void UpdateUnitStat(int unitNumber, Unit.Stat stat, int newValue)
    {
        int unitIndex = unitNumber - 1;
        unitsList[unitIndex].UpdateStat(stat, newValue);
        if (!updatedUnits.Contains(unitIndex))
        {
            updatedUnits.Add(unitIndex);
        }
    }

    //Change auto-save value
    public void ToggleAutoSave(bool autoSave)
    {
        enableAutosave = autoSave;
    }
}
