using Core.Extensions;
using Core.Settings;

/// <summary>
/// Класс менеджера с настройками. Входящие в него менеджеры будут проинициализированы. Сам также будет проинициализирован.
/// </summary>
/// <typeparam name="TInitable">Интерфейс контроллеров, которые после загрузки будут проинициализированы</typeparam>
/// <typeparam name="TInitableSettings">Файл, в котором хранится список контроллеров</typeparam>
public abstract class InitableManagerWithSettins<TInitable, TInitableSettings> : BaseManagerWithSettins<TInitable, TInitableSettings>
	where TInitable : class, IInitialize, ICanInstanse
	where TInitableSettings : SubSettings<TInitableSettings>, IInitializebleList<TInitable>
{

	public bool IsInitialized { get; set; }

	protected override void OnControlsLoaded()
	{
		Initialize();
	}

	/// <summary>
	/// Инициализация этого менеджера и дочерних элементов
	/// </summary>
	public override void Initialize()
	{
		if (!IsRoot) return; //до него пока не дошла очередь, выходим
		base.Initialize();
		if (IsRoot) this.InitializeItems(Controls.Values);  //инициализируем дочерние
		this.DebugOnItemsInitialised();
		OnItemsInitialised();
	}



	protected abstract void OnItemsInitialised();
}