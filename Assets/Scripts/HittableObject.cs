using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableObject : MonoBehaviour
{
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private GameObject hitDecal; // This should be a prefab made up of two quads facing opposite directions with a transparent texture on each

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] impacts;
    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    public void TakeDamage(int damage, Vector3 hitPos)
    {
        PlayHitSound(); 
        currentHealth -= damage;
        ShowDamage(hitPos);
        Debug.Log($"{this.name} has taken damage!");
        if (currentHealth <= 0 ) {
            Invoke(nameof(Death), 2);
                }
    }

    public void PlayHitSound()
    {
        // Play the sound for this object getting hit at its current health level;
        audioSource.pitch = 1; // Might randomize pitch later
        audioSource.PlayOneShot(GetCurrentImpactSound());
    }

    public void ShowDamage(Vector3 pos) {
        GameObject GO = Instantiate(hitDecal, pos, Quaternion.identity);
        GO.transform.SetParent(transform);
        GO.transform.localRotation = Quaternion.identity; // Resetting the local rotation of the decal object so it appears on the surface of the glass regardless of angle. 
        //Destroy(GO, 5);
    }
    void Death()
    {
        // Replace this with whatever we make the breaking behavior
        Destroy(gameObject);
    }

    private AudioClip GetCurrentImpactSound()
    {
        return impacts[maxHealth - currentHealth];
    }
}
