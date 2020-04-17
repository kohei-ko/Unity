using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void Start()
    {
        var particleSystem = GetComponent<ParticleSystem>();
        Destroy( gameObject, particleSystem.main.duration );
    }
}