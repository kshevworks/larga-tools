using System;
using System.Collections.Generic;
using SolidUtilities.SerializableCollections;
using UnityEngine;
using TypeReferences;

namespace LargaTools.DI
{
    public class Container : MonoBehaviour
    {
        private const string _containerResourcePath = "LargaDIContainer";
        [Serializable]
        private struct TypeReferenceContainer
        {
            [Inherits(typeof(InjectedMonoBehaviour))]
            public TypeReference type;
        }
        
        [Serializable]
        private class InjectedPrototypesDictionary : SerializableDictionary<TypeReferenceContainer, InjectedMonoBehaviour>
        {
        }
        
        [SerializeField]
        private InjectedPrototypesDictionary _injectedPrototypes;
        
        private static readonly Dictionary<Type, List<InjectedMonoBehaviour>> _behaviours =
            new Dictionary<Type, List<InjectedMonoBehaviour>>();

        private static Container _instance;
        public static bool IsInited { get; private set; }

        public static Container Instance
        {
            get
            {
                if (IsInited) 
                    return _instance;

                Init();

                return _instance;
            }
        }

        private static void Init()
        {
            _instance = FindObjectOfType<Container>();
            if (_instance == null)
            {
                _instance = Resources.Load<Container>(_containerResourcePath);
            }

            DontDestroyOnLoad(_instance);
            IsInited = true;
        }

        public static void Register<T>(T injectedMonoBehaviour) where T : InjectedMonoBehaviour
        {
            if (!IsInited)
                Init();
            
            var type = typeof(T);
            if (!_behaviours.ContainsKey(type))
                _behaviours.Add(type, new List<InjectedMonoBehaviour>());

            if (!_behaviours[type].Contains(injectedMonoBehaviour))
                _behaviours[type].Add(injectedMonoBehaviour);
        }

        public static void Unregister<T>(T injectedMonoBehaviour) where T : InjectedMonoBehaviour
        {
            var type = typeof(T);
            if (!_behaviours.TryGetValue(type, out var behaviours))
                return;

            if (_behaviours[type].Contains(injectedMonoBehaviour))
                _behaviours[type].Remove(injectedMonoBehaviour);
        }

        public Container MakeDependency<T>(bool useExisting, out T dependency, Transform parent = null)
            where T : InjectedMonoBehaviour
        {
            var type = typeof(T);
            var typeRef = new TypeReferenceContainer {type = type};
            if (!_instance._injectedPrototypes.ContainsKey(typeRef))
            {
                throw new KeyNotFoundException($"No prototype of type {type} in Injected prototypes dict");
            }
            dependency = useExisting ? GetOrCreate<T>(parent) : CreateAndGet<T>(parent);
            return _instance;
        }

        private static T GetOrCreate<T>(Transform parent = null) where T : InjectedMonoBehaviour
        {

            var type = typeof(T);
            if (!_behaviours.ContainsKey(type))
                _behaviours.Add(type, new List<InjectedMonoBehaviour>());

            if (_behaviours[type].Count == 1)
            {
                return (T) _behaviours[type][0];
            }

            return CreateInternal<T>(type, parent);
        }

        private static T CreateAndGet<T>(Transform parent) where T : InjectedMonoBehaviour
        {
            var type = typeof(T);

            if (!_behaviours.ContainsKey(type))
                _behaviours.Add(type, new List<InjectedMonoBehaviour>());

            return CreateInternal<T>(type, parent);
        }

        private static T CreateInternal<T>(Type type, Transform parent) where T : InjectedMonoBehaviour
        {
            var typeRef = new TypeReferenceContainer {type = type};
            var injectedMonoBehaviour = Instantiate(_instance._injectedPrototypes[typeRef], parent);

            if (!_behaviours[type].Contains(injectedMonoBehaviour))
                _behaviours[type].Add(injectedMonoBehaviour);

            return (T) injectedMonoBehaviour;
        }

        public void Update()
        {
            foreach (var (_, behaviours) in _behaviours)
            {
                foreach (var behaviour in behaviours)
                {
                    behaviour.OnInjectedUpdate();
                }
            }
        }

        public void LateUpdate()
        {
            foreach (var (_, behaviours) in _behaviours)
            {
                foreach (var behaviour in behaviours)
                {
                    behaviour.OnInjectedLateUpdate();
                }
            }
        }

        public void FixedUpdate()
        {
            foreach (var (_, behaviours) in _behaviours)
            {
                foreach (var behaviour in behaviours)
                {
                    behaviour.OnInjectedFixedUpdate();
                }
            }
        }
    }
}