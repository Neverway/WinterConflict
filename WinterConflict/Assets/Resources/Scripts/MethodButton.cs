using System;
using System.Reflection;

[Serializable]
public class MethodButton
{
    public string methodReferenceTag;
    public MethodButton(string methodReferenceTag) => this.methodReferenceTag = methodReferenceTag;

    public void Invoke(object fromObject)
    {
        if (fromObject == null) return;
        if (ReferenceTagAttribute.TryGet(methodReferenceTag, out MethodInfo method))
        {
            method.Invoke(fromObject, null);
        }
    }

    public static implicit operator MethodButton(string buttonName) => new MethodButton(buttonName);
}