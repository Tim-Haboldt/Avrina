using UnityEngine;

public class BurnParticleEffect : BaseParticleEffect
{
    /// <summary>
    ///  How intense will the lighting be compared to the size of the burning effect
    /// </summary>
    [SerializeField] private float intensitySizeRelation = 0.5f;


    /// <summary>
    ///  Will be called every time the light is assigned to a new particle
    /// </summary>
    protected override void AssignedToNewParticle()
    {
        this.lightSource.intensity = 0f;
    }

    /// <summary>
    ///  Handles the particle update
    /// </summary>
    /// <param name="particle">Current assigned particle</param>
    protected override void HandleParticleUpdate(ParticleSystem.Particle particle, ParticleSystem particleSystem)
    {
        this.lightSource.intensity = this.intensitySizeRelation * particle.GetCurrentSize(particleSystem);
        this.lightSource.color = particle.GetCurrentColor(particleSystem);
    }
}
