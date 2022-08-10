using MeetingApp.DAL.Entities;
using MeetingApp.DAL.Enums;
using MeetingApp.DAL.Interfaces;
using MeetingApp.DAL.Repositories;
using System.Text.Json;

namespace Tests
{
    public class Tests
    {
        private List<Meeting> Meetings { get; set; }       
        private Meeting Meeting { get; set; } = new Meeting();
        private Person Person { get; set; } = new Person();

        private readonly string _meetingFile = "Meetings.json";
        private readonly string _peopleFile = "People.json";

        private IMeetingRepository _meetRepository;      
        [SetUp]
        public void Setup()
        {           
            _meetRepository = new MeetingRepository();
            SetupMeeting();
            SetupPerson();
        }

        [Test]
        public void CreateMeetingTest()
        {
            // Arrange
            string name = "Meeting test";
            Person responsiblePerson = new Person("Ben", "Bo", "123456");
            string description = "Test description";
            Category category = Category.CodeMonkey;
            MeetingType type = MeetingType.InPerson;
            DateTime startDate = new DateTime(2022, 08, 08, 08, 00,00);
            DateTime endDate = new DateTime(2022, 08, 08, 08, 50, 00);

            var expected = _meetRepository.CreateMeeting(name,responsiblePerson,description,category,type,startDate,endDate);   

            //Assert
            Assert.IsTrue(expected);
        }
        [Test]
        public void DeleteMeetingTest()
        {
            var meeting = _meetRepository.GetMeetings().First();
            Assert.IsNotNull(meeting);
            var expected =  _meetRepository.DeleteMeeting(meeting);
            Assert.IsTrue(expected);
        }

        [Test]
        public void AddPersonToMeetingTest()
        {
            var expected = _meetRepository.AddPersonToMeeting(Person, Meeting);
            
            Assert.IsTrue(expected);
        }
        [Test]
        public void RemovePersonFromMeetingTest()
        {
            var expected = _meetRepository.RemovePersonFromMeeting(Person, Meeting);

            Assert.IsTrue(expected);
        }

        [Test]
        public void GetMeetingsTest()
        {
            // Description filter
            var actMeetingsDesc = _meetRepository.GetMeetings("test");
            var expMeetingsDesc = Meetings.Where(d => d.Description.Contains("test", StringComparison.InvariantCultureIgnoreCase)).ToList();

            for (int i = 0; i < actMeetingsDesc.Count; i++)
            {
                Assert.AreEqual(expMeetingsDesc[i].Id, actMeetingsDesc[i].Id);
            }

            //Person filter
            var actMeetingsPerson = _meetRepository.GetMeetings(Person);
            var expMeetingsPerson = Meetings.Where(p => p.ResponsiblePerson.Id == Person.Id).ToList();

            for (int i = 0; i < actMeetingsPerson.Count; i++)
            {
                Assert.AreEqual(expMeetingsPerson[i].Id, actMeetingsPerson[i].Id);
            }

            //Category filter
            var actMeetingsCat = _meetRepository.GetMeetings(Category.CodeMonkey);
            var expMeetingsCat = Meetings.Where(c => c.Category ==Category.CodeMonkey).ToList();

            for (int i = 0; i < actMeetingsCat.Count; i++)
            {
                Assert.AreEqual(expMeetingsCat[i].Id, actMeetingsCat[i].Id);
            }

            //Meeting type filter
            var actMeetingsType =  _meetRepository.GetMeetings(MeetingType.InPerson);
            var expMeetingsType = Meetings.Where(c => c.Type == MeetingType.InPerson).ToList();

            for (int i = 0; i < actMeetingsType.Count; i++)
            {
                Assert.AreEqual(expMeetingsType[i].Id, actMeetingsType[i].Id);
            }

            //Date filter
            var startDate = new DateTime(2022, 08, 08, 08, 00, 00);
            var endDate = new DateTime(2022, 08, 08, 08, 50, 00);

            var actMeetingsDate = _meetRepository.GetMeetings(startDate, endDate);
            var expMeetingsDate = Meetings.Where(s => s.StartDate >= startDate && s.EndDate <= endDate).ToList();

            for (int i = 0; i < actMeetingsDate.Count; i++)
            {
                Assert.AreEqual(expMeetingsDate[i].Id, actMeetingsDate[i].Id);
            }

        }

        [Test]
        public void CreatePersonTest()
        {
            var expected = _meetRepository.CreatePerson("Test", "testing", "Test1");
            Assert.IsTrue(expected);
        }

        [Test]
        public void LogInTest()
        {
            var actual = _meetRepository.LogIn("testing","Test1");

            Assert.AreEqual(actual.Username, Person.Username);
            Assert.AreEqual(actual.Name, Person.Name);
        }


        private void SetupMeeting()
        {
            Meetings = JsonSerializer.Deserialize<List<Meeting>>(File.ReadAllText(_meetingFile)) ?? new List<Meeting>();
            Meeting = _meetRepository.GetMeetings().First();
        }
        private void SetupPerson()
        {
            File.WriteAllText(_peopleFile, "[]");
            _meetRepository.CreatePerson("Test", "testing", "Test1");           
            Person = new Person("Test", "testing", "Test1");           
          

        }
      
    }
}