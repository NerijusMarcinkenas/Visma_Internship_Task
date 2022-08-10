using MeetingApp.DAL.Enums;


namespace MeetingApp.DAL.Entities
{
    public class Meeting
    { 
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Person ResponsiblePerson { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public MeetingType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Person> People { get; set; } = new List<Person>();
        public Meeting() { }

        public Meeting(string name,
            Person responsiblePerson,
            string description,
            Category category,
            MeetingType type,
            DateTime startDate,
            DateTime endDate)
        {
            Id = Guid.NewGuid();
            Name = name;
            ResponsiblePerson = responsiblePerson;
            Description = description;
            Category = category;
            Type = type;
            StartDate = startDate;
            EndDate = endDate;

        }

        public override string ToString()
        {
            return $" Name: {Name}, " +
                $"responsible person: {ResponsiblePerson.Name}, " +
                $"description: {Description}, " +
                $"category: {Category}, " +
                $"type: {Type}, " +
                $"start date: {StartDate}, " +
                $"end date: {EndDate}";
        }
    }
}
