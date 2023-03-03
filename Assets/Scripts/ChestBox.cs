using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBox : MonoBehaviour
{
  public bool interact = false;
  public Sprite openedChestBox;
  private SpriteRenderer renderer;
  public Rigidbody2D star;
  public Transform point;
  public float verticalForce;
  public float creatStartTime;
  private float timer;

  // Start is called before the first frame update
  void Start()
  {
    renderer = GetComponent<SpriteRenderer>();
    timer = 0f;
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    timer += Time.deltaTime;
    if (interact && timer >= creatStartTime)
    {
      renderer.sprite = openedChestBox;
      Rigidbody2D clone;
      clone = Instantiate(star, point);
      clone.AddForce(Vector2.up * verticalForce, ForceMode2D.Impulse);
      timer = 0;
    }
  }
}
