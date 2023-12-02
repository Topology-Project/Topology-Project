using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public Text text;
    Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameManager.Instance.MainCamera.transform;
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up*Time.deltaTime);
        transform.LookAt(transform.position + cam.rotation * (Vector3.forward * 2), cam.rotation * Vector3.up);
        text.color = new Color(255,255,255,text.color.a-Time.deltaTime); 
    }

    public void SetDamageText(int damage=0) => text.text = damage.ToString();
}
