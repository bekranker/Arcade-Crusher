using System.Linq;
using UnityEngine;

public class SingletonManager : MonoBehaviour
{
    private void Awake()
    {
        // Sahnedeki tüm MonoBehaviour bileşenlerini al
        MonoBehaviour[] allComponents = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var component in allComponents)
        {
            // Bileşenin herhangi bir ISingleton<> türevini uygulayıp uygulamadığını kontrol et
            var interfaces = component.GetType().GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISingleton<>));

            foreach (var i in interfaces)
            {
                // Static Instance alanını bul ve atama yap
                var instanceProperty = i.GetProperty("Instance");
                if (instanceProperty != null)
                {
                    instanceProperty.SetValue(null, component);
                    Debug.Log($"Singleton atandı: {component.GetType().Name}");
                }
            }
        }
    }
}
