using UnityEngine;
using System.Collections;

namespace Game.Data.TableData
{
	///
	/// !!! Machine generated code !!!
	/// !!! DO NOT CHANGE Tabs to Spaces !!!
	/// 
	[System.Serializable]
	public class HouseConsumeData
	{
    [SerializeField]
    int buildingid;
    public int Buildingid { get {return buildingid; } set { buildingid = value;} }
    
    [SerializeField]
    string name;
    public string Name { get {return name; } set { name = value;} }
    
    [SerializeField]
    int level;
    public int Level { get {return level; } set { level = value;} }
    
    [SerializeField]
    int[] consumeid = new int[0];
    public int[] Consumeid { get {return consumeid; } set { consumeid = value;} }
    
    [SerializeField]
    int[] produceamount = new int[0];
    public int[] Produceamount { get {return produceamount; } set { produceamount = value;} }
    
	}
}