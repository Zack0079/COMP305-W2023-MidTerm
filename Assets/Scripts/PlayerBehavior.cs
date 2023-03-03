using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerBehavior : MonoBehaviour
{
  [Header("Player Movement Properties")]
  public float horizontalForce;
  public float maxSpeed;
  public float verticalForce;
  public float airFactor;
  public float runDeccelAmount;
  public LayerMask groundLayerMask;
  public Transform groundPoint;
  public float groundRadius;
  public float xSpeed;
  public float ySpeed;
  public bool isGrounded;
  public bool isDucking;


  private Rigidbody2D playerRigidbody2D;
  private Animator animator;

  // Start is called before the first frame update
  void Start()
  {
    playerRigidbody2D = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();

  }

  // Update is called once per frame
  void Update()
  {
    var y = Convert.ToInt32(Input.GetKeyDown(KeyCode.Space));
    // float y = Input.GetAxis("Jump");
    var x = Input.GetAxis("Horizontal");
    bool c = Input.GetKey(KeyCode.S);

    Jump(y);
    Crouch(x, c);
  }

  private void FixedUpdate()
  {
    isGrounded = Physics2D.OverlapCircle(groundPoint.position, groundRadius, groundLayerMask);
    animator.SetBool("isGrounded", isGrounded);
    var x = Input.GetAxis("Horizontal");

    Move(x);
    Flip(x);
  }

  private void Move(float x)
  {
    float targetSpeed = x * maxSpeed;
    targetSpeed = Mathf.Lerp(playerRigidbody2D.velocity.x, targetSpeed, 1f);
    float accelRate;


    accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? horizontalForce : runDeccelAmount;

    float speedDif = targetSpeed - playerRigidbody2D.velocity.x;

    float movement = speedDif * accelRate;
    //show velocity;

    playerRigidbody2D.AddForce(Vector2.right * movement, ForceMode2D.Force);

    playerRigidbody2D.velocity = new Vector2(Mathf.Clamp(playerRigidbody2D.velocity.x, -maxSpeed, maxSpeed), playerRigidbody2D.velocity.y);


    animator.SetFloat("xSpeed", Mathf.Abs(x));
    // if (x < 0.01 && x > -0.01)
    // {
    //   animator.SetFloat("xSpeed", 0);

    // }
  }

  private void Jump(int y)
  {
    if (isGrounded && y > 0.0f)
    {

      playerRigidbody2D.AddForce(Vector2.up * verticalForce, ForceMode2D.Impulse);
      // playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x, Mathf.Clamp(playerRigidbody2D.velocity.y, 0, maxHight));
    }
  }

  private void Crouch(float x, bool c)
  {
    if (isGrounded && c && x == 0)
    {
      animator.SetBool("isDucking", true);
    }
    else
    {
      animator.SetBool("isDucking", false);
    }
  }


  private void Flip(float x)
  {
    if (x != 0)
    {
      transform.localScale = new Vector3((x > 0) ? 1 : -1, 1, 1);

    }
  }

  private void interact(ref bool value)
  {
    if (Input.GetKey(KeyCode.E))
    {
      value = true;
    }
  }


  private void OnDrawGizmos()
  {
    Gizmos.color = Color.white;
    Gizmos.DrawWireSphere(groundPoint.position, groundRadius);
  }

  private void OnTriggerStay2D(Collider2D other)
  {
    if (other.gameObject.CompareTag("Chest"))
    {
      interact(ref other.gameObject.GetComponent<ChestBox>().interact);
    }
  }
}
