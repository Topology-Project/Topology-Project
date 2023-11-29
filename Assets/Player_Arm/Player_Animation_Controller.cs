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
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if(moveDir != Vector3.zero) Walk();
        else Idle();
    }

    public void Fire1()
    {
        Anim_Arm.SetTrigger("Fire");
        Anim_Gun.SetTrigger("Fire");
    }

    public void Reload()
    {
        Anim_Arm.SetTrigger("Reload");
        Anim_Gun.SetTrigger("Reload");
    }

    public void Walk()
    {
        Anim_Arm.SetBool("Walk", true);
        Anim_Gun.SetBool("Walk", true);
    }

    public void Idle()
    {
        Anim_Arm.SetBool("Walk", false);
        Anim_Gun.SetBool("Walk", false);
    }
}
