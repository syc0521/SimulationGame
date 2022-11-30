using Game.GamePlaySystem;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class TestCommand : MonoBehaviour
    {
        public Button build1, build2, destroy, quit;

        private void Start()
        {
            build1.onClick.AddListener(BuildHandler);
            build2.onClick.AddListener(Build2Handler);
            destroy.onClick.AddListener(DestroyHandler);
            quit.onClick.AddListener(QuitStateHandler);
            quit.gameObject.SetActive(false);
        }

        private void BuildHandler()
        {
            BuildingManager.Instance.AddBuilding(0);
            
        }
        
        private void Build2Handler()
        {
            BuildingManager.Instance.AddBuilding(1);
        }

        private void DestroyHandler()
        {
            BuildingManager.Instance.RemoveBuilding();
            quit.gameObject.SetActive(true);
            destroy.gameObject.SetActive(false);
            build1.gameObject.SetActive(false);
            build2.gameObject.SetActive(false);
        }

        private void QuitStateHandler()
        {
            BuildingManager.Instance.TransitToNormalState();
            quit.gameObject.SetActive(false);
            destroy.gameObject.SetActive(true);
            build1.gameObject.SetActive(true);
            build2.gameObject.SetActive(true);
        }

    }
}
