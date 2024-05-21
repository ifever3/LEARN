using System.Reflection;

namespace LEARN.extensions
{
    internal class SwaggerDocs
    {
        public static readonly string XmlName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    }
}
