using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FogParticleEffect : BaseParticleEffect
{
    /// <summary>
    ///  Will be called every time the light is assigned to a new particle
    /// </summary>
    protected override void AssignedToNewParticle()
    {
        this.lightSource.enabled = false;
    }

    /// <summary>
    ///  The particle effect only has a animaton and nothing else
    /// </summary>
    /// <param name="particle"></param>
    /// <param name="particleSystem"></param>
    protected override void HandleParticleUpdate(ParticleSystem.Particle particle, ParticleSystem particleSystem) {}
}
