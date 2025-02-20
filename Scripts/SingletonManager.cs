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
                // Generic type'i al
                var genericType = i.GetGenericArguments()[0];

                // Instance property'sini bul
                var instanceProperty = i.GetProperty("Instance", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (instanceProperty != null && genericType.IsInstanceOfType(component))
                {
                    // Instance property'sine bileşeni ata
                    instanceProperty.SetValue(null, component); // Static property, so pass null as target
                    Debug.Log($"Singleton atandı: {component.GetType().Name}");
                }
            }
        }
    }
}