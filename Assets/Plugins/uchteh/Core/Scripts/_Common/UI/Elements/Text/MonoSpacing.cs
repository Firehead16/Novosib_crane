/*

������� ������� ������ ���������� ��������� ��� ����������� ������ ����������������� ����������.

���������� �������� ���������, ����� �������������� �������� ����.
������������� �������� ������� ����� �������, ��� ������. ���� ����� ������� ������, ��� ����� ��������� �������.
������������� �������� ��������� ����� ������, ��� ������. ��� �� ����� ��������� ������������ ���� ��������� �������.
������� �������� ������������ ����� ��� ���������.

���������� ������� �������� � ��������� �������� ���������� ���������� �
������������� �� � ����������, ����������� ����� ������ verts. ��� �������������
�������� ����������, �� �� ������ ������ � �� ���� ������� ����. ��� ��������, ���
������ ���� ����� �������� ������ ...

���� ��������� ������ ���� �������� � ������ ����������� ����, ��� ����� ������ �������.
������������, ���������� ����� ���������� ������. EG, ��������� ��� ���� Shadow
��� ������� �������. ���� �� ����� �� ��������, ������ / ���� �� ����� ��������������� ���������
���� ���������. ������ ���� �� ��������� ������ ������� / ���� ������,
�� ������ ����� �������� � ����������� ��������� �� ����� ���������� � ����� ��������
��� � ���������.

���� ��������� �������� ����� �����, ���� �� �� ���������� �������������� ������� ������. ��� �����
���������� �� ��������� �������� ������� ������. �� ����, ��� ������� � ������� ������,
�� ����� ������ ������� ������. ��� �� ����� �������� �� ��, ��� Unity ����� ����������
���� ������. ������, ���� �� ������� ����������� ������� �����, �� ������ �� ���������� �
������������� ����� ��� ����� ���, ��� �� ��������.

�������� ��������� ���������� � ��������, ���������� �� ������ ������. ��� ����
������� ����� �������, ����� ��� ��������� ������� ������ ���������� �������� �� ���������
������� �� �������. � ���� ������ ����� ���� ���������� ����������� 1/100, �����
���������� ������� ������������ ��������. �� ���� �������� ��� �����������,
�� ��������, ��� ��������� �������� ����� ��������� �������� �������.

���� ��������� �� �������� � Rich Text. ��� �� ����� �������
��������� ��������������� ����� � ������� ������, �� ��������� �� �� ����� ������, ��� ������
�������� ������, � ��� ���, �� ������ ����������� ������� �������, ����� ��
����������� HTML-���� � ����� ������. ����������, �� �������, � ��� �. ��� ��
��������� ��������, �� �� ����� ���� ��� �� ������ ��, ��� ��� �����.

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