using System.Collections;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public float lifeTime;
    void Start()
    {
        StartCoroutine(DestroySelf(lifeTime*2));
    }

    private IEnumerator DestroySelf(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
