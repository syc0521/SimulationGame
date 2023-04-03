using System.Collections;
using Game.Core;
using Game.Data;
using Game.Data.Event.Common;
using Game.GamePlaySystem.Build;
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
            //0.给玩家准备提示（假的）
            yield return MonoApp.Instance.StartCoroutine(LoadModule(0, null));

            //1.加载玩家存档
            yield return MonoApp.Instance.StartCoroutine(LoadModule(1, LoadData()));

            //2.加载建筑信息
            yield return MonoApp.Instance.StartCoroutine(LoadModule(2, LoadBuildings()));

            // 3.放置计算
            yield return MonoApp.Instance.StartCoroutine(LoadModule(3, CalculateOfflineData()));

            //4.加载Scene
            yield return MonoApp.Instance.StartCoroutine(LoadModule(4, LoadScene()));

            //5.等1.5秒让场景加载完成
            yield return MonoApp.Instance.StartCoroutine(LoadModule(5, null));

            EventCenter.DispatchEvent(new LoadSceneFinishedEvent());
        }

        private void SendLoadingEvent(int step, LoadingStepState state)
        {
            var progress = step / 6.0f + (int)state / 5.0f / 6.0f;
            EventCenter.DispatchEvent(new LoadingEvent
            {
                progress = progress,
                text = ConfigTable.Instance.GetLoadingStepData(step).Description,
            });
        }

        private IEnumerator LoadModule(int step, IEnumerator action)
        {
            var loadingData = ConfigTable.Instance.GetLoadingStepData(step);
            
            SendLoadingEvent(step, LoadingStepState.Prepare);
            yield return new WaitForSeconds(loadingData.Preparetime);
            
            SendLoadingEvent(step, LoadingStepState.Running);
            if (action != null)
            {
                yield return MonoApp.Instance.StartCoroutine(action);
            }
            yield return new WaitForSeconds(loadingData.Runningtime);
            
            SendLoadingEvent(step, LoadingStepState.Finished);
            yield return new WaitForSeconds(loadingData.Finishtime);
            
            yield return null;
        }

        private IEnumerator LoadData()
        {
            Managers.Get<ISaveDataManager>().LoadData();
            yield return null;
        }

        private IEnumerator LoadBuildings()
        {
            BuildingManager.Instance.LoadBuildings();
            yield return null;
        }
        
        private IEnumerator CalculateOfflineData()
        {
            SystemDataManager.Instance.CalculateOfflineData();
            yield return null;
        }
        
        private IEnumerator LoadScene()
        {
            var operation = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
            while (!operation.isDone)
            {
                SendLoadingEvent(4, LoadingStepState.Running);
                yield return null;
            }
            yield return null;
        }
    }
}