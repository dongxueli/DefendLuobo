using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于描述一个关卡地图的状态
/// </summary>
public class Map : MonoBehaviour
{
    #region 常量
    public const int RowCount = 8;      //行数
    public const int ColumnCount = 12;  //列数

    #endregion

    #region 事件

    #endregion

    #region 字段
    float MapWidth;     //地图宽
    float MapHeight;    //地图高

    float TileWidth;    //格子宽
    float TileHeight;   //格子高

    List<Tile> m_grid = new List<Tile>();//格子集合
    List<Tile> m_road = new List<Tile>();//路径集合

    Level m_level;
    public bool DrawGizmos = true;//是否绘制网格
    #endregion

    #region 常量

    #endregion

    #region 属性
    public string BackgroundImage
    {
        set
        {
            SpriteRenderer bg_renderer = transform.Find("Background").GetComponent<SpriteRenderer>();
            StartCoroutine(Tools.LoadImage(value, bg_renderer));
        }
    }

    public string RoadImage
    {
        set
        {
            SpriteRenderer road_renderer = transform.Find("Road").GetComponent<SpriteRenderer>();
            StartCoroutine(Tools.LoadImage(value, road_renderer));
        }
    }

    public List<Tile> Grid
    {
        get { return m_grid; }
    }

    public List<Tile> Road
    {
        get { return m_road; }
    }

    public Vector3[] Path
    {
        get
        {
            List<Vector3> m_path = new List<Vector3>();
            for (int i = 0; i < m_road.Count; i++)
            {
                Tile t = m_road[i];
                Vector3 point = GetPosition(t);
                m_path.Add(point);
            }
            return m_path.ToArray();
        }
    }
    #endregion

    #region 方法
    /// <summary>
    /// 加载关卡
    /// </summary>
    /// <param name="level"></param>
    public void LoadLevel(Level level)
    {
        //清楚当前状态
        Clear();
        //保存
        m_level = level;
        //加载图片
        BackgroundImage = "file://" + Consts.MapDir + level.Background;
        RoadImage = "file://" + Consts.MapDir + level.Road;

        //炮塔空地
        for (int i = 0; i < level.Holder.Count; i++)
        {
            Point p = level.Holder[i];
            Tile t = GetTile(p.X, p.Y);
            t.CanHold = true;
        }
        //寻路路径
        for (int i = 0; i < level.Path.Count; i++)
        {
            Point p = level.Path[i];
            Tile t = GetTile(p.X, p.Y);
            m_road.Add(t);
        }
    }

    /// <summary>
    /// 清楚塔位信息
    /// </summary>
    public void ClearHolder()
    {
        foreach (var t in m_grid)
        {
            if (t.CanHold)
                t.CanHold = false;
        }
    }

    /// <summary>
    /// 清楚寻路格子集合
    /// </summary>
    public void ClearRoad()
    {
        m_road.Clear();
    }


    public void Clear()
    {
        m_level = null;
        ClearHolder();
        ClearRoad();
    }
    #endregion

    #region Unity回调
    void Awake()
    {
        CalculateSize();

        for (int i = 0; i < RowCount; i++)
        {
            for (int j = 0; j < ColumnCount; j++)
            {
                m_grid.Add(new Tile(j, i));
            }
        }
    }

    void OnDrawGizmos()
    {
        if (!DrawGizmos) return;

        CalculateSize();
        //绘制格子，线为绿色
        Gizmos.color = Color.green;
        //绘制行
        for (int row = 0; row <= RowCount; row++)
        {
            Vector2 from = new Vector2(-MapWidth / 2, -MapHeight / 2 + row * TileHeight);
            Vector2 to = new Vector2(-MapWidth / 2 + MapWidth, -MapHeight / 2 + row * TileHeight);
            Gizmos.DrawLine(from, to);
        }
        //绘制列
        for (int col = 0; col <= ColumnCount; col++)
        {
            Vector2 from = new Vector2(-MapWidth / 2 + col * TileWidth, MapHeight / 2);
            Vector2 to = new Vector2(-MapWidth / 2 + col * TileWidth, -MapHeight / 2);
            Gizmos.DrawLine(from, to);
        }

        //绘制炮塔可放置的坐标显示
        foreach (Tile t in m_grid)
        {
            if (t.CanHold)
            {
                Vector3 pos = GetPosition(t);
                Gizmos.DrawIcon(pos, "holder.png", true);
            }
        }
        //绘制路径，线为红色
        Gizmos.color = Color.red;
        for (int i = 0; i < m_road.Count; i++)
        {
            if (i == 0)
            {
                Gizmos.DrawIcon(GetPosition(m_road[i]), "start.png", true);
            }
            if (m_road.Count > 1 && i == m_road.Count - 1)
            {
                Gizmos.DrawIcon(GetPosition(m_road[i]), "end.png", true);
            }
            if (m_road.Count > 1 && i != 0)
            {
                Vector2 from = GetPosition(m_road[i - 1]);
                Vector2 to = GetPosition(m_road[i]);
                Gizmos.DrawLine(from, to);
            }
        }

    }
    #endregion

    #region 事件回调

    #endregion

    #region 帮助方法
    /// <summary>
    /// 计算地图、格子的大小
    /// </summary>
    void CalculateSize()
    {
        Vector3 p1 = Camera.main.ViewportToWorldPoint(new Vector3(0, 0));
        Vector3 p2 = Camera.main.ViewportToWorldPoint(new Vector3(1, 1));

        MapWidth = p2.x - p1.x;
        MapHeight = p2.y - p1.y;
        TileWidth = MapWidth / ColumnCount;
        TileHeight = MapHeight / RowCount;

    }

    /// <summary>
    /// 获取格子中心点所在的世界坐标
    /// </summary>
    Vector3 GetPosition(Tile t)
    {
        return new Vector3(
            -MapWidth / 2 + (t.X + 0 / 5f) * TileWidth,
            -MapHeight / 2 + (t.Y + 0 / 5f) * TileHeight,
            0
            );
    }

    /// <summary>
    /// 根据格子索引号获取格子
    /// </summary>
    Tile GetTile(int tileX, int tileY)
    {
        int index = tileX + tileY * ColumnCount;
        if (index < 0 || index >= m_grid.Count)
            return null;
        return m_grid[index];
    }

    /// <summary>
    /// 获取鼠标下面的格子
    /// </summary>
    Tile GetTileUnderMouse()
    {
        Vector2 worldPos = GetWorldPosition();
        int col = (int)((worldPos.x + MapWidth / 2) / TileWidth);
        int row = (int)((worldPos.y + MapHeight / 2) / TileHeight);
        return GetTile(col, row);
    }

    /// <summary>
    /// 获取鼠标所在位置的世界坐标
    /// </summary>
    /// <returns></returns>
    Vector3 GetWorldPosition()
    {
        Vector3 viewPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos);
        return worldPos;
    }
    #endregion
}
