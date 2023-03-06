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
            //0.给玩家准备提示（假的）
            SendLoadingEvent(0, LoadingStepState.Prepare);
            yield return new WaitForSeconds(0.5f);
            SendLoadingEvent(0, LoadingStepState.Finished);
            yield return new WaitForSeconds(0.2f);
            
            //1.加载玩家存档
            SendLoadingEvent(1, LoadingStepState.Prepare);
            Managers.Get<ISaveDataManager>().LoadData();
            yield return new WaitForSeconds(0.5f);
            SendLoadingEvent(1, LoadingStepState.Finished);
            yield return new WaitForSeconds(0.3f);

            //2.加载建筑信息
            SendLoadingEvent(2, LoadingStepState.Prepare);
            BuildingManager.Instance.LoadBuildings();
            yield return new WaitForSeconds(0.4f);
            SendLoadingEvent(2, LoadingStepState.Running);
            yield return new WaitForSeconds(0.3f);
            SendLoadingEvent(2, LoadingStepState.Finished);
            yield return new WaitForSeconds(0.3f);
            
            // todo 3.放置计算
            SendLoadingEvent(3, LoadingStepState.Prepare);
            yield return new WaitForSeconds(0.7f);
            SendLoadingEvent(3, LoadingStepState.Finished);
            yield return new WaitForSeconds(0.3f);
            
            //4.加载Scene
            SendLoadingEvent(4, LoadingStepState.Prepare);
            yield return new WaitForSeconds(0.3f);
            var operation = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
            while (!operation.isDone)
            {
                SendLoadingEvent(4, LoadingStepState.Running);
                yield return null;
            }
            yield return new WaitForSeconds(0.6f);
            SendLoadingEvent(4, LoadingStepState.Finished);
            yield return new WaitForSeconds(0.3f);
            
            //5.等1.5秒让场景加载完成
            SendLoadingEvent(5, LoadingStepState.Running);
            yield return new WaitForSeconds(0.8f);
            SendLoadingEvent(5, LoadingStepState.Finished);
            yield return new WaitForSeconds(0.7f);

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
    }
}