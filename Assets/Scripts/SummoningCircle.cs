using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SummoningCircle : MonoBehaviour
{
    public GameObject Familiar;

    public float SummonTime = 2f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SummonTime <= 0) 
        {
            var familiar = Instantiate(Familiar);

            // TODO: Iets van een root class voor familiars
            // if (_familiarContainer != null)
            // {
            //     Familiar.transform.parent = _familiarContainer;
            // }

            familiar.transform.position = transform.position;

            // TODO: Finish animation?
            Destroy(gameObject);
        }
        else 
        {   
            transform.Rotate(new Vector3(0, 0, 90 * Time.deltaTime));
            SummonTime -= Time.deltaTime;
        }                    
    }
}


