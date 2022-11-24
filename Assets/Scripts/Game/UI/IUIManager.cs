using Game.Core;

namespace Game.UI
{
    public interface IUIManager : IManager
    {
        /// <summary>
        /// 打开UI面板
        /// </summary>
        /// <param name="option">数据</param>
        /// <typeparam name="T">面板类型</typeparam>
        /// <returns>面板</returns>
        T OpenPanel<T>(BasePanelOption option = null) where T : UIPanel;
        
        /// <summary>
        /// 创建UI面板
        /// </summary>
        /// <param name="option">数据</param>
        /// <typeparam name="T">面板类型</typeparam>
        /// <returns>面板</returns>
        void CreatePanel<T>(BasePanelOption option = null) where T : UIPanel;
        
        /// <summary>
        /// 销毁面板
        /// </summary>
        /// <param name="panel">面板</param>
        void DestroyPanel(UIPanel panel);

        /// <summary>
        /// 是否有面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool HasPanel<T>() where T : UIPanel;
    }
}