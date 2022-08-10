using MeetingApp.DAL.Entities;
using MeetingApp.DAL.Enums;

namespace MeetingApp.DAL.Interfaces
{
    public interface IMeetingRepository
    {
        public Person LogIn(string username, string password);
        public bool CreateMeeting(string name, 
            Person responsiblePerson,
            string description,
            Category category,
            MeetingType type,
            DateTime startDate,
            DateTime endDate);
        bool DeleteMeeting(Meeting meeting);
        bool AddPersonToMeeting(Person person, Meeting meeting);
        bool CreatePerson(string name, string username, string password);
        bool RemovePersonFromMeeting(Person person, Meeting meeting);
        List<Meeting> GetMeetings();
        List<Meeting> GetMeetings(string value);
        List<Meeting> GetMeetings(Person person);
        List<Meeting> GetMeetings(Category category);
        List<Meeting> GetMeetings(MeetingType type);
        public List<Meeting> GetMeetings(DateTime startDate);
        List<Meeting> GetMeetings(DateTime startDate, DateTime endDate);
        List<Meeting> GetMeetings(int attendees);
        Person GetMeetingPerson(Meeting meeting, Person person);       
        List<Person> GetPeople();



    }
}
