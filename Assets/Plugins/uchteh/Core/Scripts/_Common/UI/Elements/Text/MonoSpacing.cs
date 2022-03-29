/*

Создает простой эффект одинарного интервала для компонентов текста пользовательского интерфейса.

Установите параметр интервала, чтобы отрегулировать интервал моно.
Отрицательные значения сжимают текст сильнее, чем обычно. Если зайти слишком далеко, это будет выглядеть странно.
Положительные значения расширяют текст больше, чем обычно. Это НЕ будет учитывать определенную вами текстовую область.
Нулевой интервал представляет шрифт без изменений.

Использует подсчет символов в текстовом свойстве текстового компонента и
сопоставление их с четверными, переданными через массив verts. Это действительно
довольно примитивно, но на данный момент я не вижу лучшего пути. Это означает, что
всякие вещи могут нарушить эффект ...

Этот компонент должен быть размещен в списке компонентов выше, чем любая другая вершина.
модификаторы, изменяющие общее количество вершин. EG, поместите это выше Shadow
или Эффекты контура. Если вы этого не сделаете, контур / тень не будет соответствовать положению
букв правильно. Однако если вы поместите эффект контура / тени вторым,
он просто будет работать с измененными вершинами из этого компонента и будет работать
как и ожидалось.

Этот компонент работает лучше всего, если вы не разрешаете автоматический перенос текста. Это также
взрывается за пределами заданной области текста. По сути, это дешевый и грязный эффект,
не умный движок верстки текста. Это не может повлиять на то, как Unity решит расстаться
ваши строки. Однако, если вы вручную используете разрывы строк, он должен их обнаружить и
функционируют более или менее так, как вы ожидаете.

Параметр интервала измеряется в пикселях, умноженных на размер шрифта. Это было
выбрано таким образом, чтобы при настройке размера шрифта визуальный интервал не изменялся
который вы набрали. В этом номере также есть масштабный коэффициент 1/100, чтобы
установите удобный регулируемый диапазон. На этот параметр нет ограничений,
но очевидно, что некоторые значения будут выглядеть довольно странно.

Этот компонент не работает с Rich Text. Вам не нужно помнить
отключите форматированный текст с помощью флажка, но поскольку он не может видеть, что делает
печатный символ, а что нет, он обычно неправильно считает символы, когда вы
используйте HTML-теги в своем тексте. Попробуйте, вы поймете, о чем я. Это не
полностью ломается, но на самом деле это не совсем то, что вам нужно.

*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Ui
{
    [AddComponentMenu("UI/Effects/Extensions/Mono Spacing")]
    [RequireComponent(typeof(Text))]
    [RequireComponent(typeof(RectTransform))]
    public class MonoSpacing : BaseMeshEffect
	{
		[SerializeField]
		private float m_spacing = 0f;
        public float HalfCharWidth = 1;
        public bool UseHalfCharWidth = false;

        private RectTransform rectTransform;
        private Text text;

		protected MonoSpacing() { }

        protected override void Awake()
        {
            text = GetComponent<Text>();
            if (text == null)
            {
                Debug.LogWarning("MonoSpacing: Missing Text component");
                return;
            }
            rectTransform = text.GetComponent<RectTransform>();
        }

        #if UNITY_EDITOR
        protected override void OnValidate()
		{
			Spacing = m_spacing;
			base.OnValidate();
		}
		#endif
		
		public float Spacing
		{
			get { return m_spacing; }
			set
			{
				if (m_spacing == value) return;
				m_spacing = value;
				if (graphic != null) graphic.SetVerticesDirty();
			}
		}

        public override void ModifyMesh(VertexHelper vh)
        {
            if (! IsActive()) return;

            List<UIVertex> verts = new List<UIVertex>();
            vh.GetUIVertexStream(verts);
			
			string[] lines = text.text.Split('\n');
			// Vector3  pos;
			float    letterOffset    = Spacing * (float)text.fontSize / 100f;
			float    alignmentFactor = 0;
			int      glyphIdx        = 0;
			
			switch (text.alignment)
			{
			case TextAnchor.LowerLeft:
			case TextAnchor.MiddleLeft:
			case TextAnchor.UpperLeft:
				alignmentFactor = 0f;
				break;
				
			case TextAnchor.LowerCenter:
			case TextAnchor.MiddleCenter:
			case TextAnchor.UpperCenter:
				alignmentFactor = 0.5f;
				break;
				
			case TextAnchor.LowerRight:
			case TextAnchor.MiddleRight:
			case TextAnchor.UpperRight:
				alignmentFactor = 1f;
				break;
			}
			
			for (int lineIdx=0; lineIdx < lines.Length; lineIdx++)
			{
				string line = lines[lineIdx];
                float lineOffset = (line.Length - 1) * letterOffset * (alignmentFactor) - (alignmentFactor - 0.5f) * rectTransform.rect.width;

                var offsetX = -lineOffset + letterOffset / 2 * (1 - alignmentFactor * 2);

				for (int charIdx = 0; charIdx < line.Length; charIdx++)
				{
					int idx1 = glyphIdx * 6 + 0;
					int idx2 = glyphIdx * 6 + 1;
					int idx3 = glyphIdx * 6 + 2;
					int idx4 = glyphIdx * 6 + 3;
                    int idx5 = glyphIdx * 6 + 4;
                    int idx6 = glyphIdx * 6 + 5;

                    // Check for truncated text (doesn't generate verts for all characters)
                    if (idx6 > verts.Count - 1) return;

                    UIVertex vert1 = verts[idx1];
                    UIVertex vert2 = verts[idx2];
                    UIVertex vert3 = verts[idx3];
                    UIVertex vert4 = verts[idx4];
                    UIVertex vert5 = verts[idx5];
                    UIVertex vert6 = verts[idx6];

                    // pos = Vector3.right * (letterOffset * (charIdx) - lineOffset);
                    float charWidth = (vert2.position - vert1.position).x;
                    var smallChar = UseHalfCharWidth && (charWidth < HalfCharWidth);

                    var smallCharOffset = smallChar ? -letterOffset/4 : 0;
                    
                    vert1.position += new Vector3(-vert1.position.x + offsetX + -.5f * charWidth + smallCharOffset, 0, 0);
                    vert2.position += new Vector3(-vert2.position.x + offsetX + .5f * charWidth + smallCharOffset, 0, 0);
                    vert3.position += new Vector3(-vert3.position.x + offsetX + .5f * charWidth + smallCharOffset, 0, 0);
                    vert4.position += new Vector3(-vert4.position.x + offsetX + .5f * charWidth + smallCharOffset, 0, 0);
                    vert5.position += new Vector3(-vert5.position.x + offsetX + -.5f * charWidth + smallCharOffset, 0, 0);
                    vert6.position += new Vector3(-vert6.position.x + offsetX + -.5f * charWidth + smallCharOffset, 0, 0);

                    if (smallChar)
                        offsetX += letterOffset / 2;
                    else
                        offsetX += letterOffset;

                    verts[idx1] = vert1;
					verts[idx2] = vert2;
					verts[idx3] = vert3;
					verts[idx4] = vert4;
                    verts[idx5] = vert5;
                    verts[idx6] = vert6;

                    glyphIdx++;
				}
				
				// Offset for carriage return character that still generates verts
				glyphIdx++;
			}
            vh.Clear();
            vh.AddUIVertexTriangleStream(verts);
        }
	}
}