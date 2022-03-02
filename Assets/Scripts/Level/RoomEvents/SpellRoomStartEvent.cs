public class SpellRoomStartEvent : CollectibleRoomStartEvent
{
    protected override string GetCollectibleGeneratorTag()
    {
        return "RandomSpellGenerator";
    }
}
