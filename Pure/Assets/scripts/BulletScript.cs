using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] float damage = 10;

    GameObject parent;

    public float Damage { get => damage; set => damage = value; }
    public GameObject Parent { get => parent; set => parent = value; }

    public void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.GetComponent<Entity>() == null || collision2D.gameObject.GetComponent<Entity>().fraction != parent.GetComponent<Entity>().fraction)
        {
            Destroy(gameObject);//уничтожаем объект со скриптом
        }
        else
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision2D.collider, true);
        }
    }
}
