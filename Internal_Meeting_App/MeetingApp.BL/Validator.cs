namespace MeetingApp.BL
{
    public static class Validator
    {
        public static bool IsNull<T>(T value) => value is null;
        public static bool ValidateSelection<T>(List<T> items, int selection) => items.Count <= selection;


    }


}
