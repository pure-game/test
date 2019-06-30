using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmGun : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject bullet_prefab;
    [SerializeField] GameObject barrel;
    [SerializeField] float plasmSpeed = 10f;

    void Start()
    {
        Player parent = gameObject.GetComponentInParent<Player>();
    }

    public void Shoot()
    {
        GameObject bullet = Instantiate(bullet_prefab, transform.position, transform.rotation) as GameObject;
        bullet.GetComponent<BulletScript>().Parent = transform.parent.gameObject; 
        bullet.transform.position = barrel.transform.position;
        bullet.GetComponent<Rigidbody2D>().velocity = transform.right * Mathf.Sign(transform.lossyScale.x) * plasmSpeed;
    }
}
