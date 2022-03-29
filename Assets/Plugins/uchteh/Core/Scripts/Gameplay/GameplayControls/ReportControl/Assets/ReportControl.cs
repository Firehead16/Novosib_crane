using System;
using System.Collections.Generic;
using System.Linq;
using Core.Settings;
using UnityEngine;

namespace Core.Settings
{
	public static partial class Messages
	{
		public enum Report
		{
			StartReport,
			FinishReport,
			AddLineInReport,
			AddErrorInReport,
			AddRightActionInReport
		}
	}
}

namespace Core.Gameplay.Reporting
{
	public class ReportControl : BaseControlMethods, IReport
	{
		public enum LineType
		{
			Title,
			UserName,
			StartTime,
			FinistTime,
			ErrorCount,
			ErrorAction,
			RightAction,
			FinishTask,
			Time,
			SimpleString,
			NotFinishTask,
		}

		[Serializable]
		public struct Line
		{
			[SerializeField]
			public LineType Type;

			[SerializeField]
			public DateTime Time;

			[SerializeField]
			public string Value;

			public Line(LineType type, DateTime time) : this()
			{
				Type = type;
				Time = time;
				Value = "";
			}

			public Line(LineType type, DateTime time, string value)
			{
				Type = type;
				Time = time;
				Value = value;
			}

			public Line(LineType type, string value)
			{
				Time = DateTime.Now;
				Type = type;
				Value = value;
			}

		}

		public bool IsInitialized { get; set; }

		/// <summary>
		/// Пользователь
		/// </summary>
		private Person person;

		[SerializeField]
		private static List<Line> lines = new List<Line>();

		[SerializeField]
		private static List<Line> errors = new List<Line>();

		/// <summary>
		/// Количество ошибок
		/// </summary>
		private static int errorCount;

		/// <summary>
		/// Логирование начато
		/// </summary>
		private static bool isStart;

		public override void Notify(Message message)
		{
			switch (message.Type)
			{
				case Messages.Report.StartReport:
					string title = (string) message.Content[0];
					StartReport(title);
					break;
				case Messages.Report.FinishReport:
					FinishReport();
					break;
				case Messages.Report.AddLineInReport:
					string lineName = (string) message.Content[0];
					AddLine(lineName);
					break;
				case Messages.Report.AddRightActionInReport:
					string rightActionName = (string)message.Content[0];
					AddRightAction(rightActionName);
					break;
				case Messages.Report.AddErrorInReport:
					string errorName = (string)message.Content[0];
					AddErrorAction(errorName);
					break;
			}
		}

		/// <summary>
		/// Начать логирование
		/// </summary>
		private void StartReport(string title)
		{
			if (!isStart)
			{
				isStart = true;
				

				AddTitle(title);
				AddPerson();
				AddStartTime();

				Send(null, new Message(Messages.Report.StartReport, title));
			}
		}

		/// <summary>
		/// Закончить логирование
		/// </summary>
		private void FinishReport()
		{
			if (isStart)
			{
				isStart = false;
				AddFinishTime();
				AddErrors();

				Send(null, new Message(Messages.Report.FinishReport, OutPut()));

				ClearReport();
			}
		}

		/// <summary>
		/// Очистить логирование
		/// </summary>
		private void ClearReport()
		{
			lines.Clear();
			errors.Clear();
			errorCount = 0;
		}

		/// <summary>
		/// Добавить строку в лог
		/// </summary>
		/// <param name="lineName">Строка</param>
		private void AddLine(string lineName)
		{
			if (isStart)
			{
				lines.Add(new Line(LineType.SimpleString, DateTime.Now, lineName));
			}
		}

		/// <summary>
		/// Добавить правильное действие в лог
		/// </summary>
		/// <param name="lineName">Строка</param>
		private void AddRightAction(string lineName)
		{
			if (isStart)
			{
				lines.Add(new Line(LineType.RightAction, DateTime.Now, lineName));
			}
		}

		/// <summary>
		/// Добавить ошибку
		/// </summary>
		/// <param name="errorName">Описание ошибки</param>
		private void AddErrorAction(string errorName)
		{
			if (isStart)
			{
				errorCount++;
				errors.Add(new Line(LineType.ErrorAction, DateTime.Now, errorName));
			}
		}
		
		/// <summary>
		/// Получить лог
		/// </summary>
		/// <returns></returns>
		private string OutPut()
		{
			string message = "";

			foreach (var line in lines)
			{
				switch (line.Type)
				{
					case LineType.Title:
						message += "Название задания: " + line.Value + "\n";
						break;
					case LineType.UserName:
						message += "Пользователь: " + line.Value + "\n";
						break;
					case LineType.StartTime:
						message += line.Time + " Время начала выполнения задания" + "\n";
						break;
					case LineType.FinistTime:
						message += line.Time + " Время окончания выполнения задания"  + "\n";
						break;
					case LineType.ErrorCount:
						message += "Количество ошибок: " + line.Value + "\n";
						break;
					case LineType.ErrorAction:
						message += line.Time + " Ошибка: " + line.Value + "\n";
						break;
					case LineType.RightAction:
						message += line.Time + " Действие пользователя: " + line.Value + "\n";
						break;
					case LineType.SimpleString:
						message += line.Value + "\n";
						break;
					case LineType.Time:
						message += "Количество прошедшего времени: " + line.Value + "\n";
						break;
					case LineType.FinishTask:
						message += line.Time + " Задание завершено: " + line.Value + "\n";
						break;
					case LineType.NotFinishTask:
						message += line.Time + " Задание не завершено: " + line.Value + "\n";
						break;
				
				}
			}

			return message;
		}

		/// <summary>
		/// Добавить название лога
		/// </summary>
		private void AddTitle(string title)
		{

			lines.Add(new Line()
			{
				Type = LineType.Title,
				Value = title
			});
		}

		/// <summary>
		/// Добавить пользователя
		/// </summary>
		private void AddPerson()
		{
			if (person != null)
			{
				lines.Add(new Line()
				{
					Type = LineType.UserName,
					Value = person.Surname + " " + person.Name + " " + person.Patronymic
				});
			}
		}

		/// <summary>
		/// Добавить время начала прохождения
		/// </summary>
		private void AddStartTime()
		{
			lines.Add(new Line()
			{
				Type = LineType.StartTime,
				Time = DateTime.Now
			});
		}

		/// <summary>
		/// Добавить время окончания прохождения
		/// </summary>
		private void AddFinishTime()
		{
			lines.Add(new Line()
			{
				Type = LineType.FinistTime,
				Time = DateTime.Now
			});
		}

		/// <summary>
		/// Получить ошибки
		/// </summary>
		private void AddErrors()
		{
			lines.Add(new Line()
			{
				Type = LineType.ErrorCount,
				Value = errorCount.ToString()
			});

			if (errors.Any())
			{
				lines.Add(new Line()
				{
					Type = LineType.SimpleString,
					Value = "Ошибки при прохождении:"
				});

				lines.AddRange(errors);
			}
		}

	}
}
