using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// This class manages the showing of the glass pane and fragments. 
public class HittableObjectParent : MonoBehaviour
{

    [SerializeField] private HittableObject primaryObject; // The gameobject containing the main mesh for the object
    [SerializeField] private List<GameObject> fragments;
    [SerializeField] private float fragmentScatter = 1.5f; 
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] impacts;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
      
    }

    public void Shatter(Vector3 hitLocation, Vector3 hitOrigin)
    {
        Destroy(primaryObject.gameObject);
        fragments.ForEach(fragment =>
        {
            Rigidbody fragRB = fragment.gameObject.GetComponent<Rigidbody>();
/*            fragment.gameObject.SetActive(true);*/
            fragRB.isKinematic = false;
            Vector3 scatter = new Vector3(Random.Range(-fragmentScatter, fragmentScatter), Random.Range(-fragmentScatter, fragmentScatter), Random.Range(-fragmentScatter, fragmentScatter));
            fragRB.AddForce((transform.position - hitOrigin) + scatter, ForceMode.Impulse);
        }
           );
        Debug.Log($"{this.name} has shattered!");
    }
    public void ShowFragments()
    {
        fragments.ForEach(fragment => { 
        fragment.gameObject.SetActive(true);
        });
    }

    public void PlayHitSound()
    {
        // Play the sound for this object getting hit at its current health level;
        audioSource.pitch = 1; // Might randomize pitch later
        audioSource.PlayOneShot(GetCurrentImpactSound());
    }

    private AudioClip GetCurrentImpactSound()
    {
        return impacts[primaryObject.maxHealth -primaryObject.currentHealth];
    }
}
