
using System;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class RaceDraggerControl : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private float _captureMinDistance;
        
        private Race _raceToMove;
        private Planet _source;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnTouchStart();
            }

            if (Input.GetMouseButtonUp(0) && _source != null)
            {
                OnTouchEnd();
            }
        }
        
        private void OnTouchStart()
        {
            var touchPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _source = GetNearestPlanet(touchPos, _captureMinDistance, planet => planet.RacesCount > 0);

            if (_source != null)
            {
                _raceToMove = GetRaceByTouchPos(_source, touchPos);
            }
        }

        private void OnTouchEnd()
        {
            var touchPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var target = GetNearestPlanet(touchPos, _captureMinDistance, planet => planet.RacesCount < 2);

            if (target != null)
            {
                if (target.AddRace(_raceToMove))
                {
                    _source.RemoveRace(_raceToMove);
                }
            }
            
            _source = null;
            _raceToMove = null;
        }

        private Planet GetNearestPlanet(Vector3 touchPos, float minDistance, Func<Planet, bool> planetCondition)
        {
            Planet result = null;
            var planets = GameController.Instance.Planets;

            var minSqrDistance = float.MaxValue;

            foreach (var planet in planets)
            {
                if(planetCondition != null && !planetCondition(planet)) continue;

                var posDif = planet.transform.position - touchPos;
                var sqrDistance = posDif.x * posDif.x + posDif.y * posDif.y;
                
                if (sqrDistance < minDistance * minDistance && sqrDistance < minSqrDistance)
                {
                    minSqrDistance = sqrDistance;
                    result = planet;
                }
            }

            return result;
        }
        private Race GetRaceByTouchPos(Planet source, Vector3 touchPos)
        {
            var id = source.RacesCount == 1 ? 
                0 : 
                (touchPos.x - source.transform.position.x < 0 ? 0 : 1);//0 for left and 1 for right
            
            return source.GetRaceById(id);
        }
    }
}