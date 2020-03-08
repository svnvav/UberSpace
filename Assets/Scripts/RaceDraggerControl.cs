
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class RaceDraggerControl : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private LineRenderer _line;
        [SerializeField] private float _captureMinDistance;

        private Race _raceToMove;
        private Planet _departure, _destination;

        private bool _captured;

        private void Start()
        {
            GameController.Instance.RegisterOnAddPlanet(OnAddPlanet);
            GameController.Instance.RegisterOnRemovePlanet(OnRemovePlanet);
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
            var touchPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _destination = GetNearestPlanet(touchPos, _captureMinDistance, planet => !planet.IsFull);
            if (_destination != null)
            {
                _line.SetPosition(0, _departure.transform.position);
                _line.SetPosition(1, _destination.transform.position);
            }
            else
            {
                _line.SetPosition(0, Vector3.zero);
                _line.SetPosition(1, Vector3.zero);
            }
        }
        
        private void OnTouchStart()
        {
            var touchPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _departure = GetNearestPlanet(touchPos, _captureMinDistance, planet => !planet.IsEmpty);

            if (_departure != null)
            {
                _raceToMove = _departure.GetRaceByTouchPos(touchPos);
                OnControlCapture();
            }
        }

        private void OnTouchEnd()
        {
            if (_destination != null)
            {
                GameController.Instance.TransferRace(_raceToMove, _departure, _destination);
            }
            
            OnControlRelease();
            Clear();
        }

        private void Clear()
        {
            _departure = null;
            _destination = null;
            _raceToMove = null;
            _line.SetPosition(0, Vector3.zero);
            _line.SetPosition(1, Vector3.zero);
        }

        private void OnAddPlanet(Planet  planet)
        {
            if (_captured && planet.IsFull)
            {
                planet.Veil();//TODO: do it on race add because planet hasnt any race just after initialization
            }
        }

        private void OnRemovePlanet(Planet planet)
        {
            planet.Unveil();
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
            GameController.Instance.GameSpeed = 0.2f;
            VeilUnavailablePlanets();
        }

        private void OnControlRelease()
        {
            Time.timeScale = 1f;
            GameController.Instance.GameSpeed = 1f;
            UnveilPlanets();
            _captured = false;
        }

        

        private Planet GetNearestPlanet(Vector3 point, float minDistance, Func<Planet, bool> planetCondition)
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

        private void VeilUnavailablePlanets()
        {
            foreach (var planet in GameController.Instance.Planets)
            {
                if(planet.IsFull) planet.Veil();
            }
        }

        private void UnveilPlanets()
        {
            foreach (var planet in GameController.Instance.Planets)
            {
                planet.Unveil();
            }
        }
    }
}