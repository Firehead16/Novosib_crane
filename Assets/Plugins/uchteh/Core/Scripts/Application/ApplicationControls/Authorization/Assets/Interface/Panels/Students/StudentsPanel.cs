using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.Ui;

namespace Core.Global.Authorization
{
	[RequireComponent(typeof(CanvasGroupHideAndShowBehavior))]
	public class StudentsPanel : Panel, IAutorizationPanel
	{
		public event Action<Student, bool> OnAddOrEditStudentButtonClick;
		public event Action<Student> OnDeleteStudentButtonClick;
		public event Action<Student> OnChangeActiveStudentButtonClick;

		[SerializeField]
		private Table table = null;

		[SerializeField]
		private Button addButton = null, editButton = null, deleteButton = null, switchActivateButton = null;

		[SerializeField]
		private Button closeButton = null;

		private List<Student> students;

		public override void Initialize()
		{
			base.Initialize();

			addButton.onClick.AddListener(Add);
			editButton.onClick.AddListener(Edit);
			deleteButton.onClick.AddListener(Delete);
			switchActivateButton.onClick.AddListener(ActiveDisactive);

			closeButton.onClick.AddListener(Hide);
		}

		public void Show(List<Student> currentStudents)
		{
			base.Show();
			students = currentStudents;

			FillTable();
		}

		/// <summary>
		/// Заполнить таблицу с пользователями
		/// </summary>
		private void FillTable()
		{
			table.Clear();

			if (students.Count != 0)
			{
				int count = 0;
				foreach (var student in students)
				{
					count++;
					TableLine line = new TableLine(table, student, new List<Button>()
					{
						UiBuilder.CreateButton(table.transform.GetChild(0).transform, Vector3.one, count.ToString()),
						UiBuilder.CreateButton(table.transform.GetChild(1).transform, Vector3.one, student.Person.Login),
						UiBuilder.CreateButton(table.transform.GetChild(2).transform, Vector3.one, student.Person.Password),
						UiBuilder.CreateButton(table.transform.GetChild(3).transform, Vector3.one, student.Person.Surname + " " + student.Person.Name + " " + student.Person.Patronymic),
						UiBuilder.CreateButton(table.transform.GetChild(4).transform, Vector3.one, student.Person.IsActive?"Да":"Нет"),
					});
					table.AddLine(line);
				}
			}

			LayoutRebuilder.ForceRebuildLayoutImmediate(table.GetComponent<RectTransform>());
		}

		/// <summary>
		/// Добавить студента
		/// </summary>
		private void Add()
		{
			Student student = new Student()
			{
				Person = new Person()
				{
					Role = Role.Student,
					IsActive = true
				}
			};

			OnAddOrEditStudentButtonClick?.Invoke(student, true);
			Hide();
		}

		/// <summary>
		/// Редактировать студента
		/// </summary>
		private void Edit()
		{
			if (table.CurLine != null)
			{
				OnAddOrEditStudentButtonClick?.Invoke(table.CurLine.TableObject, false);
				Hide();
			}
		}

		/// <summary>
		/// Удалить студента
		/// </summary>
		private void Delete()
		{
			if (table.CurLine != null)
			{
				OnDeleteStudentButtonClick?.Invoke(table.CurLine.TableObject);
			}
		}

		/// <summary>
		/// Активировать / Деактивировать студента
		/// </summary>
		private void ActiveDisactive()
		{
			if (table.CurLine != null)
			{
				OnChangeActiveStudentButtonClick?.Invoke(table.CurLine.TableObject);
			}
		}
	}
}