using Sirenix.OdinInspector;
using UnityEngine;

[InlineEditor()]
public abstract class SpellTrait : ScriptableObject
{

    public abstract void ApplyEffects(GameObject spell);
    public abstract string Name();
    public abstract string Value();
}
