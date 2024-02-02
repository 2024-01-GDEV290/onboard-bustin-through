using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// This class manages the showing of the glass pane and fragments. 
public class HittableObjectParent : MonoBehaviour
{

    [SerializeField] private HittableObject primaryObject; // The gameobject containing the main mesh for the object
    [SerializeField] private List<GameObject> fragments;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] impacts;

    public GameObject shatteredGlass;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
      
    }

    public void Shatter(Vector3 hitLocation, Vector3 hitOrigin)
    {
        Destroy(primaryObject.gameObject);

        GameObject shattered = Instantiate(shatteredGlass, transform.position, Quaternion.identity);
        shattered.transform.localEulerAngles = new Vector3(0, -90, 0);

        Debug.Log($"{this.name} has shattered!");
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
