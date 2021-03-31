using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuCopterExplosion : MonoBehaviour
{
    public ParticleSystem boomTown;
    public BoxCollider boxCollider;

    bool startMoving;
    private void Update()
    {
        if (startMoving == true)
        {
            gameObject.transform.Translate(0, -4 * Time.deltaTime, 0);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        boomTown.Play();
        Destroy(boxCollider);
        startMoving = true;
    }
}
