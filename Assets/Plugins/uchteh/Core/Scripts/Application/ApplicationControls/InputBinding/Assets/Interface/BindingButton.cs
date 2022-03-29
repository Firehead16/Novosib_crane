using System;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public interface IBindingPreview
{
    InputBinding InputBinding
    {
        get;
        set;
    }
}

public class CompositBindingPreview : IBindingPreview
{
    private InputBinding inputBinding;

    public InputBinding InputBinding
    {
        get => inputBinding;
        set => inputBinding = value;
    }
}
public class BindingButton : MonoBehaviour, IBindingPreview
{
    [SerializeField] private bool isUpdated;
    [SerializeField] private Text bindingNameText = null;
    [SerializeField] private Text bindingGroupText = null;
    [SerializeField] private Text bindingButtonText = null;
    [SerializeField] private Text bindingDeviceText = null;

    public event Action OnUpdated;

    #region Text assign
    public string BindingGroupName
    {
        get => bindingNameText.text;
        set => bindingNameText.text = value;
    }
    public string BindingName
    {
        get => bindingGroupText.text;
        set => bindingGroupText.text = value;
    }
    public string BindingButtonText
    {
        get => bindingButtonText.text;
        set => bindingButtonText.text = value;
    }
    public string DeviceText
    {
        get
        {
            if(bindingDeviceText) return bindingDeviceText.text;
            return "";
        }
        set { if(bindingDeviceText) bindingDeviceText.text = value; }
    }

    #endregion


    [SerializeField]
    private InputBinding inputBinding;

    public InputBinding InputBinding
    {
        get => inputBinding;
        set
        {
            inputBinding = value;

            BindingButtonText = inputBinding.path;
	           //var aSplit = inputBinding.path.Split('/');
               //aSplit[0]
               //    .Replace("<" ,"")
               //    .Replace(">","")
               //    .Replace("HID::","")
               //+ "\n"+
               //inputBinding.ToDisplayString();                                              // BindingButtonText = Action.GetBindingDisplayString(); //имя из action (не обновляется)
                                                                                                  //todo ПОЧИНИТЬ СТРОКА В КНОПКАх      
            BindingGroupName = inputBinding.groups;     //группа
            BindingName = inputBinding.name; //имя
        }
    }



    private PathData pathData;


    public int BindingIndex { get; set; }
    public string Asset { get; set; }
    public InputAction Action { get; set; }

    /// <summary>
    /// Хранит путь, устройство 
    /// </summary>
    public PathData PathData 
    {
        get => pathData;
        set
        {
            pathData = value; 
            ChangePath(pathData);
            BindingButtonText = PathData.GetPathText(pathData);
               // pathData.Device + "\n" +
               // pathData.DisplayName; 
            // BindingButtonText = Action.GetBindingDisplayString(); //имя из action (не обновляется)

        }
    } 
    
    public void ChangePath(PathData givenPathData)
    { 
	    // BindingButtonText =  binding.path; // весь путь  
        var aSplit = givenPathData.Path.Split('/');
        var b = new StringBuilder();
        foreach (string s in aSplit.Skip(2))
        {
            b.Append("/"+s);
        } 
        var correctPath = "<" + aSplit[1] + ">" + givenPathData.Usage + b; 
        var changedBinding = new InputBinding
        {
            path = correctPath,
            name = InputBinding.name,
            id = InputBinding.id,
            action = InputBinding.action,
            groups = InputBinding.groups,
            overridePath = "",
            interactions = InputBinding.interactions,
            isComposite = InputBinding.isComposite,
            isPartOfComposite = InputBinding.isPartOfComposite,
            overrideInteractions = InputBinding.overrideInteractions,
            overrideProcessors = InputBinding.overrideProcessors,
            processors = InputBinding.processors
        };
        InputBinding = changedBinding; 
       OnUpdated?.Invoke();
        
    }
}