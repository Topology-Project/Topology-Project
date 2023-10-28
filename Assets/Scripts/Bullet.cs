using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject parent { get; private set; }
    private float speed; // 투사체 속도
    private float range; // 최대 사거리
    private Vector3 startPos; // 발사 위치

    public void Set(GameObject parent, float speed, float range)
    {
        this.parent = parent;
        this.speed = speed;
        startPos = parent.transform.position;
        this.range = range;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // 직진~
        if(Vector3.Distance(startPos, transform.position) >= range) Destroy(gameObject); // 최대 사거리 도달 시 객체 삭제
    }

    void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.tag.Equals(parent.tag))
        {
            // 상대 객체 트리거 시 자신 객체 삭제
            Destroy(gameObject);
        }

    }
}
