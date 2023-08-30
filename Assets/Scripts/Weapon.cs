using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int bullets;
    public int bulletsMax;
    public int bulletsAll;

    public float damage;
    

    [SerializeField]
    private float reloadingMax;

    public float ReloadingMax
    {
        get 
        {
            return reloadingMax*50;
        }
        set 
        {
            if (value >= 0)
            {
                reloadingMax = value;
            }
        }
    }

    public float Reloading {get; set;}

    public float maxDistance;

    public GameObject shootPrefab;
}
