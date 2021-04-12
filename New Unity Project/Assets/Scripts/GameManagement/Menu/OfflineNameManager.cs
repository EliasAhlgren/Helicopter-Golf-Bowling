using UnityEngine;

namespace GameManagement.Menu
{
    public class OfflineNameManager : MonoBehaviour
    {
        public int index;
        public GameObject prefab;
        public OfflinePlayers file;

        public static int currentIndex;
        
        void AddNewField()
        {
            OfflineNameManager o = Instantiate(prefab).GetComponent<OfflineNameManager>();
            o.index = currentIndex + 1;
        }
    } 
}