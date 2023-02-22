using System.Collections;
using Game.Core;
using Game.Data;
using Game.Data.Event.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.GamePlaySystem.Loading
{
    public class LoadingManager : GamePlaySystemBase<LoadingManager>
    {
        public void StartLoadingGame()
        {
            MonoApp.Instance.StartCoroutine(Load());
        }

        private IEnumerator Load()
        {
            //1.加载玩家存档
            Managers.Get<ISaveDataManager>().LoadData();
            yield return new WaitForSeconds(0.4f);

            //2.加载建筑信息
            BuildingManager.Instance.LoadBuildings();
            yield return new WaitForSeconds(0.4f);
            
            //3.加载Scene
            var operation = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
            while (!operation.isDone)
            {
                Debug.Log(operation.progress);
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);

            EventCenter.DispatchEvent(new LoadSceneFinishedEvent());
        }
    }
}