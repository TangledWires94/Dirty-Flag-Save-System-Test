using UnityEngine;
using TMPro;

//UI event handling class that adds subscribers to value change events to update the saving system when stats are changed
public class UnitUI : MonoBehaviour
{
    //Identifier to tell the saving system which unit this UnitUI represents
    [SerializeField]
    int unitNumber = 0;

    [SerializeField]
    TMP_InputField healthInput, manaInput, attackInput, defenceInput, magicInput, resistanceInput, speedInput;

    [SerializeField]
    SavingSystem saveSystem = null;

    //Create anonynous methods to call StatUpdate() when any of the stat input fields are updated
    private void Awake()
    {
        healthInput.onValueChanged.AddListener(delegate (string input) { StatUpdate(SavingSystem.Unit.Stat.Health, input); });
        manaInput.onValueChanged.AddListener(delegate (string input) { StatUpdate(SavingSystem.Unit.Stat.Mana, input); });
        attackInput.onValueChanged.AddListener(delegate (string input) { StatUpdate(SavingSystem.Unit.Stat.Attack, input); });
        defenceInput.onValueChanged.AddListener(delegate (string input) { StatUpdate(SavingSystem.Unit.Stat.Defence, input); });
        magicInput.onValueChanged.AddListener(delegate (string input) { StatUpdate(SavingSystem.Unit.Stat.Magic, input); });
        resistanceInput.onValueChanged.AddListener(delegate (string input) { StatUpdate(SavingSystem.Unit.Stat.Resistance, input); });
        speedInput.onValueChanged.AddListener(delegate (string input) { StatUpdate(SavingSystem.Unit.Stat.Speed, input); });
    }

    //Update saving system to tell it the unit and stat that have been changed and what the new value is
    void StatUpdate(SavingSystem.Unit.Stat stat, string input)
    {
        float value = 0;
        //Convert the input string value to a floating point value, if it can't be converted skip the rest of the function
        if (float.TryParse(input, out value))
        {
            int integerValue = 0;
            //Saving system only accepts integer values, uses modulus function to check if the value has a fractional component and it if does the value is rounded to the
            //nearest integer and the UI text is updated to match the value that will be sent to the saving system
            if (value % 1 > 0)
            {
                integerValue = Mathf.RoundToInt(value);
                switch (stat)
                {
                    case SavingSystem.Unit.Stat.Health:
                        healthInput.text = integerValue.ToString();
                        break;
                    case SavingSystem.Unit.Stat.Mana:
                        manaInput.text = integerValue.ToString();
                        break;
                    case SavingSystem.Unit.Stat.Attack:
                        attackInput.text = integerValue.ToString();
                        break;
                    case SavingSystem.Unit.Stat.Defence:
                        defenceInput.text = integerValue.ToString();
                        break;
                    case SavingSystem.Unit.Stat.Magic:
                        magicInput.text = integerValue.ToString();
                        break;
                    case SavingSystem.Unit.Stat.Resistance:
                        resistanceInput.text = integerValue.ToString();
                        break;
                    case SavingSystem.Unit.Stat.Speed:
                        speedInput.text = integerValue.ToString();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                integerValue = (int)value;
            }
            //Tell saving system which stat on which unit has changes and what the new value is
            saveSystem.UpdateUnitStat(unitNumber, stat, integerValue);
        }
    }

    //Update the string values in each stat UI object to match the saved values in the saving system
    public void UpdateUnitUI(SavingSystem.Unit unit)
    {
        healthInput.text = unit.GetStat(SavingSystem.Unit.Stat.Health).ToString();
        manaInput.text = unit.GetStat(SavingSystem.Unit.Stat.Mana).ToString();
        attackInput.text = unit.GetStat(SavingSystem.Unit.Stat.Attack).ToString();
        defenceInput.text = unit.GetStat(SavingSystem.Unit.Stat.Defence).ToString();
        magicInput.text = unit.GetStat(SavingSystem.Unit.Stat.Magic).ToString();
        resistanceInput.text = unit.GetStat(SavingSystem.Unit.Stat.Resistance).ToString();
        speedInput.text = unit.GetStat(SavingSystem.Unit.Stat.Speed).ToString();
    }

}
