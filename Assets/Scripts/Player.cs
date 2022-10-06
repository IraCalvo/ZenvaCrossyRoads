using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public delegate void PlayerHandler();
    public event PlayerHandler OnPlayerMoved;
    public event PlayerHandler OnPlayerEscaped;

    public float moveDistance = 0.32f;
    private bool moved;
    private Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {


        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        if(!moved)
        {
            //fish didnt move
            Vector2 targetPosition = Vector2.zero;
            bool tryingToMove = false;
            if(horizontalMovement != 0)
            {
                tryingToMove = true;
                targetPosition = new Vector2(transform.position.x + (horizontalMovement > 0 ? moveDistance : -moveDistance), transform.position.y);
            }
            else if(verticalMovement != 0)
            {
                tryingToMove = true;
                targetPosition = new Vector2(transform.position.x, transform.position.y + (verticalMovement > 0 ? moveDistance : -moveDistance));
            }

            Collider2D hitCollider = Physics2D.OverlapCircle(targetPosition, 0.1f);
            if(tryingToMove == true && (hitCollider == null || hitCollider.GetComponent<Enemy>() != null))
            {
                GetComponent<AudioSource>().Play();
                transform.position = targetPosition;
                moved = true;
                if(OnPlayerMoved != null)
                {
                    OnPlayerMoved();
                }
            }
        }
        else
        {
            //fish move
            if(horizontalMovement == 0 && verticalMovement == 0)
            {
                moved = false;
            }
        }

        if(transform.position.y > (Screen.height / 100f) /2f)
        {
            transform.position = startingPosition;
            if(OnPlayerEscaped != null)
            {
                OnPlayerEscaped();
            }
        }


        if(transform.position.y < -(Screen.height / 100f) /2f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + moveDistance);
        }

        if(transform.position.x < -(Screen.width / 100f) /2f)
        {
            transform.position = new Vector3(transform.position.x + moveDistance, transform.position.y);
        }

        if(transform.position.x > (Screen.width / 100f) /2f)
        {
            transform.position = new Vector3(transform.position.x - moveDistance, transform.position.y);
        }

    }

    void OnTriggerEnter2D (Collider2D otherCollider)
    {
        if(otherCollider.GetComponent<Enemy>() != null)
        {
            Destroy(gameObject);
        }
    }
}
