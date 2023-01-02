using UnityEngine;

namespace Game.Data.TableData
{
  ///
  /// !!! Machine generated code !!!
  /// !!! DO NOT CHANGE Tabs to Spaces !!!
  /// 
  [System.Serializable]
  public class TaskData
  {
    [SerializeField]
    int taskid;
    public int Taskid { get {return taskid; } set { taskid = value;} }
  
    [SerializeField]
    string name;
    public string Name { get {return name; } set { name = value;} }
  
    [SerializeField]
    int previousid;
    public int Previousid { get {return previousid; } set { previousid = value;} }
  
    [SerializeField]
    int tasktype;
    public int Tasktype { get {return tasktype; } set { tasktype = value;} }
  
    [SerializeField]
    int targetid;
    public int Targetid { get {return targetid; } set { targetid = value;} }
  
    [SerializeField]
    int targetnum;
    public int Targetnum { get {return targetnum; } set { targetnum = value;} }
  
    [SerializeField]
    int reward;
    public int Reward { get {return reward; } set { reward = value;} }
  
    [SerializeField]
    string content;
    public string Content { get {return content; } set { content = value;} }
  
  }
}