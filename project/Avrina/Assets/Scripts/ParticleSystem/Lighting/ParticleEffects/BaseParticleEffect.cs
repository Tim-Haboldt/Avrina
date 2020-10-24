using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public abstract class BaseParticleEffect : MonoBehaviour
{
    /// <summary>
    ///  Stores the id of the last particle assigned to
    /// </summary>
    private int lastParticleId = -1;
    /// <summary>
    ///  Stores the light element of the particle effect
    /// </summary>
    protected Light2D lightSource;

    protected bool didAsignedParticleChangeLastUpdate { private set; get; }


    /// <summary>
    ///  Will be called if the particle is set active
    /// </summary>
    private void OnEnable()
    {
        if (!this.lightSource)
        {
            lightSource = this.GetComponent<Light2D>();
        }

        this.didAsignedParticleChangeLastUpdate = true;
        this.AssignedToNewParticle();
    }

    /// <summary>
    ///  Will be called every update tick of unity
    /// </summary>
    /// <param name="currentParticleId">Current particle id</param>
    /// <param name="particle">The currently assigned particle</param>
    public void UpdateParticle(int currentParticleId, ParticleSystem.Particle particle, ParticleSystem particleSystem)
    {
        if (this.lastParticleId != currentParticleId)
        {
            this.didAsignedParticleChangeLastUpdate = true;
            this.AssignedToNewParticle();
            this.lastParticleId = currentParticleId;
        }

        this.HandleParticleUpdate(particle, particleSystem);

        this.didAsignedParticleChangeLastUpdate = false;
    }

    /// <summary>
    ///  Will be called every time the light was asigned to a new particle
    /// </summary>
    protected abstract void AssignedToNewParticle();
    /// <summary>
    ///  Will be called every update tick of unity
    /// </summary>
    /// <param name="particle">The currently assigned particle</param>
    protected abstract void HandleParticleUpdate(ParticleSystem.Particle particle, ParticleSystem particleSystem);
}
