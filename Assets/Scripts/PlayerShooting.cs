using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using System;

public class PlayerShooting : MonoBehaviour
{
    public Weapon[] weapons;
    public KeyCode[] keysToSwitch;

    public int curWeaponIdex;
    public LayerMask targetLayer;

    public TMP_Text bulletsText;
    public TMP_Text bulletsAllText;

    public Animator HandAnimator;

    private Weapon CurrentWeapon => weapons[curWeaponIdex];
    private bool IsReloading => CurrentWeapon.Reloading < CurrentWeapon.ReloadingMax;

    public void Start()
    {
        CurrentWeapon.Reloading = CurrentWeapon.ReloadingMax;
        curWeaponIdex = Mathf.Clamp(curWeaponIdex, 0, weapons.Length - 1);
        ChangeWeapon();
    }

    public void FixedUpdate()
    {
        if (IsReloading)
        {
            CurrentWeapon.Reloading++;
            if (!IsReloading)
                ReloadingFinish();
        }
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsReloading)
            Shoot();

        SwitchWeapon();
    }

    public void Shoot()
    {
        if (CurrentWeapon.bullets > 0)
        {
            HandAnimator.Play("Shoot");
            Ray shootRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit rayCastHit;

            if (Physics.Raycast(shootRay, out rayCastHit, CurrentWeapon.maxDistance, targetLayer))
            {
                //Destroy(rayCastHit.collider.gameObject);
                GameObject newShoot = Instantiate(CurrentWeapon.shootPrefab, rayCastHit.point, Quaternion.identity, null);
                newShoot.transform.LookAt(Camera.main.transform);
                Destroy(newShoot, 1f);
            }

            CurrentWeapon.bullets--;
            
            if (CurrentWeapon.bullets == 0)
                StartReloading();
        }

        Refresh();
    }

    private void ReloadingFinish()
    {
        CurrentWeapon.bullets = Mathf.Clamp(CurrentWeapon.bulletsAll, 0, CurrentWeapon.bulletsMax);
        CurrentWeapon.bulletsAll -= CurrentWeapon.bullets;
        Refresh();
        HandAnimator.SetBool("IsReloading", false);
    }

    private void SwitchWeapon()
    {
        for (int i = 0; i < keysToSwitch.Length; i++)
        {
            if (Input.GetKeyDown(keysToSwitch[i]))
            {
                curWeaponIdex = i;
                ChangeWeapon();
                return;
            }
        }


        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                curWeaponIdex++;
            }
            else
            {
                curWeaponIdex--;
            }

            curWeaponIdex = Mathf.Clamp(curWeaponIdex, 0, weapons.Length - 1);
            ChangeWeapon();
        }

        Refresh();
    }

    private void ChangeWeapon()
    {
        RestartReloading();
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(false);
        }

        CurrentWeapon.gameObject.SetActive(true);
    }

    private void RestartReloading()
    {
        if (IsReloading)
        {
            StartReloading();
        }
    }

    private void StartReloading()
    {
        if (CurrentWeapon.bulletsAll > 0)
        {
            CurrentWeapon.Reloading = 0;
            HandAnimator.SetBool("IsReloading", true);
        }
    }

    private void Refresh()
    {
        bulletsText.text = string.Format("{0}/{1}", CurrentWeapon.bullets, CurrentWeapon.bulletsMax);
        bulletsAllText.text = CurrentWeapon.bulletsAll.ToString();
    }
}
