using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 格子信息
/// </summary>
public class Tile
{
    public int X;
    public int Y;
    public bool CanHold;//是否可以放置塔
    public object Data;//格子保存的数据

    public Tile(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return string.Format("[X:{0}, Y:{1}, CanHold:{2}]", X, Y, CanHold);
    }
}
