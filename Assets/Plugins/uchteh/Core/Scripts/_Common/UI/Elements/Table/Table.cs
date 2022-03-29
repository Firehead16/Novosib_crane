using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Table : MonoBehaviour
{
	public event Action<TableLine> OnSelectLine; 

	public TableLine CurLine { get; private set; }

	private List<TableLine> lines = new List<TableLine>();

	public void AddLine(TableLine line)
	{
		foreach (var button in line.Cells)
		{
			button.onClick.AddListener(() =>
			{
				SelectLine(line);
			});
		}
		lines.Add(line);
	}

	public void DeleteLine(TableLine line)
	{
		lines.Remove(line);
	}

	private void SelectLine(TableLine selectLine)
	{
		foreach (var line in lines)
		{
			DeselectLine(line);
		}

		foreach (var cell in selectLine.Cells)
		{
			cell.GetComponent<Image>().color = SettingsStorage.ThemeSettings.TableLineColor;
		}

		CurLine = selectLine;
		OnSelectLine?.Invoke(selectLine);
	}

	private void DeselectLine(TableLine selectLine)
	{
		foreach (var cell in selectLine.Cells)
		{
			cell.GetComponent<Image>().color = Color.white;
		}
	}

	/// <summary>
	/// Очистить таблицу
	/// </summary>
	public void Clear()
	{
		CurLine = null;

		if (lines.Any())
		{
			foreach (var line in lines)
			{
				foreach (var lineCell in line.Cells)
				{
					Destroy(lineCell.gameObject);
				}
			}
		}
		lines.Clear();
	}
}