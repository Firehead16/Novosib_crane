using Sirenix.OdinInspector;
using UnityEngine;

public abstract class ParticleControl : SerializedMonoBehaviour
{
    protected ParticleSystem Particles;

    [SerializeField,BoxGroup("Цвета")]
    private Color color;
    public Color Color
    {
        protected get { return color; }
        set { color = value; }
    }

    [SerializeField]
    private Material material;

    private bool isLoaded;

    public Material Material
    {
        set { material = value; }
    }


    public virtual void Load()
    {
        Particles = GetPaticleSystem();
        isLoaded = true;
    }


    protected abstract ParticleSystem.Particle[] Calculate();

    private ParticleSystem GetPaticleSystem()
    {
        var paticleSystem = gameObject.GetComponent<ParticleSystem>();
        if (paticleSystem == null) paticleSystem = gameObject.AddComponent<ParticleSystem>();
        var emission = paticleSystem.emission;
        emission.enabled = false;

        var shape = paticleSystem.shape;
        shape.enabled = false; paticleSystem.Stop();

        SetMain(paticleSystem.main);
        SetRenderer(paticleSystem.GetComponent<ParticleSystemRenderer>());

        return paticleSystem;
    }

    protected virtual void SetMain(ParticleSystem.MainModule main)
    {
        main.playOnAwake = false;
        main.loop = false;
        main.startSize3D = true;
        main.startColor = color;
    }

    protected virtual void SetRenderer(ParticleSystemRenderer render)
    {
        render.material = material;
        render.renderMode = ParticleSystemRenderMode.Billboard;
        render.alignment = ParticleSystemRenderSpace.World;
        render.pivot = Vector3.zero;
        render.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
    }

    protected void Draw()
    {
        if (isLoaded)
            Draw(Particles, Calculate());
    }

    private void Draw(ParticleSystem curParticleSystem, ParticleSystem.Particle[] points)
    {
	    curParticleSystem.SetParticles(points, points.Length);
    }
}