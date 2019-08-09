using System.Collections.Generic;
using System.Linq;
using Delegates;
using UnityEngine;

namespace Tools
{
    public class ObjectPool<T>
    {
        public IList<PoolingProperty<T>> ObjectList { get; private set; }

        public PoolingProperty<T> this[int i] => ObjectList[i];

        public int Length => ObjectList.Count;

        private GameObject                _item;
        private Transform                 _parent;

        public static ObjectPool<T> InstantiatePool(GameObject item, uint length, bool inChild)
        {
            return InstantiatePool(item, length, null, inChild, null);
        }
        
        public static ObjectPool<T> InstantiatePool(GameObject item, uint length, InstantiateValue instantiateCallback)
        {
            return InstantiatePool(item, length, null, false, instantiateCallback);
        }
        
        public static ObjectPool<T> InstantiatePool(GameObject item, uint length, bool inChild,
                                                    InstantiateValue instantiateCallback)
        {
            return InstantiatePool(item, length, null, inChild, instantiateCallback);
        }

        public static ObjectPool<T> InstantiatePool(GameObject item, uint length, Transform parent = null,
                                                    bool inChild = false, InstantiateValue instantiateCallback = null)
        {
            if (inChild)
            {
                Transform listParent = new GameObject($"{item.name}s").transform;
                listParent.SetParent(parent);

                listParent.localPosition = Vector3.zero;
                parent                   = listParent;
            }

            ObjectPool<T> pool = new ObjectPool<T>
                                 {
                                     ObjectList = new List<PoolingProperty<T>>(), _item = item, _parent = parent
                                 };


            for (int i = 0; i < length; i++)
            {
                GameObject obj         = Object.Instantiate(item);
                T          objProperty = obj.GetComponent<T>();
                
                instantiateCallback?.Invoke(obj);
            
                obj.name = $"{pool.ObjectList.Count} {item.name}";
 
                PoolingProperty<T> property = new PoolingProperty<T>
                                              {
                                                  ObjectRef = objProperty, Property = objProperty as IModuleProperty
                                              };

                property.Property.Init();
            
                if (parent != null)
                {
                    property.Property.Parent = parent;
                }

                pool.ObjectList.Add(property);
            }

            return pool;
        }

        public T GetObject(InstantiateValue instantiateCallback = null)
        {
            foreach (var property in ObjectList)
            {
                if (property.Property.IsAvailable)
                {
                    return property.ObjectRef;
                }
            }
        
            var obj         = Object.Instantiate(_item);
            var objProperty = obj.GetComponent<T>();
            
            instantiateCallback?.Invoke(obj);
            
            obj.name = $"{ObjectList.Count} {_item.name}";

            PoolingProperty<T> newProperty = new PoolingProperty<T>
                                             {
                                                 ObjectRef = objProperty, Property = objProperty as IModuleProperty
                                             };

            newProperty.Property.Init();
        
            if (_parent != null)
            {
                newProperty.Property.Parent = _parent;
            }
        
            ObjectList.Add(newProperty);

            return newProperty.ObjectRef;
        }
    }

    public struct PoolingProperty<T>
    {
        public T               ObjectRef;
        public IModuleProperty Property;
    }

    public interface IModuleProperty
    {
        bool IsAvailable
        {
            get;
        }

        Transform Parent
        {
            get;
            set;
        }

        void Init();
    }
}