using UnityEngine;

// a Generic Singleton class

namespace UDEV
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        // private static instance
        static T m_ins;

        // public static instance used to refer to Singleton (e.g. MyClass.Instance)
        public static T Ins
        {
            get
            {
                return m_ins;
            }
        }

        public virtual void Awake()
        {
            MakeSingleton(true);
        }

        public void MakeSingleton(bool destroyOnload)
        {
            if (m_ins == null)
            {
                m_ins = this as T;
                if (destroyOnload)
                {
                    DontDestroyOnLoad(this.gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

}