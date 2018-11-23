using System;
using UnityEngine;

namespace ETModel
{
    public class ComponentView: MonoBehaviour
    {
        public Component Component { get; private set; }
        
        public static GameObject Create(Component component)
        {
            Type type = component.GetType();
            if (type.IsDefined(typeof(HideInInspector), false))
            {
                return null;
            }
            GameObject gameObject = new GameObject(type.Name);
            gameObject.AddComponent<ComponentView>().Component = component;
            return gameObject;
        }
    }
}