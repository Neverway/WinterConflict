using System;
using System.Collections.Generic;
using UnityEngine;

public class GI_ContextSystem : MonoBehaviour
{
    public Dictionary<object, List<Context>> localContextInfo;
    public List<Context> globalContextInfo;
}

public class Context
{
    public abstract class History<T> : Context
    {
        public List<T> history = new();
        public bool TryGetPrevious(out T previous, int byCount = 1)
        {
            byCount -= 1;
            int targetIndex = history.Count - byCount;
            if (history.IsIndexOutOfRange(targetIndex))
            {
                previous = default;
                return false;
            }
            previous = history[targetIndex];
            return true;
        }
    }

}


