using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

// This class manages the showing of the glass pane and fragments. 
public class HittableObjectParent : MonoBehaviour
{
    [Header("Numerical Values")]
    [SerializeField] float radius = 5.0f;
    [SerializeField] float explosionPower = 450.0f;

    [Header("Objects")]
    [SerializeField] private HittableObject primaryObject; // The gameobject containing the main mesh for the object
    [SerializeField] private List<GameObject> fragments;
    [SerializeField] private GameObject shatteredGlass;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] impacts;

    

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
      
    }

    public void Shatter(Vector3 hitLocation, Vector3 hitOrigin)
    {
        Destroy(primaryObject.gameObject);

        GameObject shards = Instantiate(shatteredGlass, transform.position, Quaternion.identity);
        shards.transform.localEulerAngles = new Vector3(0, -90, 0);


        Vector3 explosionPos = hitLocation;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionPower, explosionPos,radius,3.0f);
            }
        }

        // Leaving alternate glass shard force in a comment for now. Not sure which one I like better. 
/*        foreach (Transform shard in shards.transform)
        {
            shard.gameObject.GetComponent<Rigidbody>().AddForce((hitOrigin - hitLocation) * -glassShatterForceScalar,ForceMode.Impulse);
        }*/
    }
  

    public void PlayHitSound()
    {
        // Play the sound for this object getting hit at its current health level;
        audioSource.pitch = Random.Range(.7f, 1.3f);
        audioSource.PlayOneShot(GetCurrentImpactSound());
    }

    private AudioClip GetCurrentImpactSound()
    {
        return impacts[primaryObject.maxHealth -primaryObject.currentHealth];
    }
}
