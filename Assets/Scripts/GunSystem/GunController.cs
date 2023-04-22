using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour
{
    public Gun[] guns;
    public Transform weaponHold;
    public Gun startingGun;
    Gun equippedGun;

    void Start()
    {
        if (startingGun != null)
        {
            EquipGun(startingGun);
        }
    }

    public void EquipGun(Gun gunToEquip)
    {
        if (equippedGun != null)
        {
            Destroy(equippedGun.gameObject);
        }
        equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation) as Gun;
        equippedGun.transform.parent = weaponHold;
    }

    public void SwitchGun(int position)
    {
        switch (position)
        {
            case 0:
                EquipGun(guns[0]);
                break;
            case 1:
                EquipGun(guns[1]);
                break;
            default:
                EquipGun(guns[0]);
                break;
        }
    }

    public void Shoot()
    {
        if (equippedGun != null)
        {
            equippedGun.Shoot("pistol");
        }
    }
}