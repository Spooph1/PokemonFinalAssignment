using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Direction characterDir;
    public float movementSpeed = 2f;
    public Transform endPos;
    public Vector2 playerInput;

    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;
    void Start()
    {
        endPos.parent = null;
    }
    void Update()
    {
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        transform.position = Vector3.MoveTowards(transform.position, endPos.position, movementSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, endPos.position) <= 0.05f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                endPos.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                endPos.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
            }
        }
        if (playerInput != Vector2.zero)
        {
            if (playerInput.x < 0)
            {
                characterDir = Direction.Left;
            }
            if (playerInput.x > 0)
            {
                characterDir = Direction.Right;
            }
            if (playerInput.y < 0)
            {
                characterDir = Direction.Down;
            }
            if (playerInput.y > 0)
            {
                characterDir = Direction.Up;
            }
            switch (characterDir)
            {
                case Direction.Up:
                    gameObject.GetComponent<SpriteRenderer>().sprite = upSprite;
                    break;
                case Direction.Down:
                    gameObject.GetComponent<SpriteRenderer>().sprite = downSprite;
                    break;
                case Direction.Left:
                    gameObject.GetComponent<SpriteRenderer>().sprite = leftSprite;
                    break;
                case Direction.Right:
                    gameObject.GetComponent<SpriteRenderer>().sprite = rightSprite;
                    break;
            }
        }
    }
    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

}
