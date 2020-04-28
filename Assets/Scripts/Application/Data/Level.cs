using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    /// <summary>
    /// 关卡名称
    /// </summary>
    public string Name;
    /// <summary>
    /// 背景图片
    /// </summary>
    public string Background;
    /// <summary>
    /// 路径图片
    /// </summary>
    public string Road;
    /// <summary>
    /// 初始金币
    /// </summary>
    public int InitScore;
    /// <summary>
    /// 炮塔可放置的点
    /// </summary>
    public List<Point> Holder = new List<Point>();
    /// <summary>
    /// 怪物行走的路径点
    /// </summary>
    public List<Point> Path = new List<Point>();
    /// <summary>
    /// 出怪回合信息
    /// </summary>
    public List<Round> Rounds = new List<Round>();

}
