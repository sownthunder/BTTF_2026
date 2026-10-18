using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace QuanticBrains.Core.Scripts.Agent
{
	[InlineEditor(InlineEditorObjectFieldModes.Foldout)]
	[DisableInInlineEditors]
    public abstract class EnvironmentManager : ScriptableObject
    {
	    public abstract void Initialize(AgentController ac);

	    public abstract void BeginEpisode(AgentController ac);
    }
}
