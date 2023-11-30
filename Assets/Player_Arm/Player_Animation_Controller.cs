using UnityEngine;

public class Player_Animation_Controller : MonoBehaviour
{
    public GameObject Arm;
    public GameObject Gun;
    Animator Anim_Arm;
    Animator Anim_Gun;
    // Start is called before the first frame update
    void Start()
    {
        Anim_Arm = Arm.GetComponent<Animator>();
        Anim_Gun = Gun.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool fireDown = Input.GetKeyDown(KeyCode.Alpha1);
        bool reloadDown = Input.GetKeyDown(KeyCode.Alpha2);
        bool walkDown = Input.GetKey(KeyCode.Alpha3);

        if (fireDown)
        {
            Anim_Arm.SetTrigger("Fire");
            Anim_Gun.SetTrigger("Fire");
        }
        if (reloadDown)
        {
            Anim_Arm.SetTrigger("Reload");
            Anim_Gun.SetTrigger("Reload");
        }
        if (walkDown)
        {
            Debug.Log("이동중");
            Anim_Arm.SetBool("Walk", true);
            Anim_Gun.SetBool("Walk", true);
        }
        else
        {
            Anim_Arm.SetBool("Walk", false);
            Anim_Gun.SetBool("Walk", false);
        }
    }
}
