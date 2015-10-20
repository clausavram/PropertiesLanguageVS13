using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace PropertiesLanguage
{
    #region Format definition
    /// <summary>
    /// Defines the editor format for the PropertiesKey classification type. Text is colored light blue.
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "ook!")]
    [Name("ook!")]
    //this should be visible to the end user
    [UserVisible(false)]
    //set the priority to be after the default classifiers
    [Order(Before = Priority.Default)]
    internal sealed class PropertiesKey : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "key" classification type
        /// </summary>
        public PropertiesKey()
        {
            DisplayName = "Properties Key"; //human readable version of the name
            ForegroundColor = Color.FromRgb(86, 156, 214);  // Dark theme
            //ForegroundColor = Color.FromRgb(0, 0, 255);   // Light theme
        }
    }

    /// <summary>
    /// Defines the editor format for the PropertiesValue classification type. Text is colored light orange.
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "ook?")]
    [Name("ook?")]
    //this should be visible to the end user
    [UserVisible(false)]
    //set the priority to be after the default classifiers
    [Order(Before = Priority.Default)]
    internal sealed class PropertiesValue : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "value" classification type
        /// </summary>
        public PropertiesValue()
        {
            DisplayName = "Properties Value"; //human readable version of the name
            ForegroundColor = Color.FromRgb(214, 157, 113);   // Dark theme
            //ForegroundColor = Color.FromRgb(163, 21, 21);   // Light theme
        }
    }

    /// <summary>
    /// Defines the editor format for the PropertiesComment classification type. Text is colored Gray.
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "ook.")]
    [Name("ook.")]
    //this should be visible to the end user
    [UserVisible(false)]
    //set the priority to be after the default classifiers
    [Order(Before = Priority.Default)]
    internal sealed class PropertiesComment : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "period" classification type
        /// </summary>
        public PropertiesComment()
        {
            DisplayName = "Properties Comment"; //human readable version of the name
            ForegroundColor = Colors.Gray;  // Dark theme
            //ForegroundColor = Color.FromRgb(128, 128, 128);   // Light theme
        }
    }
    #endregion //Format definition
}
