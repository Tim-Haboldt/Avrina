using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AttachGameObjectsToParticles : MonoBehaviour
{
    public BaseParticleEffect m_Prefab;

    private ParticleSystem m_ParticleSystem;
    private List<BaseParticleEffect> m_Instances = new List<BaseParticleEffect>();
    private ParticleSystem.Particle[] m_Particles;

    // Start is called before the first frame update
    void Start()
    {
        m_ParticleSystem = GetComponent<ParticleSystem>();
        m_Particles = new ParticleSystem.Particle[m_ParticleSystem.main.maxParticles];
    }

    // Update is called once per frame
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

            if (currentParticle.remainingLifetime < 0.05f)
            {
                currentLightInstance.gameObject.SetActive(false);
                continue;
            }

            if (i < count)
            {
                if (worldSpace)
                    currentLightInstance.transform.position = currentParticle.position;
                else
                    currentLightInstance.transform.localPosition = currentParticle.position;

                currentLightInstance.gameObject.SetActive(true);
                currentLightInstance.UpdateParticle(i, currentParticle);
            }
            else
            {
                currentLightInstance.gameObject.SetActive(false);
            }
        }
    }
}
