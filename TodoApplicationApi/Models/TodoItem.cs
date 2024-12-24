using TodoApplicationApi.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TodoApplicationApi.Models
{
	public class TodoItem
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; } = Guid.NewGuid();
		public string? Title { get; set; }
		public string? Description { get; set; }
		public Status Status { get; set; }
		public bool IsApproved { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}

}

