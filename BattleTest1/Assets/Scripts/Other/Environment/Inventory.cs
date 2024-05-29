using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Inventory
{
    public event EventHandler IndicateGold;
    private static Inventory __Inventory;
    private int gold = 46;
    public static Inventory getInstance()
    {
        if (__Inventory == null)
        {
            __Inventory = new Inventory();
        }
        return __Inventory;
    }
    public int Gold
    { 
        get { return gold; }
        set
        {
            if (gold != value)
            {
                gold = value;
                OnGoldChanged();
            }
        }
    }
    protected virtual void OnGoldChanged()
    {
        IndicateGold?.Invoke(this, EventArgs.Empty);
    }
}
