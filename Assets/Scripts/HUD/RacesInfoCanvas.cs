
using System.Collections.Generic;
using UnityEngine;

namespace Svnvav.UberSpace
{
    internal class RacesInfoCanvas : MonoBehaviour
    {
        [SerializeField] private RectTransform _racesInfoPanel;
        [SerializeField] private RaceInfo _raceInfoPrefab;
        
        private List<RaceInfo> _raceInfos;

        private void Awake()
        {
            _raceInfos = new List<RaceInfo>();
        }

        public void AddRaceInfo(Race race)
        {
            var info = Instantiate(_raceInfoPrefab, _racesInfoPanel);
            info.Initialize(race);
            _raceInfos.Add(info);
        }
        
        public void RemoveRaceInfo(Race race)
        {
            var raceInfoIndex = _raceInfos.FindIndex(info => info.GetComponent<RaceInfo>().Race == race);
            var raceInfo = _raceInfos[raceInfoIndex];
            _raceInfos.RemoveAt(raceInfoIndex);
            Destroy(raceInfo.gameObject);
        }
        
    }
}