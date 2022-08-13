namespace MeetingApp.BL
{
    public static class Validator
    {
        public static bool InRange<T>(List<T> items, int selection) => items.Count >= selection;
    }


}
