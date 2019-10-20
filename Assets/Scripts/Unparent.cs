using UnityEngine;

namespace DefaultNamespace
{
    public class Unparent : MonoBehaviour
    {
        public void DoUnparent()
        {
            transform.SetParent(null);
        }
    }
}