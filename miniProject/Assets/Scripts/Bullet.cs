using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject parent { get; private set; }
    private float speed;

    public void Set(GameObject parent, float speed)
    {
        this.parent = parent;
        this.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.tag.Equals(parent.tag))
        {
            Destroy(gameObject);
        }

    }
}
