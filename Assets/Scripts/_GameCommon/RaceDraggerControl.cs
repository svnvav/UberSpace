
using System;
using Svnvav.UberSpace.CoreScene;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class RaceDraggerControl : MonoBehaviour
    {
        [SerializeField] private Arrow _arrow;
        [SerializeField] private float _captureMinDistance;
        [SerializeField] private GameController _gameController;

        private Race _raceToMove;
        private Planet _departure, _destination;

        private bool _captured;
        
        private Camera _mainCamera;

        public Camera MainCamera
        {
            get
            {
                if (_mainCamera == null)
                {
                    _mainCamera = Camera.main;
                }

                return _mainCamera;
            }
        }

        private void Start()
        {
            _gameController.OnAddPlanet.RegisterCallback(OnAddPlanet);
            _gameController.OnRemovePlanet.RegisterCallback(OnRemovePlanet);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnTouchStart();
            }

            if (_captured)
            {
                SearchPotentialDestination();
            }

            if (Input.GetMouseButtonUp(0) && _departure != null)
            {
                OnTouchEnd();
            }
        }

        private void SearchPotentialDestination()
        {
            var touchPos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            touchPos.z = 0;
            _destination = GetNearestPlanet(touchPos, _captureMinDistance, planet => !planet.IsFull);
            var start = _departure.transform.position;
            _arrow.SetPosition(0, start);
            if (_destination != null)
            {
                _arrow.SetPosition(1, _destination.OnTheEdgeFrom(start));
            }
            else
            {
                _arrow.SetPosition(1, touchPos);
            }
        }
        
        private void OnTouchStart()
        {
            var touchPos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            _departure = GetNearestPlanet(touchPos, _captureMinDistance, planet => !planet.IsEmpty);

            if (_departure != null)
            {
                _raceToMove = _departure.GetRaceByTouchPos(touchPos);
                OnControlCapture();
            }
        }

        private void OnTouchEnd()
        {
            if (_destination != null && _destination != _departure)
            {
                _gameController.TransferRace(_raceToMove, _departure, _destination);
            }
            
            OnControlRelease();
            Clear();
        }

        private void Clear()
        {
            _departure = null;
            _destination = null;
            _raceToMove = null;
            _arrow.Clear();
        }

        private void OnAddPlanet(Planet  planet)
        {
            if (_captured)
            {
                planet.SetVeiling(true);
            }
        }

        private void OnRemovePlanet(Planet planet)
        {
            planet.SetVeiling(false);
            if (planet == _departure)
            {
                OnControlRelease();
                Clear();
            }
        }

        private void OnControlCapture()
        {
            _captured = true;
            Time.timeScale = 0.5f;
            _gameController.GameSpeed = 0.2f;
            SetPlanetsVeiling();
        }

        private void OnControlRelease()
        {
            Time.timeScale = 1f;
            _gameController.GameSpeed = 1f;
            UnveilPlanets();
            _captured = false;
        }

        private Planet GetNearestPlanet(Vector3 point, float minDistance, Func<Planet, bool> planetCondition)
        {
            Planet result = null;

            var minSqrDistance = float.MaxValue;

            foreach (var planet in _gameController.Planets)
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

        private void SetPlanetsVeiling()
        {
            foreach (var planet in _gameController.Planets)
            {
                if(planet.IsFull) planet.SetVeiling(true);
            }
        }

        private void UnveilPlanets()
        {
            foreach (var planet in _gameController.Planets)
            {
                planet.SetVeiling(false);
            }
        }
    }
}