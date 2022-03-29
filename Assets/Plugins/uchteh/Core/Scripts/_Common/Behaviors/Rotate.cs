using UnityEngine;

namespace Core
{
	/// <summary>
	/// Поворот объекта
	/// </summary>
	public class Rotate : MonoBehaviour
	{
		[SerializeField]
		private bool xRotate = false;

		[SerializeField]
		private bool yRotate = false;

		[SerializeField]
		private bool zRotate = false;

		[SerializeField] 
		private float xAngel = 0;

		[SerializeField]
		private float yAngel = 0;

		[SerializeField]
		private float zAngel= 0;

		private void Update()
		{
			if (xRotate)
			{
				transform.transform.RotateAround(transform.position, transform.right, xAngel);
			}

			if (yRotate)
			{
				transform.RotateAround(transform.position, transform.up, yAngel);
			}

			if (zRotate)
			{
				transform.RotateAround(transform.position, transform.forward, zAngel);
			}
		}
	}

}