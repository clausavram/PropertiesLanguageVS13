using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace PropertiesLanguage
{
    internal static class OrdinaryClassificationDefinition
    {
        #region Type definition
        
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("ook!")]
        internal static ClassificationTypeDefinition propertiesExclamation = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("ook?")]
        internal static ClassificationTypeDefinition propertiesQuestion = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("ook.")]
        internal static ClassificationTypeDefinition propertiesPeriod = null;

        #endregion
    }
}
