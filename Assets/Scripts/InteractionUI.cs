using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
    public Image image;

    public Sprite scroll;
    public Sprite weapon;
    public Sprite warp;
    public Sprite chest;
    public Sprite reload;

    Camera m_camera;

    LayerMask layerMask;
    RaycastHit raycastHit;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.Player;
        m_camera = Camera.main;
        layerMask = (1 << LayerMask.NameToLayer("WarpPoint")) |
            (1 << LayerMask.NameToLayer("Enemy")) |
            (1 << LayerMask.NameToLayer("Item")) |
            (1 << LayerMask.NameToLayer("Chest"));
    }

    // Update is called once per frame
    void Update()
    {
        Weapon weapon = player.weapon.GetComponent<Weapon>();
        Vector3 rayOrigin = m_camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        Vector3 rayDir = m_camera.transform.forward;
        image.gameObject.SetActive(true);
        if(Physics.Raycast(rayOrigin, rayDir, out raycastHit, 5f, layerMask))
        {
            if (raycastHit.collider.transform.tag.Equals("Scroll")) image.sprite = scroll;
            else if (raycastHit.collider.transform.tag.Equals("Weapon")) image.sprite = this.weapon;
            else if (raycastHit.collider.transform.tag.Equals("Warp")) image.sprite = warp;
            else if (raycastHit.collider.transform.tag.Equals("Chest")) image.sprite = chest;
            else image.gameObject.SetActive(false);
        }
        else if(weapon.ResidualAmmunition / weapon.Magazine <= 0.4f) image.sprite = reload;
        else image.gameObject.SetActive(false);
    }
}
