using MeetingApp.BL;
using MeetingApp.BL.Interfaces;
using MeetingApp.DAL.Entities;
using MeetingApp.DAL.Enums;

namespace InternalMeetingApp
{
    public class MeetingsUI
    {
        private IService Service { get; set; }
        private Person LogedInnPerson { get; set; }
        private readonly string _guest = "Guest";
        public MeetingsUI()
        {
            Service = new Service();
            LogedInnPerson = new Person();
        }

        public void UserInterface()
        {          
            Console.WriteLine($"Logged in as: {LogedInnPerson.Username??_guest}");
            Console.WriteLine("Select:");
            Console.WriteLine("" +
                "[1] - Log in\n" +
                "[2] - Create user\n" +
                "[3] - Create meeting \n" +
                "[4] - Delete meeting\n" +
                "[5] - Add person to meeting\n" +
                "[6] - Remove person from meeting\n" +
                "[7] - Filter meetings");

            var selection = Static.GetInt();

            switch (selection)
            {
                case 1:
                    LogIn();
                    break;
                    case 2:
                    CreateUser();                                      
                    break;
                case 3:
                    CreateMeeting();
                    break;
                case 4:
                    DeleteMeeting();
                    break;
                case 5:
                    AddPersonToMeeting();
                    break;
                case 6:
                    RemovePersonFromMeeting();
                    break;
                case 7:
                    MeetingFilter();
                    break;

                default:
                    Console.WriteLine("No such selection");
                    break;
            }
        }
        public void MeetingFilter()
        {
            Console.Clear();
            Console.WriteLine("Please select how to filter meetings: ");
            Console.WriteLine("" +
                "[1] - By description\n" +
                "[2] - By responsible person\n" +
                "[3] - By category\n" +
                "[4] - By meeting type\n" +
                "[5] - By dates\n" +
                "[6] - By number of attendees ");

            var selection = Static.GetInt();
            ShowMeetings(selection);
        }
        public void LogIn()
        {
            Console.Clear();
            Console.WriteLine("Please log in:");
            Console.Write("Username: ");
            var username = Console.ReadLine();
            Console.Write("Password: ");
            var password = Console.ReadLine();
            try
            {
                LogedInnPerson = Service.LogIn(username, password);
            }
            catch (Exception e )
            {
                Console.WriteLine(e.Message);
            }            
        }
        public void CreateMeeting()
        {
            Console.Clear();
            Console.WriteLine("Who is organizer?(enter username): ");
            Static.ShowItems(Service.GetPeople());
            var username = Console.ReadLine();
            var person = Service.GetPerson(username);
            if (person is null)
            {
                Console.WriteLine("Person not found");
                return;
            }

            Console.Write("Enter meeting name: ");
            var name = Console.ReadLine();          
            Console.Write("Enter meeting description: ");
            var description = Console.ReadLine();
            var category = GetCategory();
            var meetingType = GetMeetingType();

             Console.Write("Enter start date: ");
            var startDate = Static.GetDate();
            Console.Write("Enter end date: ");
            var endDate = Static.GetDate();

            var created = false;
            try
            {
                created = Service.CreateMeeting(name, person, description, category, meetingType, startDate, endDate);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);                
            }          
            if (created)
            {                
                Console.WriteLine("Meeting created successfully");
                return;
            }
            Console.WriteLine("Meeting not created");
        }
        public void CreateUser()
        {
            Console.Clear();
            bool created = false;

            Console.Write("Enter name: ");
            var name = Console.ReadLine();
            Console.Write("Enter username: ");
            var username = Console.ReadLine();
            Console.Write("Enter password: ");
            var password = Console.ReadLine();
           
            try
            {
               created = Service.CreatePerson(name, username, password);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            if(created)
            {
                Console.WriteLine("Person created!");
            }          

        }
        public void DeleteMeeting()
        { 
            int x = 1;

            Console.Clear();
            if (LogedInnPerson is null || LogedInnPerson.Username is null)
            {
                Console.WriteLine("Please log in to delete meeting");
                return;
            }        
            var meetings = Service.GetMeetings(LogedInnPerson);
            if (meetings.Count == 0)
            {
                Console.WriteLine("You do not have meetings");
                return;
            }
            Console.WriteLine("Please select meeting to delete: ");
            foreach (var meeting in meetings)
            {
                Console.WriteLine($"[{x++}] {meeting}");
            }
            int selection = Static.GetInt();
            try
            {
                var meeting = meetings[--selection];
            }
            catch (Exception)
            {
                Console.WriteLine("selection out of range");
                return;
            }
            if (Service.DeleteMeeting(LogedInnPerson, meetings[selection]))
            {
                Console.WriteLine("Meeting deleted successfully");
                return;
            }
            Console.WriteLine("Only resposible person for the meeting can delete meeting");            
        }
        public void AddPersonToMeeting()
        {
            var meeting = GetMeeting();
            if (meeting is null)
            {
                Console.WriteLine("Meeting not selected");
                return;
            }
            var person = GetPerson();

            Console.Clear();
            if (person is null)
            {
                Console.WriteLine("Person not selected");
                return;
            }
            if (Service.PersonExists(person, meeting))
            {
                Console.WriteLine($"{person.Name} already exists in meeting");
                return;
            }
            if (Service.AddPersonToMeeting(person, meeting))
            {
                Console.WriteLine($"{person.Name} added successfully");
                return;
            }
            Console.WriteLine("Person not added");
        }
        public void RemovePersonFromMeeting()
        {           
            var meeting = GetMeeting();

            Console.Clear();
            if (meeting is null)
            {
                Console.WriteLine("Meeting not selected");
                return;
            }
            if (meeting.People.Count == 0)
            {
                Console.WriteLine("No people is added to a meeting");
                return;
            }
            var person = GetPerson();
            try
            {
                Service.RemovePersonFromMeeting(person, meeting);             
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            Console.WriteLine($"{person.Name} was removed successfully");
        }
        public void ShowMeetings(int selection)
        {
            var meetings = new List<Meeting>();
            Console.Clear();

           

            switch (selection)
            {
                case 1:
                    Console.WriteLine("Enter description");
                    meetings = Service.GetMeetings(Console.ReadLine());                    
                    break;
                case 2:
                    var person = GetPerson();                   
                    meetings = Service.GetMeetings(person);
                    break;
                case 3:
                    var category = GetCategory();
                    meetings = Service.GetMeetings(category);                    
                    break;
                case 4:
                    var type = GetMeetingType();
                    meetings = Service.GetMeetings(type);
                    break;
                case 5:
                    Console.WriteLine("Enter start date, and/or end date: ");
                    meetings = Service.GetMeetings(Static.GetDate(), Static.GetDate());
                    break;
                case 6:
                    Console.WriteLine("Enter number of atendees");
                    meetings = Service.GetMeetings(Static.GetInt());
                    break;
                default:
                    Console.WriteLine("No such selection");
                    break;
            }
            if (meetings.Count == 0 || meetings is null)
            {
                Console.WriteLine("No meetings by this filter");
                return;
            }
            Static.ShowItems(meetings);
            Console.ReadKey();
        }
        private Meeting GetMeeting()
        {
            var meetings = Service.GetMeetings();
            if (meetings.Count == 0 || meetings is null)
            {
                Console.WriteLine("No meetings created");
                return null;
            }
            Console.WriteLine("Select meeting: ");
            Static.ShowItems(meetings);
            var meetingSelection = Static.GetInt();
            if (!Validator.InRange(meetings, meetingSelection))
            {
                Console.WriteLine("Wrong selection");
                return null;
            }
           return meetings[--meetingSelection];
        }
        private Person GetPerson()
        {
            var people = Service.GetPeople();

            Console.WriteLine("Select person: ");
            Static.ShowItems(people);
            var personSelection = Static.GetInt();

            if (!Validator.InRange(people, personSelection))
            {
                Console.WriteLine("Wrong selection");
                return null;
            }
            return people[--personSelection];
        }
        private static Category GetCategory()
        {
            Console.WriteLine("Select category: ");
            var categories = Enum.GetNames(typeof(Category)).ToList();
            Static.ShowItems(categories);
            var selection = Static.GetInt();
            selection--;
            return (Category)selection;
        }
        private static MeetingType GetMeetingType()
        {
            Console.WriteLine("Select type of meeting: ");
            var meetingTypes = Enum.GetNames(typeof(MeetingApp.DAL.Enums.MeetingType)).ToList();
            Static.ShowItems(meetingTypes);
            var selection = Static.GetInt();
            selection--;
            return (MeetingType)selection;
        }
    }
}
