using UnityEngine;
using System.Collections;

namespace Game.Data.TableData
{
	///
	/// !!! Machine generated code !!!
	/// !!! DO NOT CHANGE Tabs to Spaces !!!
	/// 
	[System.Serializable]
	public class RewardGroupData
	{
    [SerializeField]
    int rewardid;
    public int Rewardid { get {return rewardid; } set { rewardid = value;} }
    
    [SerializeField]
    int[] rewardtype = new int[0];
    public int[] Rewardtype { get {return rewardtype; } set { rewardtype = value;} }
    
    [SerializeField]
    int[] itemid = new int[0];
    public int[] Itemid { get {return itemid; } set { itemid = value;} }
    
    [SerializeField]
    int[] count = new int[0];
    public int[] Count { get {return count; } set { count = value;} }
    
	}
}