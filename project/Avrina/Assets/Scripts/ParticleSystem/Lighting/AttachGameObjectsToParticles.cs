using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AttachGameObjectsToParticles : MonoBehaviour
{
    public BaseParticleEffect m_Prefab;

    private ParticleSystem m_ParticleSystem;
    private List<BaseParticleEffect> m_Instances = new List<BaseParticleEffect>();
    private ParticleSystem.Particle[] m_Particles;

    /// <summary>
    ///  Will get the particle system and sets all particles
    /// </summary>
    void Start()
    {
        m_ParticleSystem = GetComponent<ParticleSystem>();
        m_Particles = new ParticleSystem.Particle[m_ParticleSystem.main.maxParticles];
    }

    /// <summary>
    ///  Will set and unset the particle lights
    /// </summary>
    void LateUpdate()
    {
        int count = m_ParticleSystem.GetParticles(m_Particles);

        while (m_Instances.Count < count)
            m_Instances.Add(Instantiate(m_Prefab, m_ParticleSystem.transform));

        bool worldSpace = (m_ParticleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World);
        for (int i = 0; i < m_Instances.Count; i++)
        {
            var currentParticle = this.m_Particles[i];
            var currentLightInstance = this.m_Instances[i].GetComponent<BaseParticleEffect>();

            if (i < count)
            {
                currentLightInstance.gameObject.SetActive(true);
                currentLightInstance.UpdateParticle(i, currentParticle, this.m_ParticleSystem);

                if (worldSpace)
                    currentLightInstance.transform.position = currentParticle.position;
                else
                    currentLightInstance.transform.localPosition = currentParticle.position;
            }
            else
            {
                currentLightInstance.gameObject.SetActive(false);
            }
        }
    }
}
