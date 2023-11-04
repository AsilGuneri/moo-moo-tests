using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.FSM
{
    public class BaseStateMachine : NetworkBehaviour
    {
        [SerializeField] private BaseState _initialState;
        private Dictionary<Type, Component> _cachedComponents = new Dictionary<Type, Component>();

        protected bool isActive = false;

        private void Awake()
        {
            Init();
            isActive = true;
        }

        public BaseState CurrentState { get; set; }

        private void Update()
        {
            if (!isServer) return;
            Execute();
        }

        public virtual void Init()
        {
            if (!isServer) return;
            CurrentState = _initialState;
        }

        public virtual void Execute()
        {
            CurrentState.Execute(this);
        }

        // Allows us to execute consecutive calls of GetComponent in O(1) time
        public new T GetComponent<T>() where T : Component
        {
            if (_cachedComponents.ContainsKey(typeof(T)))
                return _cachedComponents[typeof(T)] as T;

            var component = base.GetComponent<T>();
            if (component != null)
            {
                _cachedComponents.Add(typeof(T), component);
            }
            return component;
        }

    }
}