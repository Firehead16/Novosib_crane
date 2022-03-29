using Core.Gameplay.Interface;
using Core.Ui;

public sealed class MapCanvas : GameplayCanvasControl<MiniMapPanel>, IDesktopCanvasControl 
{
	public void Subscribe()
    {
    }

    public void UnSubscribe()
    {
        
    }

    protected override void OnItemsInitialised()
    {
    }

  
}


public sealed class MiniMapPanel : Panel, IGameplayPanel
{
    
}