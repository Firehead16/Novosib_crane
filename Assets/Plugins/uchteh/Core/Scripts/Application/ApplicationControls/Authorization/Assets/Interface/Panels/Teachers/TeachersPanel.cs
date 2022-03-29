using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.Ui;

namespace Core.Global.Authorization
{
	[RequireComponent(typeof(CanvasGroupHideAndShowBehavior))]
	public class TeachersPanel : Panel, IAutorizationPanel
	{
		public event Action<Teacher, bool> OnAddOrEditTeacherButtonClick;
		public event Action<Teacher> OnDeleteTeacherButtonClick;
		public event Action<Teacher> OnChangeActiveTeacherButtonClick;

		[SerializeField]
		private Table table = null;

		[SerializeField]
		private Button addButton = null;

		[SerializeField]
		private Button editButton = null;

		[SerializeField]
		private Button deleteButton = null;

		[SerializeField]
		private Button activeButton = null;

		[SerializeField]
		private Button closeButton = null;

		private List<Teacher> teachers;

		public override void Initialize()
		{
			base.Initialize();

			addButton.onClick.AddListener(Add);
			editButton.onClick.AddListener(Edit);
			deleteButton.onClick.AddListener(Delete);
			activeButton.onClick.AddListener(ActiveDisactive);

			closeButton.onClick.AddListener(Hide);
		}

		public void Show(List<Teacher> currentTeachers)
		{
			base.Show();

			teachers = currentTeachers;
			FillTable();
		}
		
		/// <summary>
		/// Заполнить таблицу
		/// </summary>
		private void FillTable()
		{
			table.Clear();

			if (teachers.Count != 0)
			{
				int count = 0;

				foreach (var teacher in teachers)
				{
					count++;
					TableLine line = new TableLine(table, teacher, new List<Button>()
					{
						UiBuilder.CreateButton(table.transform.GetChild(0).transform, Vector3.one, count.ToString()),
						UiBuilder.CreateButton(table.transform.GetChild(1).transform, Vector3.one, teacher.Person.Login),
						UiBuilder.CreateButton(table.transform.GetChild(2).transform, Vector3.one, teacher.Person.Password),
						UiBuilder.CreateButton(table.transform.GetChild(3).transform, Vector3.one, teacher.Person.Surname + " " + teacher.Person.Name + " " + teacher.Person.Patronymic),
						UiBuilder.CreateButton(table.transform.GetChild(4).transform, Vector3.one, teacher.Person.IsActive?"Да":"Нет"),
					});
					table.AddLine(line);
				}
			}

			LayoutRebuilder.ForceRebuildLayoutImmediate(table.GetComponent<RectTransform>());
		}

		/// <summary>
		/// Добавить учителя
		/// </summary>
		private void Add()
		{
			Teacher teacher = new Teacher()
			{
				Person = new Person()
				{
					Role = Role.Teacher,
					IsActive = true
				}
			};

			OnAddOrEditTeacherButtonClick?.Invoke(teacher, true);
			Hide();
		}

		/// <summary>
		/// Редактировать учителя
		/// </summary>
		private void Edit()
		{
			if (table.CurLine != null)
			{
				OnAddOrEditTeacherButtonClick?.Invoke(table.CurLine.TableObject, false);
				Hide();
			}
		}

		/// <summary>
		/// Удалить учителя
		/// </summary>
		private void Delete()
		{
			if (table.CurLine != null)
			{
				OnDeleteTeacherButtonClick?.Invoke(table.CurLine.TableObject);
			}
		}

		/// <summary>
		/// Активировать / Деактивировать учителя
		/// </summary>
		private void ActiveDisactive()
		{
			if (table.CurLine != null)
			{
				OnChangeActiveTeacherButtonClick?.Invoke(table.CurLine.TableObject);
			}
		}
	}
}
