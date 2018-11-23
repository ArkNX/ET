using UnityEngine;

namespace ETModel
{
    public class ComponentView: MonoBehaviour
    {
        public static GameObject Create(string name)
        {
            if (name == "Scene")
            {
                Log.Debug($"11111111111111111111111111111111");
            }
            GameObject gameObject = new GameObject(name);
            gameObject.AddComponent<ComponentView>();
            return gameObject;
        }
    }
}