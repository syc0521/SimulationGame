using UnityEngine;
using System.Collections;

namespace Game.Data.TableData
{
	///
	/// !!! Machine generated code !!!
	/// !!! DO NOT CHANGE Tabs to Spaces !!!
	/// 
	[System.Serializable]
	public class StoreItemData
	{
    [SerializeField]
    int id;
    public int ID { get {return id; } set { id = value;} }
    
    [SerializeField]
    int storeid;
    public int Storeid { get {return storeid; } set { storeid = value;} }
    
    [SerializeField]
    int itemtype;
    public int Itemtype { get {return itemtype; } set { itemtype = value;} }
    
    [SerializeField]
    int itemid;
    public int Itemid { get {return itemid; } set { itemid = value;} }
    
    [SerializeField]
    int count;
    public int Count { get {return count; } set { count = value;} }
    
    [SerializeField]
    int[] consumeid = new int[0];
    public int[] Consumeid { get {return consumeid; } set { consumeid = value;} }
    
    [SerializeField]
    int[] consumecount = new int[0];
    public int[] Consumecount { get {return consumecount; } set { consumecount = value;} }
    
    [SerializeField]
    int[] currencyid = new int[0];
    public int[] Currencyid { get {return currencyid; } set { currencyid = value;} }
    
    [SerializeField]
    int[] currencycount = new int[0];
    public int[] Currencycount { get {return currencycount; } set { currencycount = value;} }
    
    [SerializeField]
    string name;
    public string Name { get {return name; } set { name = value;} }
    
    [SerializeField]
    int requirelevel;
    public int Requirelevel { get {return requirelevel; } set { requirelevel = value;} }
    
	}
}