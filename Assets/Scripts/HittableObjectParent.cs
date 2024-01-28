using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableObjectParent : MonoBehaviour
{
    [SerializeField] private HittableObject primaryObject; // The gameobject containing the main mesh for the object
    [SerializeField] private List<GameObject> fragments;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
