using System.Collections.Generic;
using UnityEngine.UI;

public class TableLine
{
	private Table currentTable;

	public dynamic TableObject { get; }

	protected internal List<Button> Cells;

	public TableLine(Table table, dynamic tableObject, List<Button> cells)
	{
		currentTable = table;

		TableObject = tableObject;
		Cells = cells;
	}
}