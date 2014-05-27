using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace CustomTableRegion
{
	/// <summary>
	/// Db context with application objects.
	/// </summary>
	public class Db : DbContext
	{
		/// <summary>
		/// Our custom entity
		/// </summary>
		public DbSet<Entities.CustomObject> CustomObjects { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Db() : base("piranha") { }

		/// <summary>
		/// Creates the model.
		/// </summary>
		protected override void OnModelCreating(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new Entities.Maps.CustomObjectMap());

			base.OnModelCreating(modelBuilder);
		}
	}
}