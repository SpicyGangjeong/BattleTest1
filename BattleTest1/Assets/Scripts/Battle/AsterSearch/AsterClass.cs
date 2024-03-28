using System;
using Vec2Map = System.Collections.Generic.List<System.Collections.Generic.List<string>>;
const float INF = (98654321.0000f);

// Vec2MapInfo: 맵정보, 맵의 최대크기, 맵의 출발지점, 맵의 도착지점
class Vec2MapInfo
{
    Vec2Map mapInfo;

    int map_row;
    int map_col;

    int beg_x;
    int beg_y;

    int dest_x;
    int dest_y;
}

// 한 정점의 좌표와 비용
class CostCoord
{
    float cost;
    int x, y;
}

// 한 정점의 좌표
class Coord
{
    int x, y;
}

// 비용과 부모노드
class CellDetails
{
    float f, g, h;
    int parent_x, parent_y;
}

// 맵에 표기된 문자의 역할
class PointDecl
{
    string BeginPoint;
    string DestinationPoint;
    string Void;
    string Wall;
    string TracedPath;
}
