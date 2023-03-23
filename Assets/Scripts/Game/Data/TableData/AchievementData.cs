using UnityEngine;
using System.Collections;

namespace Game.Data.TableData
{
	///
	/// !!! Machine generated code !!!
	/// !!! DO NOT CHANGE Tabs to Spaces !!!
	/// 
	[System.Serializable]
	public class AchievementData
	{
    [SerializeField]
    int id;
    public int ID { get {return id; } set { id = value;} }
    
    [SerializeField]
    string name;
    public string Name { get {return name; } set { name = value;} }
    
    [SerializeField]
    string content;
    public string Content { get {return content; } set { content = value;} }
    
    [SerializeField]
    int unlockcondition;
    public int Unlockcondition { get {return unlockcondition; } set { unlockcondition = value;} }
    
    [SerializeField]
    int type;
    public int Type { get {return type; } set { type = value;} }
    
    [SerializeField]
    int targetid;
    public int Targetid { get {return targetid; } set { targetid = value;} }
    
    [SerializeField]
    int targetnum;
    public int Targetnum { get {return targetnum; } set { targetnum = value;} }
    
    [SerializeField]
    int[] requiretask = new int[0];
    public int[] Requiretask { get {return requiretask; } set { requiretask = value;} }
    
    [SerializeField]
    int rewardgroup;
    public int Rewardgroup { get {return rewardgroup; } set { rewardgroup = value;} }
    
	}
}