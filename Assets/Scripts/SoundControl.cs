using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControl : MonoBehaviour
{
    public static SoundControl Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    [SerializeField] private AudioSource idleCrane, workCrane;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CraneControl.craneState.CraneControlEnabled && !idleCrane.isPlaying) idleCrane.Play();
        if(!CraneControl.craneState.CraneControlEnabled && idleCrane.isPlaying) idleCrane.Stop();
        
        if(CraneControl.craneState.CraneMoveEnabled && !workCrane.isPlaying) workCrane.Play();
        if(!CraneControl.craneState.CraneMoveEnabled && workCrane.isPlaying) workCrane.Stop();
    }
}
