using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableObject : MonoBehaviour
{
    public int currentHealth = 0;
    public int maxHealth = 3;
    [SerializeField] private Material transparentMaterial;

    [SerializeField] private GameObject hitDecal; // This should be a prefab made up of two quads facing opposite directions with a transparent texture on each
    public HittableObjectParent parent;

    // Start is called before the first frame update
    void Awake()
    {
        parent = transform.parent.GetComponent<HittableObjectParent>();
        currentHealth = maxHealth;
    }

    public void ShowDamage(Vector3 pos) {
        GameObject GO = Instantiate(hitDecal, pos, Quaternion.identity);
        GO.transform.SetParent(transform);
        GO.transform.localRotation = Quaternion.identity; // Resetting the local rotation of the decal object so it appears on the surface of the glass regardless of angle. 
        //Destroy(GO, 5);
    }

    public void TakeDamage(int damage, Vector3 hitPos, Vector3 hitOrigin)
    {
        parent.PlayHitSound();
        currentHealth -= damage;
        ShowDamage(hitPos);
        Debug.Log($"{this.name} has taken damage!");
        if (currentHealth <= 0)
        {
            parent.Shatter(hitPos, hitOrigin);
        } else if (currentHealth != maxHealth) {
            parent.ShowFragments();
            Color newColor = GetComponent<MeshRenderer>().material.color;
            newColor.a = 0;
            GetComponent<MeshRenderer>().material = transparentMaterial;
        }
    }

}
