using UnityEngine;
using System.Collections;

namespace Game.Data.TableData
{
	///
	/// !!! Machine generated code !!!
	/// !!! DO NOT CHANGE Tabs to Spaces !!!
	/// 
	[System.Serializable]
	public class BuildingUpgradeData
	{
    [SerializeField]
    int id;
    public int ID { get {return id; } set { id = value;} }
    
    [SerializeField]
    int level;
    public int Level { get {return level; } set { level = value;} }
    
    [SerializeField]
    int[] itemid = new int[0];
    public int[] Itemid { get {return itemid; } set { itemid = value;} }
    
    [SerializeField]
    int[] itemcount = new int[0];
    public int[] Itemcount { get {return itemcount; } set { itemcount = value;} }
    
    [SerializeField]
    int[] currencyid = new int[0];
    public int[] Currencyid { get {return currencyid; } set { currencyid = value;} }
    
    [SerializeField]
    int[] currencycount = new int[0];
    public int[] Currencycount { get {return currencycount; } set { currencycount = value;} }
    
	}
}