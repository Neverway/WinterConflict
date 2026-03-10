using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public interface IHasEventConnections { public EventConnection[] GetEventConnections(EventSequence source); }

public class EventConnection
{
    public EventSequence from;
    public EventSequence to;
    public static EventConnection[] None => new EventConnection[0];
    public EventConnection(EventSequence from, EventSequence to)
    {
        this.from = from;
        this.to = to;
    }

    public virtual void GizmosDrawConnection()
    {
        if (from == null || to == null) return;

        Vector3 pos = from.transform.position;
        Vector3 dir = to.transform.position - pos;
        dir = dir.normalized * (dir.magnitude * 0.8f);

        Gizmos.color = GizmosGetArrowColor();
        Gizmos.DrawLine(pos, pos + dir);
        GizmosDrawArrowHead(pos + dir, dir);
        dir *= 0.6f;
        GizmosDrawArrowHead(pos + dir, dir);
    }
    protected virtual Color GizmosGetArrowColor() => new Color(1, 1, 1, 0.25f);
    protected void GizmosDrawArrowHead(Vector3 pos, Vector3 dir)
    {
        Vector3 cameraDirection = SceneView.lastActiveSceneView.camera.transform.forward;

        dir = dir.normalized * -0.1f;
        Vector3 a = Quaternion.AngleAxis(-30, cameraDirection) * dir;
        Vector3 b = Quaternion.AngleAxis(30, cameraDirection) * dir;

        Gizmos.DrawLine(pos, pos + a);
        Gizmos.DrawLine(pos, pos + b);
        Gizmos.DrawLine(pos + a, pos + b);
    }

    public static implicit operator EventConnection[](EventConnection singleConnection) =>
            new EventConnection[] { singleConnection };
}

public class WaitForNew_EventConnection : EventConnection
{
    public WaitForNew_EventConnection(EventSequence from, EventSequence to) : base(from, to) { }

    public override void GizmosDrawConnection()
    {
        base.GizmosDrawConnection();
        SwapConnectionDirection();
        base.GizmosDrawConnection();
    }
    private void SwapConnectionDirection()
    {
        EventSequence stored = from;
        from = to;
        to = stored;
    }
    protected override Color GizmosGetArrowColor() => new Color(0, 1, 1, 0.25f);
}

public static class EventConnectionExtensionMethods
{
    public static EventConnection[] GetEventConnections(this IEnumerable<Event> array, EventSequence source) =>
        array.OfType<IHasEventConnections>().GetEventConnections(source);
    public static EventConnection[] GetEventConnections(this Event single, EventSequence source) =>
    single.GetEventConnections(source);
    public static EventConnection[] GetEventConnections(this IEnumerable<EventSequence.Instruction> array, EventSequence source) =>
        array.OfType<IHasEventConnections>().GetEventConnections(source);
    public static EventConnection[] GetEventConnections(this EventSequence.Instruction single, EventSequence source) =>
        single is IHasEventConnections singleCasted ? singleCasted.GetEventConnections(source) : EventConnection.None;
    public static EventConnection[] GetEventConnections(this IEnumerable<IHasEventConnections> array, EventSequence source) =>
        array.SelectMany(i => i.GetEventConnections(source)).ToArray();
}