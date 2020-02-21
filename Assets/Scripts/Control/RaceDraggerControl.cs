
using System;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class RaceDraggerControl : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private float _captureMinDistance;
        [SerializeField] private Taxi _taxi;

        private Race _raceToMove;
        private Planet _departure;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnTouchStart();
            }

            if (Input.GetMouseButtonUp(0) && _departure != null)
            {
                OnTouchEnd();
            }
        }

        private void OnControlCapture()
        {
            Time.timeScale = 0.5f;
            GameController.Instance.GameSpeed = 0.2f;
        }

        private void OnControlRelease()
        {
            Time.timeScale = 1f;
            GameController.Instance.GameSpeed = 1f;
        }

        private void OnTouchStart()
        {
            var touchPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _departure = GetNearestPlanet(touchPos, _captureMinDistance, planet => planet.RacesCount > 0);

            if (_departure != null)
            {
                _raceToMove = GetRaceByTouchPos(_departure, touchPos);
                OnControlCapture();
            }
        }

        private void OnTouchEnd()
        {
            OnControlRelease();
            var touchPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var destination = GetNearestPlanet(touchPos, _captureMinDistance, planet => planet.RacesCount < 2);

            if (destination != null)
            {
                _taxi.AddOrder(_raceToMove, _departure, destination);
            }
            
            _departure = null;
            _raceToMove = null;
        }
        
        public Planet GetNearestPlanet(Vector3 point, float minDistance, Func<Planet, bool> planetCondition)
        {
            Planet result = null;

            var minSqrDistance = float.MaxValue;

            foreach (var planet in GameController.Instance.Planets)
            {
                if(planetCondition != null && !planetCondition(planet)) continue;

                var posDif = planet.transform.position - point;
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