using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpScript : MonoBehaviour
{
    [SerializeField] float heathPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.GetComponent<Entity>().Bullet)
        {
            heathPoint -= collision2D.gameObject.GetComponent<BulletScript>().Damage;
        }
        if (heathPoint <= 0)
        {
            Destroy(gameObject);
        }
    }

}
