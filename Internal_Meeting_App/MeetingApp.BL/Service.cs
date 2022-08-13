using MeetingApp.BL.Interfaces;
using MeetingApp.DAL.Entities;
using MeetingApp.DAL.Enums;
using MeetingApp.DAL.Interfaces;
using MeetingApp.DAL.Repositories;

namespace MeetingApp.BL
{
    public class Service : IService
    {
        private IMeetingRepository MeetingRepository { get; set; }

        public Service()
        {
            MeetingRepository = new MeetingRepository();
        }
        public Person LogIn(string username, string password)
        {
            var users = MeetingRepository.GetPeople();
            var user = users.SingleOrDefault(u => u.Username == username);
            if (user is null)
            {
                throw new Exception(message: "User not found");
            }
            if (user.Password != password)
            {
                throw new Exception(message: "Wrong password");

            }
            return MeetingRepository.LogIn(username, password);

        }
        public bool CreateMeeting(string name,
            Person person,
            string description,
            Category category,
            MeetingType type,
            DateTime startDate,
            DateTime endDate)
        {
            if (name is null ||
            person is null ||
            description is null) return false;

            if (startDate == DateTime.MinValue || endDate == DateTime.MinValue)
            {
                throw new Exception(message: "Please enter dates");
            }
            MeetingRepository.CreateMeeting(name, person, description, category, type, startDate, endDate);

            return true;
        }
        public bool DeleteMeeting(Person person, Meeting meeting)
        {
            if (meeting.ResponsiblePerson.Id == person.Id)
            {
                return MeetingRepository.DeleteMeeting(meeting);
            }
            return false;
           
        }
        public bool AddPersonToMeeting(Person person, Meeting meeting)
        {
            if (person is null || meeting is null) return false;

            return MeetingRepository.AddPersonToMeeting(person, meeting);
        }
        public bool RemovePersonFromMeeting(Person person, Meeting meeting)
        {
            if (meeting.ResponsiblePerson.Id == person.Id)
            {
                throw new Exception(message: "Cannot remove responsible person from meeting");               
            }
            return MeetingRepository.RemovePersonFromMeeting(person, meeting);

        }
        public bool CreatePerson(string name, string username, string password)
        {
            if (name is null || username is null || password is null) return false;

            var people = MeetingRepository.GetPeople();
            if (people.Any(u => u.Username == username))
            {
                throw new Exception(message: $"Username: {username} already exist");               
            }
            return MeetingRepository.CreatePerson(name, username, password);
        }
        public List<Person> GetPeople() => MeetingRepository.GetPeople();
        public Person GetPerson(string username)
        {
            return MeetingRepository.GetPeople().SingleOrDefault(p => p.Username == username);
        }     
        public List<Meeting> GetMeetings() => MeetingRepository.GetMeetings();
        public List<Meeting> GetMeetings(string description)
        {
            if (description is not null || description != String.Empty)
            {
                return MeetingRepository.GetMeetings(description);
            }
            return null;
        }
        public List<Meeting> GetMeetings(Person person)
        {
            if (person is not null)
            {
                return MeetingRepository.GetMeetings(person);
            }
            return null;
        }
        public List<Meeting> GetMeetings(Category category)
        {
            return MeetingRepository.GetMeetings(category);
        }
        public List<Meeting> GetMeetings(MeetingType type)
        {
            return MeetingRepository.GetMeetings(type);
        }
        public List<Meeting> GetMeetings(DateTime startDate, DateTime endDate)
        {
            if (endDate == DateTime.MinValue)
            {
                return MeetingRepository.GetMeetings(startDate);
            }
            return MeetingRepository.GetMeetings(startDate, endDate);
        }
        public List<Meeting> GetMeetings(int attendees)
        {
            return MeetingRepository.GetMeetings(attendees);
        }
        public bool PersonExists(Person person, Meeting meeting) => meeting.People.Any(i => i.Id == person.Id);


    }

}
