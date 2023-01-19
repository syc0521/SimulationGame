using Game.GamePlaySystem;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class TestCommand : MonoBehaviour
    {
        public Button destroy, quit;

        private void Start()
        {
            destroy.onClick.AddListener(DestroyHandler);
            quit.onClick.AddListener(QuitStateHandler);
            quit.gameObject.SetActive(false);
        }

        private void DestroyHandler()
        {
            BuildingManager.Instance.RemoveBuilding();
            quit.gameObject.SetActive(true);
            destroy.gameObject.SetActive(false);
        }

        private void QuitStateHandler()
        {
            BuildingManager.Instance.TransitToNormalState();
            quit.gameObject.SetActive(false);
            destroy.gameObject.SetActive(true);
        }

    }
}
