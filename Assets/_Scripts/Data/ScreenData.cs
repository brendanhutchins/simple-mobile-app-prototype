using System;
using UnityEngine;

namespace _Scripts.Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CaptionsData", order = 2)]
    public class ScreenData : ScriptableObject
    {
        public CaptionsData[] captionsData;
    }
}
