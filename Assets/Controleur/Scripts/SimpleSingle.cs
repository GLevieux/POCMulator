using UnityEngine;

public class SimpleSingle<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;
    public static T sharedInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<T>();
                if (instance == null)
                    Debug.Log("SimpleSingle error !");
            }
            return instance;
        }
    }
}
