using UnityEngine;
using System.Collections;

namespace Game.Data.TableData
{
	///
	/// !!! Machine generated code !!!
	/// !!! DO NOT CHANGE Tabs to Spaces !!!
	/// 
	[System.Serializable]
	public class BuildingProduceData
	{
    [SerializeField]
    int id;
    public int ID { get {return id; } set { id = value;} }
    
    [SerializeField]
    string name;
    public string Name { get {return name; } set { name = value;} }
    
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
    int consumeid;
    public int Consumeid { get {return consumeid; } set { consumeid = value;} }
    
    [SerializeField]
    int[] consumeamount = new int[0];
    public int[] Consumeamount { get {return consumeamount; } set { consumeamount = value;} }
    
	}
}