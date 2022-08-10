namespace InternalMeetingApp
{
    public static class Static
    {
        public static int GetInt()
        {       
            int number;
            bool correct = false;
            do
            {
                var input = Console.ReadLine();
                if (!int.TryParse(input, out number))
                {
                    Console.WriteLine("Wrong input");
                }
                else
                {
                    correct = true;
                }               

            } while (!correct);
            return number;
        }
        public static void ShowItems<T>(List<T> items)
        {
            if (items is null) return;
            
            int x = 1;
            for (int i = 0; i < items.Count; i++)
            {
                Console.WriteLine($"[{x++}] {items[i]}");
            }            
        }
        public static DateTime GetDate()
        {          
            var correct = false;
            DateTime date;
            do
            {
                var input = Console.ReadLine();
                if (input == String.Empty)
                {
                    return DateTime.MinValue;
                }
                if (!DateTime.TryParse(input, out date))
                {
                    Console.WriteLine("Wrong input. Enter (yyyy-MM-dd HH-mm)");
                }
                else
                {
                    correct = true;
                }
            } while (!correct);

            return date;        
        }

    }
}
