using MeetingApp.DAL.Entities;
using MeetingApp.DAL.Enums;

namespace MeetingApp.BL.Interfaces
{
    public interface IService
    {
        bool AddPersonToMeeting(Person person, Meeting meeting);
        bool CreateMeeting(string name, Person person, string description, Category category, MeetingType type, DateTime startDate, DateTime endDate);
        bool CreatePerson(string name, string username, string password);
        bool DeleteMeeting(Person person, Meeting meeting);
        List<Meeting> GetMeetings();
        List<Meeting> GetMeetings(Category category);
        List<Meeting> GetMeetings(DateTime startDate, DateTime endDate);
        List<Meeting> GetMeetings(int attendees);
        List<Meeting> GetMeetings(Person person);
        List<Meeting> GetMeetings(MeetingType type);
        List<Meeting> GetMeetings(string description);
        List<Person> GetPeople();
        Person GetPerson(string username);
        Person LogIn(string username, string password);
        bool RemovePersonFromMeeting(Person person, Meeting meeting);
    }
}