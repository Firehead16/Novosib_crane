using UnityEngine;
using System;
using System.Collections;

namespace Obi{

	/**
	 * Small helper class that lets you specify Obi-only properties for rigidbodies.
	 */

	[ExecuteInEditMode]
	[RequireComponent(typeof(Rigidbody))]
	public class ObiRigidbody : ObiRigidbodyBase
	{
		private Rigidbody unityRigidbody;
        private Quaternion prevRotation;
        private Vector3 prevPosition;

        [SerializeField] private bool logVelocity = false;

        public override void Awake(){
			unityRigidbody = GetComponent<Rigidbody>();
            prevPosition = transform.position;
            prevRotation = transform.rotation;
            base.Awake();
		}

        private void UpdateKinematicVelocities(float stepTime)
        {
            // differentiate positions/orientations to get our own velocites for kinematic objects.
            // when calling Physics.Simulate, MovePosition/Rotation do not work correctly. Also useful for animations.
            if (unityRigidbody.isKinematic)
            {
                // differentiate positions to obtain linear velocity:
                unityRigidbody.velocity = (transform.position - prevPosition) / stepTime;

                // differentiate rotations to obtain angular velocity:
                Quaternion delta = transform.rotation * Quaternion.Inverse(prevRotation);
                unityRigidbody.angularVelocity = new Vector3(delta.x, delta.y, delta.z) * 2.0f / stepTime;
            }

            prevPosition = transform.position;
            prevRotation = transform.rotation;
        }

		public override void UpdateIfNeeded(float stepTime)
        {
            UpdateKinematicVelocities(stepTime);

            var rb = ObiColliderWorld.GetInstance().rigidbodies[handle.index];
            rb.FromRigidbody(unityRigidbody, kinematicForParticles);
            ObiColliderWorld.GetInstance().rigidbodies[handle.index] = rb;

        }

		public Vector3 addVel;

		/**
		 * Reads velocities back from the solver.
		 */
		public override void UpdateVelocities(Vector3 linearDelta, Vector3 angularDelta)
        {
			// kinematic rigidbodies are passed to Obi with zero velocity, so we must ignore the new velocities calculated by the solver:
			if (Application.isPlaying && !(unityRigidbody.isKinematic || kinematicForParticles))
			{
				//Debug.Log("LinearDelta" + linearDelta + " angularDelta " + angularDelta);
                //TODO очень погано (╯‵□′)╯︵┻━┻

                if(logVelocity) Debug.Log(linearDelta + " / " + GetComponent<Rigidbody>().velocity.y);

                if (Math.Abs(linearDelta.x) > 10000 || float.IsNaN(linearDelta.x)) linearDelta.x = 0;
                if (Math.Abs(linearDelta.y) > 10000 || float.IsNaN(linearDelta.y)) linearDelta.y = 0;
                if (Math.Abs(linearDelta.z) > 10000 || float.IsNaN(linearDelta.z)) linearDelta.z = 0;
                
                addVel = linearDelta;
                
                if (Math.Abs(angularDelta.x) > 10000 || float.IsNaN(angularDelta.x)) angularDelta.x = 0;
                if (Math.Abs(angularDelta.y) > 10000 || float.IsNaN(angularDelta.y)) angularDelta.y = 0;
                if (Math.Abs(angularDelta.z) > 10000 || float.IsNaN(angularDelta.z)) angularDelta.z = 0;
                angularDelta *= 0.25f;

                unityRigidbody.velocity += linearDelta;
                unityRigidbody.angularVelocity += angularDelta;
            }
        }
	}
}

