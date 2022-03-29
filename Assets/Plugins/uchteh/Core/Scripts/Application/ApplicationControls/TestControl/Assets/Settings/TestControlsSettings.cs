using Sirenix.OdinInspector;
using Core.Settings;
using UnityEngine;

namespace Core.Testing
{
	[CreateAssetMenu(fileName = "TestControlsSettings", menuName = "Settings/TestControlsSettings")]
	public class TestControlsSettings : SubSettings<TestControlsSettings>
	{
		[Title("Время на тесты")]
		public int Time;

		[Title("Нужно ли перемешивать вопросы в тесте")]
		public bool NeedShuffle;


		[Header(header: "Префабы вопросов")]
		[Title("Вопрос с изображением")]
		public GameObject ImageQuestionForm;

		[Title("Вопрос с выбором нескольких вариантов ответа")]
		public GameObject ToggleQuestionForm;

		[Title("Вопрос с выбором одного варианта ответа")]
		public GameObject RadioBoxQuestionForm;

		[Title("Вопрос с вводом ключевой фразы")]
		public GameObject EnterAnswerQuestionForm;

		[Title("Вопрос с пропущенными словами")]
		public GameObject SpaceFillQuestionForm;

		[Title("Вопрос с последовательностью ответов")]
		public GameObject OrderListQuestionForm;


		[Header(header: "Префабы ответов")]
		[Title("OrderList ответ")]
		public GameObject OrderPrefab;

		public GameObject DragLineHandeler;
		public GameObject DropLineHandeler;

		[Title("Radiobox ответ")]
		public GameObject RadioBoxPrefab;

		public GameObject RadioBoxObject;

		[Title("Drag&Drop ответ")]
		public GameObject DrugAndDropPrefab;

		public GameObject LineContainer;
		public GameObject TextObject;
		public GameObject DragTextHandeler;
		public GameObject PlaceholderText;

		[Title("Toggle ответ")]
		public GameObject ToggleObject;

		[Title("EnterAnswer ответ")]
		public GameObject TextLinePrefab;

		public GameObject InputFieldObject;

		[Title("Image ответ")]
		public GameObject ImagePrefab;


		/// <summary>
		/// Получить объект вопроса
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public GameObject GetQuestionObject(int type)
		{
			GameObject questionObject = null;

			switch (type)
			{
				case (int)Question.Type.Image:
					questionObject = Instantiate(ImageQuestionForm);
					break;
				case (int)Question.Type.CheckBox:
					questionObject = Instantiate(ToggleQuestionForm);
					break;
				case (int)Question.Type.RadioBox:
					questionObject = Instantiate(RadioBoxQuestionForm);
					break;
				case (int)Question.Type.KeyPhrase:
					questionObject = Instantiate(EnterAnswerQuestionForm);
					break;
				case (int)Question.Type.SpaceFill:
					questionObject = Instantiate(SpaceFillQuestionForm);
					break;
				case (int)Question.Type.OrderList:
					questionObject = Instantiate(OrderListQuestionForm);
					break;
			}

			return questionObject;
		}
	}
}