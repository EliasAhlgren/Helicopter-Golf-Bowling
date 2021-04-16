using System.Collections.Generic;
using UnityEngine;

namespace GameManagement
{
    [CreateAssetMenu(fileName = "Player", menuName = "OfflinePlayers", order = 0)]
    public class OfflinePlayers : ScriptableObject
    {
        public List<string> playerNames;
        public List<float> playerScores;
    }
}