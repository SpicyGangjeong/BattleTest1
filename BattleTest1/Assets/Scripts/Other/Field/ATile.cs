public class ATile
{
    public TileController refTile;
    public float fCost { get; set; }
    public float gCost { get; set; }
    public float hCost { get; set; }
    public ATile parentTile { get; set; }

    public ATile nextTile { get; set; }

    public ATile(ref TileController tile, float f = 0, float g = 0, float h = 0)
    {
        refTile = tile;
        fCost = f;
        gCost = g;
        hCost = h;
    }
}