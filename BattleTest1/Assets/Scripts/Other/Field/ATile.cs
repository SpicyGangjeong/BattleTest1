using System;

public class ATile: IComparable<ATile>
{
    public TileController refTile;
    public float fCost { get; set; }
    public float gCost { get; set; }
    public float hCost { get; set; }
    public ATile parentTile { get; set; }

    public ATile nextTile { get; set; }

    public ATile(ref TileController tile, float f = 0, float g = float.MaxValue, float h = 0)
    {
        refTile = tile;
        fCost = f;
        gCost = g;
        hCost = h;
    }
    public override int GetHashCode()
    {
        return (refTile.coordi_X * 100 + refTile.coordi_Y).GetHashCode();
    }
    public override bool Equals(object obj)
    {
        if (obj == this) return true;
        if (obj is null || obj.GetType() != this.GetType()) return false;
        ATile other = (ATile)obj;
        return other.refTile.Equals(this.refTile);
    }
    public override string ToString()
    {
        return "[ " + refTile + " ] ( " + fCost + " " + gCost + " " + hCost + " )";
    }

    // fCost = gCost + hCost
    // gCost = 그 위치까지 가는 실제 비용
    // hCost = 그 타일에서 도착지점까지의 거리
    // 열린목록에 F cost가 가장 작은 노드를 찾는다. 만약에 F cost가 같다면 H cost가 작은 노드를 찾는다.
    public int CompareTo(ATile other)
    {
        if (this.fCost == other.fCost)
        {
            return hCost.CompareTo(other.hCost);
        } else
        {
            return fCost.CompareTo(other.fCost);
        }
    }
}