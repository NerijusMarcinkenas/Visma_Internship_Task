using MeetingApp.DAL.Entities;
using MeetingApp.DAL.Enums;
using MeetingApp.DAL.Interfaces;
using System.Text.Json;

namespace MeetingApp.DAL.Repositories
{
    public class MeetingRepository : IMeetingRepository
    {       
        private List<Meeting> Meetings { get; set; }
        private List<Person> People { get; set; }
        private readonly string _meetingFile = "Meetings.json";
        private readonly string _peopleFile = "People.json";
        public MeetingRepository()
        {
            Meetings = new List<Meeting>();
            People = new List<Person>();
            LoadMeetings();
            LoadPeople();
        }
        
        public bool CreateMeeting(string name,
            Person responsiblePerson, 
            string description,
            Category category,
            MeetingType type,
            DateTime startDate,
            DateTime endDate)
        {
            if (name == string.Empty || responsiblePerson is null || description == String.Empty) return false;
            responsiblePerson.TimeAddedToMeeting = DateTime.Now;
            var meeting = new Meeting(name, responsiblePerson, description, category, type, startDate, endDate);          
            Meetings.Add(meeting);
            WriteChanges();
            LoadMeetings();
            return true;           
        }

        public bool DeleteMeeting(Meeting meeting)
        {
            LoadMeetings();         

            var removed = Meetings.Remove(Meetings.Single(i => i.Id == meeting.Id));
            WriteChanges();
            LoadMeetings();
            return removed;
        }
        public bool AddPersonToMeeting(Person person, Meeting meeting)
        {
            LoadMeetings();
            var meetingToAddPerson = Meetings.Single(m => m.Id == meeting.Id);

            if (meetingToAddPerson.People.Any(i => i.Id == person.Id)) return false;
            person.TimeAddedToMeeting = DateTime.Now;
            meetingToAddPerson.People.Add(person);

            WriteChanges();
            LoadMeetings();
            return true;
        }     
        public bool RemovePersonFromMeeting(Person person, Meeting meeting)
        {
            var localPerson = GetMeetingPerson(meeting, person);

            if (localPerson is null && meeting.ResponsiblePerson.Id == person.Id) return false;

            Meetings.SingleOrDefault(m => m.Id == meeting.Id).People.Remove(localPerson);
            WriteChanges();
            LoadMeetings();
            return true;
        }
        public List<Meeting> GetMeetings() => Meetings;        
        public List<Meeting> GetMeetings(string value)  
        {          
             return Meetings.Where(d=> d.Description.Contains(value, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }
        public List<Meeting> GetMeetings(Person person)
        {
            return Meetings.Where(p => p.ResponsiblePerson.Id == person.Id).ToList();
        }
        public List<Meeting> GetMeetings(Category category)
        {
            return Meetings.Where(c => c.Category == category).ToList();
        }
        public List<Meeting> GetMeetings(MeetingType type)
        {
            return Meetings.Where(c => c.Type == type).ToList();
        }
        public List<Meeting> GetMeetings(DateTime startDate, DateTime endDate)
        {            
            return Meetings.Where(s => s.StartDate >= startDate && s.EndDate <= endDate).ToList();            
        }   
        public List<Meeting> GetMeetings(DateTime startDate)
        {
           return Meetings.Where(s => s.StartDate >= startDate).ToList();
        }
        public List<Meeting> GetMeetings(int attendees)
        {

            return Meetings.Where(p => p.People.Count >= attendees).ToList();
        }
        public Person LogIn(string username, string password)
        {
            LoadPeople();
            return People.SingleOrDefault(u => u.Username == username && u.Password ==password);
        }       
        public List<Person> GetPeople()
        {
            LoadPeople();
            return People;
        }
        public bool CreatePerson(string name, string username, string password)
        {
            LoadPeople();       
            var newPerson = new Person(name, username, password);
            if (newPerson is null) return false;
           
            People.Add(newPerson);
            WriteChanges();
            return true;
        }
        public Person GetMeetingPerson(Meeting meeting, Person person)
        {
            return Meetings.Single(m => m.Id == meeting.Id).People.SingleOrDefault(p => p.Id == person.Id);
        }
        private void LoadMeetings() => Meetings = JsonSerializer.Deserialize<List<Meeting>>(File.ReadAllText(_meetingFile)) ?? new List<Meeting>();
        private void LoadPeople() => People = JsonSerializer.Deserialize<List<Person>>(File.ReadAllText(_peopleFile)) ?? new List<Person>();
        private void WriteChanges()
        {
            var meetingJson = JsonSerializer.Serialize(Meetings);
            var peopleJson = JsonSerializer.Serialize(People);
            File.WriteAllText(_meetingFile, meetingJson);
            File.WriteAllText(_peopleFile, peopleJson);
        }

       
       
    }
}
