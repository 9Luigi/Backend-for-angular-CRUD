
using System.Reflection;
namespace Backend_for_angular_CRUD;
public static class FieldsController
{
	public static void CopyFields<T>(T source, T target)
	{
		if (source == null || target == null)
			throw new ArgumentNullException();

		var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

		foreach (var field in fields)
		{
			var value = field.GetValue(source);
			field.SetValue(target, value);
		}
	}

}
