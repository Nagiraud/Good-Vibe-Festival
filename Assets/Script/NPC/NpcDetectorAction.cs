using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "NPC Detector", story: "Player camera [detect] NPC", category: "Action", id: "079b5f12f2e0192c363c7a150d1f08c9")]
public partial class NpcDetectorAction : Action
{
    [SerializeReference] public BlackboardVariable<PlayerInteraction> Detect;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

