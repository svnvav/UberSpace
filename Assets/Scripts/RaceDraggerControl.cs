
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
        private Planet _departure, _potentialDestination;

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
            //TODO:
        }
        
        private void OnTouchStart()
        {
            _captured = true;
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
            OnControlRelease();
            var touchPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var destination = GetNearestPlanet(touchPos, _captureMinDistance, planet => !planet.IsFull);

            if (destination != null)
            {
                GameController.Instance.TransferRace(_raceToMove, _departure, destination);
            }
            
            _departure = null;
            _raceToMove = null;
            _captured = false;
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
        }

        private void OnControlCapture()
        {
            Time.timeScale = 0.5f;
            GameController.Instance.GameSpeed = 0.2f;
            VeilUnavailablePlanets();
        }

        private void OnControlRelease()
        {
            Time.timeScale = 1f;
            GameController.Instance.GameSpeed = 1f;
            UnveilPlanets();
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