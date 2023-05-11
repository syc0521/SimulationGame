using UnityEngine;
using System.Collections;

namespace Game.Data.TableData
{
	///
	/// !!! Machine generated code !!!
	/// !!! DO NOT CHANGE Tabs to Spaces !!!
	/// 
	[System.Serializable]
	public class BagItemData
	{
    [SerializeField]
    int id;
    public int ID { get {return id; } set { id = value;} }
    
    [SerializeField]
    string name;
    public string Name { get {return name; } set { name = value;} }
    
    [SerializeField]
    int type;
    public int Type { get {return type; } set { type = value;} }
    
    [SerializeField]
    string content;
    public string Content { get {return content; } set { content = value;} }
    
    [SerializeField]
    int price;
    public int Price { get {return price; } set { price = value;} }
    
    [SerializeField]
    string iconname;
    public string Iconname { get {return iconname; } set { iconname = value;} }
    
    [SerializeField]
    bool cansell;
    public bool Cansell { get {return cansell; } set { cansell = value;} }
    
	}
}