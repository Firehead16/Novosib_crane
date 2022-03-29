using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Ui;
using UnityEngine.UI;

public class DropdownGrabChanger : MonoBehaviour
{
    [SerializeField] private List<Toggle> questToggles = new List<Toggle>();

    [SerializeField] private CustomDropdown CargoQuest2 = null;
    
    [SerializeField] private Toggle _activeToggle = null;
    [SerializeField] private Toggle _lastToggle = null;
    private int lastCargoIndex;

    
    private List<CustomDropdown.Item> _notSelected = new List<CustomDropdown.Item>()
    {
        new CustomDropdown.Item() {itemName = "Выберите задание"}
    };
    
    private List<CustomDropdown.Item> _grab_first = new List<CustomDropdown.Item>()
    {
        new CustomDropdown.Item() {itemName = "Линейная траверса"},
        new CustomDropdown.Item() {itemName = "Ленточные стропы"}
    };
    
    private List<CustomDropdown.Item> _grab_second = new List<CustomDropdown.Item>()
    {
        new CustomDropdown.Item() {itemName = "Спредер"},
        new CustomDropdown.Item() {itemName = "Канатные стропы"}
    };
    
    private List<CustomDropdown.Item> _grab_second_Gencargo = new List<CustomDropdown.Item>()
    {
        new CustomDropdown.Item() {itemName = "Канатные стропы"}
    };
    
    private List<CustomDropdown.Item> _grab_third = new List<CustomDropdown.Item>()
    {
        new CustomDropdown.Item() {itemName = "Автомобильная траверса"}
    };
    
    private List<CustomDropdown.Item> _grab_fourth = new List<CustomDropdown.Item>()
    {
        new CustomDropdown.Item() {itemName = "Грейфер"}
    };

    // [SerializeField]
    // private Button button = null;
    
    void ChangeDropdown(List<CustomDropdown.Item> dropdownList)
    {
        var dropdown = GetComponent<CustomDropdown>();
        dropdown.dropdownItems = dropdownList;
        dropdown.selectedItemIndex = 0;
        dropdown.SetupDropdown();
    }
    void Start()
    {
        //button.onClick.AddListener(() => GetComponent<CustomDropdown>().Animate());
        ChangeDropdown(_notSelected);
    }
    
    void Update()
    {
        _activeToggle = questToggles.Find(x => x.isOn);

        if (_lastToggle != _activeToggle || CargoQuest2.selectedItemIndex != lastCargoIndex)
        {
            if (_activeToggle == null)
            {
                ChangeDropdown(_notSelected);
            }
            else if (_activeToggle == questToggles[0])
            {
                ChangeDropdown(_grab_first);
            }
            else if (_activeToggle == questToggles[1])
            {
                if (CargoQuest2.selectedItemIndex == 0)
                {
                    ChangeDropdown(_grab_second_Gencargo);
                }
                else ChangeDropdown(_grab_second);
            }
            else if (_activeToggle == questToggles[2])
            {
                ChangeDropdown(_grab_third);
            }
            else if (_activeToggle == questToggles[3])
            {
                ChangeDropdown(_grab_fourth);
            }

            if (_activeToggle != null) _lastToggle = _activeToggle;
            else _lastToggle = null;
            lastCargoIndex = CargoQuest2.selectedItemIndex;
        }
    }
}
