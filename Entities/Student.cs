namespace ProjectSchool.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string SchollClass { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsActive { get; set; }

        public Student() { }

        public Student(string fullName, string SchoolClass, DateTime birthDate)
        {
            FullName = fullName;
            SchollClass = SchoolClass;
            BirthDate = birthDate;
            IsActive = true;
        }
    }
}
