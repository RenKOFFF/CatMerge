using UnityEngine;

namespace Components
{
    public class DestroyObject : MonoBehaviour
    {
        [SerializeField] private GameObject objectToBeDestroyed;

        public void Destroy()
        {
            Destroy(objectToBeDestroyed);
        }
    }
}
