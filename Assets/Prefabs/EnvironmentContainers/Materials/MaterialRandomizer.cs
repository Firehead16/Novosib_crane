using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialRandomizer : MonoBehaviour
{
    public List<Material> materials = new List<Material>();
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material = materials[Random.Range(0, materials.Count)];
    }
}
