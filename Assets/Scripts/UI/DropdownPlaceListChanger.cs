using System.Collections;
using System.Collections.Generic;
using Core.Gameplay.Questing;
using Core.Ui;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DropdownPlaceListChanger : MonoBehaviour
{
    public List<Toggle> questToggles = new List<Toggle>();

    [SerializeField] private CustomDropdown CargoType2Quest;

    public Toggle _activeToggle = null;
    public Toggle _lastToggle = null;
    public int lastCargo2Index = 0;

    
    private List<CustomDropdown.Item> _notSelected = new List<CustomDropdown.Item>()
    {
        new CustomDropdown.Item() {itemName = "Выберите задание"}
    };

    private List<CustomDropdown.Item> _place_first = new List<CustomDropdown.Item>()
    {
        new CustomDropdown.Item() {itemName = "Ж/Д платформа"},
        new CustomDropdown.Item() {itemName = "Автотранспорт"},
        new CustomDropdown.Item() {itemName = "Плашкоут"},
        new CustomDropdown.Item() {itemName = "Другое судно"}
    };
    
    private List<CustomDropdown.Item> _place_second = new List<CustomDropdown.Item>()
    {
        new CustomDropdown.Item() {itemName = "Ж/Д платформа"},
        new CustomDropdown.Item() {itemName = "Автотранспорт"},
        new CustomDropdown.Item() {itemName = "Плашкоут"},
        new CustomDropdown.Item() {itemName = "Другой контейнер"}
    };

    private List<CustomDropdown.Item> _place_second_gen = new List<CustomDropdown.Item>()
    {
        new CustomDropdown.Item() {itemName = "Трюм другого судна"}
    };
    
    private List<CustomDropdown.Item> _place_third = new List<CustomDropdown.Item>()
    {
        new CustomDropdown.Item() {itemName = "Плашкоут"},
        new CustomDropdown.Item() {itemName = "Трюм"}
    };
    
    private List<CustomDropdown.Item> _place_fourth = new List<CustomDropdown.Item>()
    {
        new CustomDropdown.Item() {itemName = "Трюм"},
        new CustomDropdown.Item() {itemName = "Плашкоут"},
        new CustomDropdown.Item() {itemName = "Ж/Д вагон"},
        new CustomDropdown.Item() {itemName = "Другое судно"}
    };

    void Start()
    {
        ChangeDropdown(_notSelected);
    }

    void ChangeDropdown(List<CustomDropdown.Item> dropdownList)
    {
        GetComponent<CustomDropdown>().dropdownItems = dropdownList;
        GetComponent<CustomDropdown>().selectedItemIndex = 0;
        GetComponent<CustomDropdown>().SetupDropdown();
    }

    void Update()
    {
        _activeToggle = questToggles.Find(x => x.isOn);

        if (_lastToggle != _activeToggle || lastCargo2Index != CargoType2Quest.index)
        {
            if (_activeToggle == null)
            {
                ChangeDropdown(_notSelected);
            }
            else if (_activeToggle == questToggles[0])
            {
                ChangeDropdown(_place_first);
            }
            else if (_activeToggle == questToggles[1])
            {
                if (CargoType2Quest.index == 0) //выбраны ген. грузы
                {
                    ChangeDropdown(_place_second_gen);
                }
                else ChangeDropdown(_place_second);
            }
            else if (_activeToggle == questToggles[2])
            {
                ChangeDropdown(_place_third);
            }
            else if (_activeToggle == questToggles[3])
            {
                ChangeDropdown(_place_fourth);
            }

            lastCargo2Index = CargoType2Quest.index;
            if (_activeToggle != null) _lastToggle = _activeToggle;
            else _lastToggle = null;
        }

        //Debug.Log(GetComponent<CustomDropdown>().index);
        //Debug.Log(GetComponent<CustomDropdown>().dropdownItems.Count);
    }
}
