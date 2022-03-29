using Core.Ui;
using UnityEngine;

[ExecuteInEditMode]
public abstract class UiManagerObject : MonoBehaviour, ILoadable
{
	protected ThemeSettings ThemeSettings;

	public virtual void Load()
	{
		
	}

	protected void OnEnable()
	{
		Load();

		if (ThemeSettings == null)
		{
			try
			{
				ThemeSettings = SettingsStorage.ThemeSettings;
			}

			catch
			{
				Debug.Log("No UI Manager found. Assign it manually, otherwise you'll get errors about it.", this);
			}
		}
	}

	private void LateUpdate()
	{
		if (ThemeSettings == null)
		{
			ThemeSettings = SettingsStorage.ThemeSettings;
		}
		else if (ThemeSettings != SettingsStorage.ThemeSettings)
		{
			ThemeSettings = SettingsStorage.ThemeSettings;
		}
		else
		{
		
			if (ThemeSettings.enableDynamicUpdate)
			{
				UpdateTheme();
			}
		}
	}

	protected abstract void UpdateTheme();
}
