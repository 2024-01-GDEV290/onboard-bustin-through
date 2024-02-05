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

    public void ShowDamage(Vector3 pos, Vector3 hitOrigin) 
    {
        GameObject decal = Instantiate(hitDecal, pos, Quaternion.identity);
        decal.transform.SetParent(transform);
        Vector3 diff = hitOrigin - pos;
        if (Vector3.Angle(diff, pos) > 90){
            // Orienting the decal to the correct direction so the particle system is facing towards the side the player is on. 
            decal.transform.localEulerAngles = new Vector3(-90, 0, 0);
        } else
        {
            decal.transform.localEulerAngles = new Vector3(90, 0, 0);
        }
    }

    public void TakeDamage(int damage, Vector3 hitPos, Vector3 hitOrigin)
    {
        parent.PlayHitSound();
        currentHealth -= damage;
        ShowDamage(hitPos,hitOrigin);
        if (currentHealth <= 0)
        {
            parent.Shatter(hitPos, hitOrigin);
        }
    }

}
