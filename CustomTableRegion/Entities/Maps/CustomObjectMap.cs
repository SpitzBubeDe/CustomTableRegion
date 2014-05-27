using System;
using System.Data.Entity.ModelConfiguration;

namespace CustomTableRegion.Entities.Maps
{
	public class CustomObjectMap : EntityTypeConfiguration<CustomObject>
	{
		public CustomObjectMap() {
			HasKey(c => new { c.Id, c.IsDraft });
		}
	}
}