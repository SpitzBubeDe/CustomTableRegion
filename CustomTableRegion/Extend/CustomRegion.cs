using System;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using System.Web.Script.Serialization;
using Piranha.Extend;

namespace CustomTableRegion.Extend
{
	/// <summary>
	/// Custom region that stores its values in a custom table.
	/// </summary>
	[Export(typeof(IExtension))]
	[ExportMetadata("InternalId", "CustomRegion")]
	[ExportMetadata("Name", "Custom region")]
	[ExportMetadata("Type", ExtensionType.Region)]
	[Serializable]
	public class CustomRegion : Extension
	{
		/// <summary>
		/// The id of the custom entity.
		/// </summary>
		public Guid EntityId { get; set; }

		/// <summary>
		/// The main entity. This is actually stored in its own table
		/// </summary>
		[ScriptIgnore]
		public Entities.CustomObject Entity { get; set; }

		/// <summary>
		/// Initializes the client model.
		/// </summary>
		/// <param name="model">The current model</param>
		public override void Init(object model) {
			Load(((Piranha.Models.PageModel)model).Page.Id);
		}

		/// <summary>
		/// Initializes the manager edit model.
		/// </summary>
		/// <param name="model">The current model</param>
		public override void InitManager(object model) {
			Load(((Piranha.Models.Manager.PageModels.EditModel)model).Page.Id, true);
		}

		/// <summary>
		/// Saves the entity.
		/// </summary>
		/// <param name="model">The current model</param>
		public override void OnManagerSave(object model) {
			var p = (Piranha.Models.Page)model;

			Save(p.Id, p.IsDraft);
		}

		/// <summary>
		/// Returns the main entity to the client model.
		/// </summary>
		/// <param name="model">The current model</param>
		/// <returns>The main value</returns>
		public override object GetContent(object model) {
			if (Entity == null)
				Load(((Piranha.Models.PageModel)model).Page.Id);
			return Entity;
		}

		#region Private methods
		/// <summary>
		/// Loads or creates the entity.
		/// </summary>
		/// <param name="pageId">The current page id</param>
		/// <param name="draft">If it is a draft</param>
		private void Load(Guid pageId, bool draft = false) {
			if (EntityId == Guid.Empty)
				EntityId = Guid.NewGuid();

			if (pageId != Guid.Empty) {
				using (var db = new Db()) {
					Entity = db.CustomObjects
						.Where(c => c.PageId == pageId && c.Id == EntityId && c.IsDraft == draft)
						.SingleOrDefault();
					if (Entity == null) {
						Entity = new Entities.CustomObject() {
							Id = EntityId,
							PageId = pageId
						};
					}
				}
			} else {
				Entity = new Entities.CustomObject() { 
					Id = EntityId
				};
			}
		}

		/// <summary>
		/// Saves the entity.
		/// </summary>
		/// <param name="pageId">The page id</param>
		/// <param name="draft">If we're saving a draft or publishing</param>
		private void Save(Guid pageId, bool draft) {
			using (var db = new Db()) {
				var entity = db.CustomObjects
					.Where(c => c.PageId == pageId && c.Id == EntityId && c.IsDraft == true)
					.SingleOrDefault();

				if (entity == null) {
					entity = new Entities.CustomObject() { 
						Id = EntityId,
						PageId = pageId,
						IsDraft = true				
					};
					db.CustomObjects.Add(entity);
				}

				// Map values
				entity.Field1 = Entity.Field1;
				entity.Field2 = Entity.Field2;

				if (!draft) {
					entity = db.CustomObjects
						.Where(c => c.PageId == pageId && c.Id == EntityId && c.IsDraft == false)
						.SingleOrDefault();

					if (entity == null) {
						entity = new Entities.CustomObject() { 
							Id = EntityId,
							PageId = pageId,
							IsDraft = false
						};
						db.CustomObjects.Add(entity);
					}
					// Map values
					entity.Field1 = Entity.Field1;
					entity.Field2 = Entity.Field2;	
				}

				// Save
				db.SaveChanges();
			}
		}
		#endregion
	}
}