using System.Collections.Generic;
using UnityEngine;

namespace Core.Ui.ColorPicker
{
	[ExecuteInEditMode]
	public class ColorPickerControl : MonoBehaviour
	{
		private float hue = 0;
		private float saturation = 0;
		private float brightness = 0;

		private float red = 0;
		private float green = 0;
		private float blue = 0;

		private float alpha = 1;

		public ColorChangedEvent onValueChanged = new ColorChangedEvent();
		public HSVChangedEvent onHSVChanged = new HSVChangedEvent();

		[SerializeField]
		bool hsvSlidersOn = true;

		[SerializeField]
		List<GameObject> hsvSliders = new List<GameObject>();

		[SerializeField]
		bool rgbSlidersOn = true;

		[SerializeField]
		List<GameObject> rgbSliders = new List<GameObject>();

		[SerializeField]
		GameObject alphaSlider = null;

		private void Start()
		{
			SendChangedEvent();
		}

		private void Update()
		{
#if UNITY_EDITOR
			SetHSVSlidersOn(hsvSlidersOn);
			SetRGBSlidersOn(rgbSlidersOn);
#endif
		}

		public void SetHSVSlidersOn(bool value)
		{
			hsvSlidersOn = value;

			foreach (var item in hsvSliders)
				item.SetActive(value);

			if (alphaSlider)
				alphaSlider.SetActive(hsvSlidersOn || rgbSlidersOn);
		}

		public void SetRGBSlidersOn(bool value)
		{
			rgbSlidersOn = value;
			foreach (var item in rgbSliders)
				item.SetActive(value);

			if (alphaSlider)
				alphaSlider.SetActive(hsvSlidersOn || rgbSlidersOn);
		}

		public Color CurrentColor
		{
			get
			{
				return new Color(red, green, blue, alpha);
			}
			set
			{
				if (CurrentColor == value)
					return;

				red = value.r;
				green = value.g;
				blue = value.b;
				alpha = value.a;

				RgbChanged();

				SendChangedEvent();
			}
		}

		public float H
		{
			get
			{
				return hue;
			}
			set
			{
				if (hue == value)
					return;

				hue = value;

				HsvChanged();

				SendChangedEvent();
			}
		}

		public float S
		{
			get
			{
				return saturation;
			}
			set
			{
				if (saturation == value)
					return;

				saturation = value;

				HsvChanged();

				SendChangedEvent();
			}
		}

		public float V
		{
			get
			{
				return brightness;
			}
			set
			{
				if (brightness == value)
					return;

				brightness = value;

				HsvChanged();

				SendChangedEvent();
			}
		}

		public float R
		{
			get
			{
				return red;
			}
			set
			{
				if (red == value)
					return;

				red = value;

				RgbChanged();

				SendChangedEvent();
			}
		}

		public float G
		{
			get
			{
				return green;
			}
			set
			{
				if (green == value)
					return;

				green = value;

				RgbChanged();

				SendChangedEvent();
			}
		}

		public float B
		{
			get
			{
				return blue;
			}
			set
			{
				if (blue == value)
					return;

				blue = value;

				RgbChanged();

				SendChangedEvent();
			}
		}

		private float A
		{
			get
			{
				return alpha;
			}
			set
			{
				if (alpha == value)
					return;

				alpha = value;

				SendChangedEvent();
			}
		}

		private void RgbChanged()
		{
			HsvColor color = HSVUtil.ConvertRgbToHsv(CurrentColor);

			hue = color.NormalizedH;
			saturation = color.NormalizedS;
			brightness = color.NormalizedV;
		}

		private void HsvChanged()
		{
			Color color = HSVUtil.ConvertHsvToRgb(hue * 360, saturation, brightness, alpha);

			red = color.r;
			green = color.g;
			blue = color.b;
		}

		private void SendChangedEvent()
		{
			onValueChanged.Invoke(CurrentColor);
			onHSVChanged.Invoke(hue, saturation, brightness);
		}

		public void AssignColor(ColorValues type, float value)
		{
			switch (type)
			{
				case ColorValues.R:
					R = value;
					break;
				case ColorValues.G:
					G = value;
					break;
				case ColorValues.B:
					B = value;
					break;
				case ColorValues.A:
					A = value;
					break;
				case ColorValues.Hue:
					H = value;
					break;
				case ColorValues.Saturation:
					S = value;
					break;
				case ColorValues.Value:
					V = value;
					break;
				default:
					break;
			}
		}

		public float GetValue(ColorValues type)
		{
			switch (type)
			{
				case ColorValues.R:
					return R;
				case ColorValues.G:
					return G;
				case ColorValues.B:
					return B;
				case ColorValues.A:
					return A;
				case ColorValues.Hue:
					return H;
				case ColorValues.Saturation:
					return S;
				case ColorValues.Value:
					return V;
				default:
					throw new System.NotImplementedException("");
			}
		}
	}
}