using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronDomeBattery : MonoBehaviour
{
    
    public GameObject missilePrefab;
    public float lastFire = 0;
    
    // Update is called once per frame
    void Update()
    {
        if (lastFire + 3 > Time.time)
        {
            return;
        }
        
        var targets = GameObject.FindGameObjectsWithTag("Rocket");

        foreach (var target in targets)
        {
            var unguidedComponent = target.GetComponent<UnguidedMissile>();

            if (unguidedComponent == null)
            {
                continue;
            }

            var missile = Instantiate(missilePrefab);

            missile.transform.position = transform.position;
            missile.transform.rotation = transform.rotation;
            
            var missileComponent = missile.GetComponent<MissileScript>();

            missileComponent.launcher = gameObject;
            missileComponent.target = target;
        }

        lastFire = Time.time;
    }
}
