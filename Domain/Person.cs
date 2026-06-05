using System.ComponentModel.DataAnnotations;

namespace RiraisTask.Domain
{
    public class Person
    {
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(10)]
        public string NationalCode { get; set; } = string.Empty;

        public DateOnly BirthDate { get; set; }

        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}
