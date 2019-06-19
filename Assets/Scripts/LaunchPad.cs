using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPad : MonoBehaviour
{

    [SerializeField]
    GameObject particleSystemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        particleSystemPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {

        //if (!particleSystem.isEmitting)
        // particleSystem.Play();
        var kineticEnergy = KineticEnergy(collision);
        var particleSize = kineticEnergy > 25 ? 25 : kineticEnergy;
        var startLifetime = kineticEnergy > 3 ? 3 : kineticEnergy / 5;
        var rateOverTime = kineticEnergy;
        var contactPoints = new List<Vector3>();
        foreach (ContactPoint contact in collision.contacts)
        {

            contactPoints.Add(contact.point);
        }
        var particle = Instantiate(particleSystemPrefab, GetMeanVector(contactPoints), Quaternion.identity);
        particle.transform.localScale += new Vector3(particleSize, 0, particleSize);
        var particleSystem = particle.GetComponent<ParticleSystem>();
        var main = particleSystem.main;
        main.startLifetime = startLifetime;
        main.duration = startLifetime;
        main.maxParticles = Mathf.RoundToInt(rateOverTime);
        main.stopAction = ParticleSystemStopAction.Destroy;
        var emissionModule = particleSystem.emission;
        emissionModule.rateOverTime = rateOverTime;
        main.playOnAwake = true;
        particle.SetActive(true);

    }
    private void OnCollisionExit(Collision collision)
    {
        //if (particleSystem.isEmitting)
        // particleSystem.Stop();
    }
    private static float KineticEnergy(Collision collision)
    {
        // mass in kg, velocity in meters per second, result is joules
        return 0.5f * collision.gameObject.GetComponent<Rigidbody>().mass * Mathf.Pow(collision.relativeVelocity.magnitude, 2);
    }

    private Vector3 GetMeanVector(List<Vector3> positions)
    {
        if (positions.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 meanVector = Vector3.zero;

        foreach (Vector3 pos in positions)
        {
            meanVector += pos;
        }

        return (meanVector / positions.Count);
    }
}
