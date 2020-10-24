using UnityEngine;

public class WaspParticleEffect : BaseParticleEffect
{
    /// <summary>
    ///  How long does it take until the max intesity of the light is reached
    /// </summary>
    [SerializeField] public float fadeInAndFadeOutTime = 1f;
    /// <summary>
    ///  What is the max intensity
    /// </summary>
    [SerializeField] private float maxIntensity = 3f;
    /// <summary>
    ///  What is the current timer till the max intensity is reached
    /// </summary>
    [HideInInspector] private float currentTime;
    /// <summary>
    ///  Will true if the exit animation was triggered
    /// </summary>
    [HideInInspector] public bool isExitAnimationRunning;

    
    /// <summary>
    ///  Will called after a new particle was assigned to the particle effect
    /// </summary>
    protected override void AssignedToNewParticle()
    {
        this.isExitAnimationRunning = false;
        this.currentTime = 0f;
        this.lightSource.intensity = 0f;
    }

    /// <summary>
    ///  Handles the particle update
    /// </summary>
    /// <param name="particle">Current assigned particle</param>
    protected override void HandleParticleUpdate(ParticleSystem.Particle particle, ParticleSystem particleSystem)
    {
        this.UpdateParticleEffectState(particle);

        if (this.isExitAnimationRunning)
        {
            this.currentTime -= Time.deltaTime;
            if (this.currentTime >= 0f)
            {
                this.lightSource.intensity = this.currentTime / this.fadeInAndFadeOutTime * this.maxIntensity;
            }
            else
            {
                this.lightSource.intensity = 0f;
            }
        }
        else
        {
            this.currentTime += Time.deltaTime;


            if (this.currentTime < this.fadeInAndFadeOutTime)
            {
                this.lightSource.intensity = this.currentTime / this.fadeInAndFadeOutTime * this.maxIntensity;
            }
            else
            {
                this.lightSource.intensity = this.maxIntensity;
            }
        }
    }

    /// <summary>
    ///  Will be called before the particle update is processed
    /// </summary>
    /// <param name="particle">Current assigned particle</param>
    private void UpdateParticleEffectState(ParticleSystem.Particle particle)
    {
        if (this.didAsignedParticleChangeLastUpdate)
        {
            this.currentTime = particle.startLifetime - particle.remainingLifetime;
        }

        if (particle.remainingLifetime < this.fadeInAndFadeOutTime + 0.1f && !this.isExitAnimationRunning)
        {
            this.isExitAnimationRunning = true;
            this.currentTime = this.fadeInAndFadeOutTime;
        }
    }
}
