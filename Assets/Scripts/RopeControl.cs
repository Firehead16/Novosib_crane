using Obi;
using UnityEngine;

[RequireComponent(typeof(ObiRope))]
[RequireComponent(typeof(ObiRopeCursor))]
public class RopeControl : MonoBehaviour
{
	public ObiRope Rope { get; private set; }
	public ObiRopeCursor Cursor { get; private set; }
	//public float Tension { get; private set; }

	private void Awake()
	{
		Rope = GetComponent<ObiRope>();
		Cursor = GetComponent<ObiRopeCursor>();
        //Tension = Rope.CalculateLength() / Rope.restLength;
    }
}
