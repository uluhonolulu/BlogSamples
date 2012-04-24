using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using StructureMap;

namespace EntityBinderSampleWeb {
	/// <summary>
	/// Converts entity id's to entities
	/// </summary>
	public class EntityBinderAttribute : CustomModelBinderAttribute, IModelBinder   {
		private readonly string _idName;
		private readonly Type _entityType;
		private readonly bool _relaxed;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="idName">Field name</param>
		/// <param name="entityType">Entity type</param>
		/// <param name="relaxed">If true and the entity is not found, should return null</param>
		public EntityBinderAttribute(string idName, Type entityType, bool relaxed = false) {
			_idName = idName;
			_entityType = entityType;
			_relaxed = relaxed;
		}

		public EntityBinderAttribute(Type entityType, bool relaxed = false) : this("id", entityType, relaxed) { }


		public EntityBinderAttribute(string idName, bool relaxed = false) : this(idName, null, relaxed) { }

		public EntityBinderAttribute(bool relaxed = false) : this("id", relaxed) {}

		/// <summary>
		/// Retrieves the associated model binder.
		/// </summary>
		/// <returns>
		/// A reference to an object that implements the <see cref="T:System.Web.Mvc.IModelBinder"/> interface.
		/// </returns>
		public override IModelBinder GetBinder() {
			return this;
		}

		/// <summary>
		/// Binds the model to a value by using the specified controller context and binding context.
		/// </summary>
		/// <returns>
		/// The bound value.
		/// </returns>
		/// <param name="controllerContext">The controller context.</param><param name="bindingContext">The binding context.</param>
		/// <exception cref="HttpException"><c>HttpException</c>.</exception>
		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
			var fieldName = bindingContext.ModelName + "Id";
			var result = bindingContext.ValueProvider.GetValue(fieldName);
			if (FieldNotFoundOrValueIsEmpty(result))
				fieldName = _idName;
				result = bindingContext.ValueProvider.GetValue(fieldName);
			if (FieldNotFoundOrValueIsEmpty(result)) {
				if (_relaxed) return null;
				throw new MissingFieldException("Could not find the request parameter: " + fieldName);
			}
			var entityType = _entityType ?? bindingContext.ModelType;
			var session = ObjectFactory.GetInstance<ISession>();
			if (entityType.IsArray) {
				var realType = entityType.GetElementType();
				var instances = from idString in (string[]) result.RawValue select session.Get(realType, GetId(idString, fieldName));
				return instances.ToArray();
			}
			else {
				var id = GetId(result, fieldName);
				object instance = session.Get(entityType, id);
				if (instance == null)
					bindingContext.ModelState.AddModelError("null", new HttpException(404, string.Format("Could not find {0} ({1}: {2}", entityType, fieldName, id)));
				return instance;
				
			}
		}

		private int GetId(ValueProviderResult result, string fieldName) {
			return GetId(result.AttemptedValue, fieldName);
		}

		private static int GetId(string attemptedValue, string fieldName) {
			try {
				return int.Parse(attemptedValue);
			}
			catch (FormatException) {
				throw new ArgumentException(string.Format("Invalid value for field {0}: {1}", fieldName, attemptedValue));
			}
		}

		private static bool FieldNotFoundOrValueIsEmpty(ValueProviderResult result) {
			return result == null || string.IsNullOrEmpty(result.AttemptedValue) || result.AttemptedValue == "System.Web.Mvc.UrlParameter";
		}
	}
}
