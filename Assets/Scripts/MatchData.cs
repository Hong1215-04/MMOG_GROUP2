public static class MatchData
{
    public static CharacterSlot Player1Character { get; private set; }
    public static CharacterSlot Player2Character { get; private set; }

    public static void SetSelections(CharacterSlot p1, CharacterSlot p2)
    {
        Player1Character = p1;
        Player2Character = p2;
    }
}