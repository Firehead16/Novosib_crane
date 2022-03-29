using Core.Settings;
using UnityEngine;

namespace Core.Ui
{
	/// <summary>
    /// Реализация менеджера с настройками для канваса. Необходим для применения темы
    /// </summary>
    /// <typeparam name="TPanel"></typeparam>
    /// <typeparam name="TPanelsSettings"></typeparam>
    [RequireComponent(typeof(Canvas))]
    public abstract class CanvasControwWithSettings<TPanel, TPanelsSettings> : CanvasControl<TPanel>
        where TPanel : class, IPanel
        where TPanelsSettings : SubSettings<TPanelsSettings>
    {
	  
    }
}

