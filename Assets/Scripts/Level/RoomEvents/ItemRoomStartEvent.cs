public class ItemRoomStartEvent : CollectibleRoomStartEvent
{
    protected override string GetCollectibleGeneratorTag()
    {
        return "RandomItemGenerator";
    }

}
