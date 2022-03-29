using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Extensions;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Core.Ui.Extensions;

namespace Core.Testing
{
	public class OrderPosition : IQuestionAnswer
	{
		public bool IsNewInstanse { get; set; }
		public bool IsLoaded { get; set; }

		public ReorderableListElement ListElement;

		public AnswerOrderList AnswerOrderList;

		public int Position;

		public int CurrentPosition;

		public OrderPosition(int position, ReorderableListElement listElementHandler, AnswerOrderList answerOrderList)
		{
			ListElement = listElementHandler;
			AnswerOrderList = answerOrderList;
			Position = position;
			CurrentPosition = position;
		}

		public void ChangePosition(int index)
		{
			ListElement.transform.SetSiblingIndex(index);
			CurrentPosition = index;
		}

		public void RefreshPosition()
		{
			CurrentPosition = ListElement.transform.GetSiblingIndex();
		}

		public bool IsElementRightPosition()
		{
			return Position == CurrentPosition;
		}

		public void Load() { }

		public void Unload() { }
	}

	public class OrderListQuestionForm : QuestionForm<OrderPosition>
	{
		[SerializeField]
		private ReorderableList reorderableList = null;

		[ShowInInspector]
		private List<OrderPosition> positions;

		public override void Load()
		{
			base.Load();

			Title.text = Question.Name;
			ShuffleOrder();
			reorderableList.Refresh();
		}

		private void Update()
		{
			Controls?.Values.ForEach(c => c.RefreshPosition());
		}
		
		protected override IReadOnlyCollection<OrderPosition> GetChildControls()
		{
			var answers = TestControl.GetOrderList(QuestionId).OrderBy(a => a.StringOrder).ToList();
			positions = new List<OrderPosition>();
			for (var index = 0; index < answers.Count; index++)
			{
				var answer = answers[index];
				var listElement = GetDragLine(answer);
				positions.Add(new OrderPosition(index, listElement, answer));
			}
			return positions;
		}

		protected override string GetControlKey(OrderPosition loadedControl)
		{
			return loadedControl.Position.ToString("D");
		}

		private void ShuffleOrder()
		{
			int index = 0;
			foreach (var i in MathExtensions.RandomizeOrder(Controls.Count))
			{
				OrderPosition orderPosition = Controls[index.ToString()];
				orderPosition.ChangePosition(i);
				index++;
			}
		}

		private ReorderableListElement GetDragLine(AnswerOrderList answer)
		{
			var dragLine = Instantiate(TestControlsSettings.Default().DragLineHandeler)
				.GetComponent<ReorderableListElement>();
			dragLine.transform.SetParent(AnswerArea);
			dragLine.transform.GetComponentInChildren<Text>().text = answer.StringValue;
			return dragLine;
		}
		
		/// <summary>
		/// Проверить правильность ответа на вопрос
		/// </summary>
		/// <returns></returns>
		public override bool CheckIfRight()
		{
			foreach (var op in Controls.Values)
			{
				if (!op.IsElementRightPosition()) return false;
			}
			return true;
		}

		/// <summary>
		/// Добавить вопрос в пройденные
		/// </summary>
		/// <returns></returns>
		public override PassedQuestion SetInLog()
		{
			var currentAnswer = new StringBuilder();

			foreach (var value in Controls.Values
				.OrderBy(x => x.CurrentPosition)
				.Select(x => x.AnswerOrderList.StringValue))
			{
				currentAnswer.AppendLine(value);
			}

			PassedQuestion passedQuestion = new PassedQuestion
			{
				QuestionId = Question.QuestionId,
				Answer = currentAnswer.ToString(),
				IsRight = CheckIfRight(),
			};
			return passedQuestion;
		}
	}
}