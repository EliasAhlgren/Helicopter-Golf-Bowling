using UnityEngine;

namespace GameManagement.Menu
{
    public class HostButton : MonoBehaviour
    {
        void StartServer()
        {
            PlayerPrefs.SetInt("ShouldStartClient", 1);
        }
    }
}