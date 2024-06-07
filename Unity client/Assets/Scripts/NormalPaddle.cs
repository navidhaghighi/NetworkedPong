using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPaddle : PaddleController
{
    public override void Move(Direction direction)
    {
        base.Move(direction);
        var vel = rb2d.velocity;
        if (direction == Direction.Up)
        {
            vel.y = speed;
        }
        else if (direction == Direction.Down)
        {
            vel.y = -speed;
        }
        else if (!Input.anyKey)
        {
            vel.y = 0;
        }
        rb2d.velocity = vel;

        var pos = transform.position;
        if (pos.y > boundY)
        {
            pos.y = boundY;
        }
        else if (pos.y < -boundY)
        {
            pos.y = -boundY;
        }
        transform.position = pos;
    }
}
