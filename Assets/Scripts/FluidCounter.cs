using System;
using System.Collections.Generic;
using UnityEngine;
using Obi;

public class FluidCounter : MonoBehaviour
{
    //[System.Serializable]
    //public class ScoreChangedEvent : UnityEvent<int,int> { }

    public ObiSolver solver;
    public ObiEmitter emitter;
    public ObiCollider finishLine;
    //public ParticleSystem sparks;

    //public FluidColorizer[] colorizers;

    [NonSerialized]
    public HashSet<int> finishedParticles = new HashSet<int>();
    HashSet<int> coloredParticles = new HashSet<int>();

    //public float SumParticles;
    void Start()
    {
        solver.OnCollision += Solver_OnCollision;
        emitter.OnEmitParticle += Emitter_OnEmitParticle;
    }

    public void SetMeshRenderer(bool state)
    {
        MeshRenderer m = GetComponent<MeshRenderer>();
        m.enabled = state;
    }

    private void OnDestroy()
    {
        solver.OnCollision -= Solver_OnCollision;
        emitter.OnEmitParticle -= Emitter_OnEmitParticle;
    }

    void Emitter_OnEmitParticle(ObiEmitter em, int particleIndex)
    {
        int k = emitter.solverIndices[particleIndex];
        solver.userData[k] = solver.colors[k];
    }

    private void Solver_OnCollision(ObiSolver s, ObiSolver.ObiCollisionEventArgs e)
    {
        var world = ObiColliderWorld.GetInstance();
        foreach (Oni.Contact contact in e.contacts)
        {
            // look for actual contacts only:
            if (contact.distance < 0.01f)
            {
                var col = world.colliderHandles[contact.bodyB].owner;
                //if (colorizers[0].collider == col)
                //{
                //    solver.userData[contact.particle] = colorizers[0].color;
                //    if (coloredParticles.Add(contact.particle))
                //        UpdateScore(finishedParticles.Count, coloredParticles.Count);
                //}
                //else if (colorizers[1].collider == col)
                //{
                //    solver.userData[contact.particle] = colorizers[1].color;
                //    if (coloredParticles.Add(contact.particle))
                //        UpdateScore(finishedParticles.Count, coloredParticles.Count);
                //}
                //else 
                if (finishLine == col)
                {
                    if (finishedParticles.Add(contact.bodyA))
                        UpdateScore(finishedParticles.Count);
                }

            }
        }
    }

    void LateUpdate()
    {
        for (int i = 0; i < emitter.solverIndices.Length; ++i)
        {
            int k = emitter.solverIndices[i];
            emitter.solver.colors[k] = emitter.solver.userData[k];
        }
    }

    public void UpdateScore(int finishedParticles)
    {
        //if(finishedParticles > 0) sparks.Play(); 
        //Debug.Log("Congrats: " + finishedParticles);
    }
}
