using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Weapon[] weapons;
    public KeyCode[] keysToSwitch;

    public int currentWeapon;
    public GameObject sphere;
    public LayerMask targetLayer;

    private Weapon CurrentWeapon => weapons[currentWeapon];

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Shoot();
        
        SwitchWeapon();
    }

    public void Shoot()
    {
        Ray shootRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit rayCastHit;

        if (Physics.Raycast(shootRay, out rayCastHit, CurrentWeapon.maxDistance, targetLayer))
        {
            //Destroy(rayCastHit.collider.gameObject);
            GameObject newShoot = Instantiate(CurrentWeapon.shootPrefab, rayCastHit.point, Quaternion.identity, null);
            newShoot.transform.LookAt(Camera.main.transform);
            Destroy(newShoot, 1f);
        }
    }

    private void SwitchWeapon()
    {
        for (int i =0; i < keysToSwitch.Length; i++)
        {
            if (Input.GetKeyDown(keysToSwitch[i]))
            {
                currentWeapon = i;
                Refresh();
                return;
            }
        }
        

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                currentWeapon++;
            }
            else
            {
                currentWeapon--;
            }

            currentWeapon = Mathf.Clamp(currentWeapon, 0, weapons.Length - 1);
            Refresh();
        }
    }

    public void Refresh()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(false);
        }

        CurrentWeapon.gameObject.SetActive(true);
    }
}
