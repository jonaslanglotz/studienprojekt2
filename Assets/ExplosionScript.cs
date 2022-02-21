using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{

    public Light light;
    
    void Start()
    {
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(3f);
        light.enabled = false;
        yield return new WaitForSeconds(15f);
        Destroy(gameObject);
        yield return null;
    }
}
