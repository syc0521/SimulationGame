using UnityEngine;
using System.Collections;

namespace Game.Data.TableData
{
	///
	/// !!! Machine generated code !!!
	/// !!! DO NOT CHANGE Tabs to Spaces !!!
	/// 
	[System.Serializable]
	public class BuildingData
	{
    [SerializeField]
    int buildingid;
    public int Buildingid { get {return buildingid; } set { buildingid = value;} }
    
    [SerializeField]
    string resourcepath;
    public string Resourcepath { get {return resourcepath; } set { resourcepath = value;} }
    
    [SerializeField]
    string name;
    public string Name { get {return name; } set { name = value;} }
    
    [SerializeField]
    int buildingtype;
    public int Buildingtype { get {return buildingtype; } set { buildingtype = value;} }
    
    [SerializeField]
    string description;
    public string Description { get {return description; } set { description = value;} }
    
    [SerializeField]
    int[] currencytype = new int[0];
    public int[] Currencytype { get {return currencytype; } set { currencytype = value;} }
    
    [SerializeField]
    int[] currencycount = new int[0];
    public int[] Currencycount { get {return currencycount; } set { currencycount = value;} }
    
    [SerializeField]
    int rowcount;
    public int Rowcount { get {return rowcount; } set { rowcount = value;} }
    
    [SerializeField]
    int colcount;
    public int Colcount { get {return colcount; } set { colcount = value;} }
    
    [SerializeField]
    int[] cd = new int[0];
    public int[] Cd { get {return cd; } set { cd = value;} }
    
    [SerializeField]
    int producetype;
    public int Producetype { get {return producetype; } set { producetype = value;} }
    
    [SerializeField]
    int produceid;
    public int Produceid { get {return produceid; } set { produceid = value;} }
    
    [SerializeField]
    int[] produceamount = new int[0];
    public int[] Produceamount { get {return produceamount; } set { produceamount = value;} }
    
    [SerializeField]
    bool unlock;
    public bool Unlock { get {return unlock; } set { unlock = value;} }
    
    [SerializeField]
    int people;
    public int People { get {return people; } set { people = value;} }
    
	}
}