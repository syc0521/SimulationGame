using UnityEngine;
using System.Collections;

namespace Game.Data.TableData
{
	///
	/// !!! Machine generated code !!!
	/// !!! DO NOT CHANGE Tabs to Spaces !!!
	/// 
	[System.Serializable]
	public class LoadingStepData
	{
    [SerializeField]
    int stepid;
    public int Stepid { get {return stepid; } set { stepid = value;} }
    
    [SerializeField]
    string name;
    public string Name { get {return name; } set { name = value;} }
    
    [SerializeField]
    string description;
    public string Description { get {return description; } set { description = value;} }
    
    [SerializeField]
    float preparetime;
    public float Preparetime { get {return preparetime; } set { preparetime = value;} }
    
    [SerializeField]
    float runningtime;
    public float Runningtime { get {return runningtime; } set { runningtime = value;} }
    
    [SerializeField]
    float finishtime;
    public float Finishtime { get {return finishtime; } set { finishtime = value;} }
    
	}
}