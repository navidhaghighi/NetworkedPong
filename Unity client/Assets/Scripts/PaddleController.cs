using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    public float speed = 10.0f;
    public float boundY = 2.25f;
    protected Rigidbody2D rb2d;
    public enum Direction
    {
        Up,
        Down
    }
    public virtual void Move(Direction direction)
    {

    }
}
