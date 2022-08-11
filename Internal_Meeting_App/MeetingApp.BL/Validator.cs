namespace MeetingApp.BL
{
    public static class Validator
    {
        public static bool ValidateSelection<T>(List<T> items, int selection) => items.Count <= selection;
    }


}
