using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.Ui;

namespace Core.Global.Authorization
{
	[RequireComponent(typeof(CanvasGroupHideAndShowBehavior))]
	public class AdministratorsPanel : Panel, IAutorizationPanel
	{
		public event Action<Administrator, bool> OnAddOrEditAdministratorsButtonClick;
		public event Action<Administrator> OnDeleteAdministratorsButtonClick;
		public event Action<Administrator> OnChangeActiveAdministratorsButtonClick;

		[SerializeField]
		private Table table = null;

		[SerializeField]
		private Button addButton = null, editButton = null, deleteButton = null, switchActivateButton = null;

		[SerializeField]
		private Button closeButton = null;

		private List<Administrator> administrators;

		public override void Initialize()
		{
			base.Initialize();

			addButton.onClick.AddListener(Add);
			editButton.onClick.AddListener(Edit);
			deleteButton.onClick.AddListener(Delete);
			switchActivateButton.onClick.AddListener(ActiveDisactive);

			closeButton.onClick.AddListener(Hide);
		}

		public void Show(List<Administrator> currentAdministrators)
		{
			base.Show();
			administrators = currentAdministrators;

			FillTable();
		}
		
		/// <summary>
		/// Заполнить таблицу
		/// </summary>
		private void FillTable()
		{
			table.Clear();

			if (administrators.Count != 0)
			{
				int count = 0;

				foreach (var administrator in administrators)
				{
					count++;

					TableLine line = new TableLine(table, administrator, new List<Button>()
					{
						UiBuilder.CreateButton(table.transform.GetChild(0).transform, Vector3.one, count.ToString()),
						UiBuilder.CreateButton(table.transform.GetChild(1).transform, Vector3.one, administrator.Person.Login),
						UiBuilder.CreateButton(table.transform.GetChild(2).transform, Vector3.one, administrator.Person.Password),
						UiBuilder.CreateButton(table.transform.GetChild(3).transform, Vector3.one, administrator.Person.Surname + " " + administrator.Person.Name + " " + administrator.Person.Patronymic),
						UiBuilder.CreateButton(table.transform.GetChild(4).transform, Vector3.one, administrator.Person.IsActive?"Да":"Нет"),
					});
					table.AddLine(line);

				}
			}

			LayoutRebuilder.ForceRebuildLayoutImmediate(table.GetComponent<RectTransform>());
		}

		/// <summary>
		/// Добавить администратора
		/// </summary>
		private void Add()
		{
			Administrator administrator = new Administrator()
			{
				Person = new Person()
				{
					Role = Role.Administrator,
					IsActive = true
				}
			};

			OnAddOrEditAdministratorsButtonClick?.Invoke(administrator, true);
			Hide();
		}

		/// <summary>
		/// Редактировать администратора
		/// </summary>
		private void Edit()
		{
			if (table.CurLine != null)
			{
				OnAddOrEditAdministratorsButtonClick?.Invoke(table.CurLine.TableObject, false);
				Hide();
			}
		}

		/// <summary>
		/// Удалить администратора
		/// </summary>
		private void Delete()
		{
			if (table.CurLine != null)
			{
				OnDeleteAdministratorsButtonClick?.Invoke(table.CurLine.TableObject);
			}
		}

		/// <summary>
		/// Активировать / Дизактивировать администратора
		/// </summary>
		private void ActiveDisactive()
		{
			if (table.CurLine != null)
			{
				OnChangeActiveAdministratorsButtonClick?.Invoke(table.CurLine.TableObject);
			}
		}
	}
}
