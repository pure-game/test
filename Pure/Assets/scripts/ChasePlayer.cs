using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float chaseSpeed;
    [SerializeField] float botMemory = 5f;
    [SerializeField] float idlePerimeter = 5f;

    Rigidbody2D rigidbody2D;
    GameObject gun;
    GameObject player = null;
    Vector3 lastPlayerPosition;
    bool playerVisibility = false;
    bool patrol = false;
    float lastSeen = 0;
    float curPerimeter = 0;
    int playerInCollidersCount = 0;
    int curDirection = 0;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        gun = transform.Find("plasmgun").gameObject;
    }

    private void Update()
    {
        if (lastSeen > 0)
            lastSeen -= Time.deltaTime;
        else
            lastSeen = 0;
        CheckPlayerVisibility();
        FollowPlayer();
        FlipSprite();
        RotateGun();
        Shooting();
    }

    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.GetComponent<Entity>().Player)
        {
            playerInCollidersCount++;
            player = collider2D.gameObject;
        }
    }

    public void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.GetComponent<Entity>().Player)
        {
            playerInCollidersCount--;
            if (playerInCollidersCount == 0)
            {
                playerVisibility = false;
                player = null;
            }
        }
    }

    void CheckPlayerVisibility()
    {
        if (player == null)
            return;

        Vector2 direction = player.transform.position - transform.position;
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, layerMask);
        Debug.DrawRay(transform.position, direction * 10, Color.yellow, 1f, false);

        if (hit.collider.gameObject.tag == "Player")
        {
            lastPlayerPosition = hit.collider.transform.position;
            playerVisibility = true;
        }
        else
        {
            if (playerVisibility)
                lastSeen = botMemory;
            playerVisibility = false;
        }
    }

    public void Shooting()
    {
        Animator animation = gun.GetComponent<Animator>();
        animation.speed = 1.5f;
        if (playerVisibility)
        {
            animation.Play("shoot");
        }
        else
        {
            animation.Play("PlasmGunIdle");
        }
    }

    public void RotateGun()
    {
        if (playerVisibility)
        {
            float swap = Mathf.Sign(transform.lossyScale.x);
            gun.transform.rotation = Quaternion.FromToRotation(Vector3.right * swap, player.transform.position - transform.position);
        }
        else
        {
            gun.transform.rotation = transform.rotation;
        }
    }

    void FollowPlayer()
    { 
        if (playerVisibility)
        {
            patrol = false;
            Vector2 dir = player.transform.position - transform.position;
            rigidbody2D.velocity = dir.normalized * chaseSpeed;
            return;
        }
        if (!playerVisibility && player != null && lastSeen > 0)
        {
            patrol = false;
            Vector2 dir = lastPlayerPosition - transform.position;
            rigidbody2D.velocity = dir.normalized * chaseSpeed;
            return;
        }
        if (!patrol)
        {
            curDirection = 0;
            rigidbody2D.velocity = getRotatedVelocity(curDirection);
        }
        patrol = true;
        if (curPerimeter > idlePerimeter || rigidbody2D.velocity.magnitude < Mathf.Epsilon)
        {
            curPerimeter = 0;
            curDirection++;
            curDirection %= 4;
            rigidbody2D.velocity = getRotatedVelocity(curDirection);
            
        }
        else
        {
            curPerimeter += rigidbody2D.velocity.magnitude * Time.deltaTime;
        }
    }

    public void FlipSprite()
    {
        bool horisontalSpeed = Mathf.Abs(rigidbody2D.velocity.x) > Mathf.Epsilon;
        if (horisontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rigidbody2D.velocity.x), 1f);
        }
    }

    Vector2 getRotatedVelocity(int rot)
    {
        if (rot == 0)
            return new Vector2(0, chaseSpeed);
        if (rot == 1)
            return new Vector2(chaseSpeed, 0);
        if (rot == 2)
            return new Vector2(0, -chaseSpeed);
        return new Vector2(-chaseSpeed, 0);
    }

}
