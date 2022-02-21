using System.Collections;
using UnityEngine;

public class IronDomeBattery : MonoBehaviour
{
    
    public GameObject missilePrefab;
    
    public IEnumerator Fire(int rocketCount)
    {
        var targets = GameObject.FindGameObjectsWithTag("Rocket");

        foreach (var target in targets)
        {
            var unguidedComponent = target.GetComponent<UnguidedMissile>();

            if (unguidedComponent == null)
            {
                continue;
            }

            for (int i = 0; i < rocketCount; i++)
            {
                var missile = Instantiate(missilePrefab);

                missile.transform.position = transform.position;
                missile.transform.rotation = transform.rotation;
            
                var missileComponent = missile.GetComponent<MissileScript>();

                missileComponent.launcher = gameObject;
                missileComponent.target = target;

                yield return new WaitForSeconds(5f);
            }
        }
    }
}
