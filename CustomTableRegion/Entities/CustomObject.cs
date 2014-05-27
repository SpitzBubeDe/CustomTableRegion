using System;

namespace CustomTableRegion.Entities
{
	public class CustomObject
	{
		public Guid Id { get; set; }
		public bool IsDraft { get; set; }
		public Guid PageId { get; set; }
		public string Field1 { get; set; }
		public string Field2 { get; set; }
	}
}