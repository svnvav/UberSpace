using System;
using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class Taxi : MonoBehaviour, IPersistable
    {
        [SerializeField] private float _speed;
        [SerializeField] private AnimationCurve _speedBoost;
        [SerializeField] private float _takePassengerRadius;

        public bool IsIdle => _current == null;
        
        private Order _current;
        private Action _onCurrentComplete;
        private float _speedBoostTime;

        public void ExecuteOrder(Order order, Action onComplete)
        {
            _current = order;
            _onCurrentComplete = onComplete;
        }

        public void GameUpdate(float deltaTime)
        {
            if (_speedBoostTime > 0f)
            {
                _speedBoostTime -= deltaTime;
            }
            else
            {
                _speedBoostTime = 0f;
            }
            if (IsIdle)
            {
                Idle();
                return;
            }
            switch (_current.Status)
            {
                case OrderStatus.Queued:
                    _current.Take();
                    break;
                case OrderStatus.Taken:
                    MoveTo(_current.GetCurrentPointToMove(), deltaTime);
                    if (Vector3.SqrMagnitude(transform.position - _current.GetCurrentPointToMove()) <
                        _takePassengerRadius * _takePassengerRadius)
                    {
                        _current.StartExecuting();
                        _speedBoostTime = 1;
                    }
                    break;
                case OrderStatus.Executing:
                    MoveTo(_current.GetCurrentPointToMove(), deltaTime);
                    if (Vector3.SqrMagnitude(transform.position - _current.GetCurrentPointToMove()) <
                        _takePassengerRadius * _takePassengerRadius)
                    {
                        _current.Complete();
                        _onCurrentComplete?.Invoke();
                        _speedBoostTime = 1;
                    }
                    break;
                case OrderStatus.Completed:
                    _current = null;
                    break;
            }
        }

        private void Idle()
        {
        }


        private void MoveTo(Vector3 destination, float deltaTime)
        {
            transform.LookAt(destination, Vector3.forward);
            var boost = 1 + _speedBoost.Evaluate(1 - _speedBoostTime);
            transform.Translate(boost * _speed  * deltaTime * transform.forward, Space.World);
        }

        public void Save(GameDataWriter writer)
        {
            writer.Write(transform.localPosition);
            writer.Write(transform.localRotation);
        }

        public void Load(GameDataReader reader)
        {
            _current = null;
            transform.localPosition = reader.ReadVector3();
            transform.localRotation = reader.ReadQuaternion();
        }
    }
}