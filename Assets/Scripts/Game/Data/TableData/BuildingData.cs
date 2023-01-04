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
    int currencytype;
    public int Currencytype { get {return currencytype; } set { currencytype = value;} }
    
    [SerializeField]
    int currencycount;
    public int Currencycount { get {return currencycount; } set { currencycount = value;} }
    
	}
}