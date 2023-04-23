using System.Collections.Generic;

public class Inventory
{
    public static List<string> foods = new List<string>();
    public static List<string> tools = new List<string>();
    public static void InitializeToolsInventory()
    {
        if (tools.Count == 0)
        {
            tools.Add("Stegepande");
        }
    }
}
