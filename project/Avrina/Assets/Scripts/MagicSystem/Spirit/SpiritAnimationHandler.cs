using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpiritAnimationHandler : MonoBehaviour
{
    /// <summary>
    ///  Used to know which spirit 
    /// </summary>
    [SerializeField] public int spiritIndex;
    /// <summary>
    ///  Particle system of default spirit when it represents no specific element
    /// </summary>
    [SerializeField] private ParticleSystem noElementParticles;
    /// <summary>
    ///  Particle system of the fire element
    /// </summary>
    [SerializeField] private ParticleSystem fireParticles;
    /// <summary>
    ///  Particle system of the fire element
    /// </summary>
    [SerializeField] private ParticleSystem waterParticles;
    /// <summary>
    ///  Particle system of the fire element
    /// </summary>
    [SerializeField] private ParticleSystem windParticles;
    /// <summary>
    ///  Particle system of the fire element
    /// </summary>
    [SerializeField] private ParticleSystem earthParticles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
