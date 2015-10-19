namespace PropertiesLanguage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Utilities;

    [Export(typeof(ITaggerProvider))]
    [ContentType("ook!")]
    [TagType(typeof(ClassificationTag))]
    internal sealed class PropertiesClassifierProvider : ITaggerProvider
    {

        [Export]
        [Name("ook!")]
        [BaseDefinition("text")]
        internal static ContentTypeDefinition PropertiesContentType = null;

        [Export]
        [FileExtension(".properties")]
        [ContentType("ook!")]
        internal static FileExtensionToContentTypeDefinition PropertiesFileType = null;

        [Export]
        [FileExtension(".profile")]
        [ContentType("ook!")]
        internal static FileExtensionToContentTypeDefinition ProfilePropertyFileType = null;

        [Import]
        internal IClassificationTypeRegistryService ClassificationTypeRegistry = null;

        [Import]
        internal IBufferTagAggregatorFactoryService aggregatorFactory = null;

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {

            ITagAggregator<PropertiesTokenTag> propertiesTagAggregator = 
                                            aggregatorFactory.CreateTagAggregator<PropertiesTokenTag>(buffer);

            return new PropertiesClassifier(buffer, propertiesTagAggregator, ClassificationTypeRegistry) as ITagger<T>;
        }
    }

    internal sealed class PropertiesClassifier : ITagger<ClassificationTag>
    {
        ITextBuffer _buffer;
        ITagAggregator<PropertiesTokenTag> _aggregator;
        IDictionary<PropertiesTokenTypes, IClassificationType> _propertiesTypes;

        /// <summary>
        /// Construct the classifier and define search tokens
        /// </summary>
        internal PropertiesClassifier(ITextBuffer buffer, 
                               ITagAggregator<PropertiesTokenTag> propertiesTagAggregator, 
                               IClassificationTypeRegistryService typeService)
        {
            _buffer = buffer;
            _aggregator = propertiesTagAggregator;
            _propertiesTypes = new Dictionary<PropertiesTokenTypes, IClassificationType>();
            _propertiesTypes[PropertiesTokenTypes.PropertiesKey] = typeService.GetClassificationType("ook!");
            _propertiesTypes[PropertiesTokenTypes.PropertiesValue] = typeService.GetClassificationType("ook?");
            _propertiesTypes[PropertiesTokenTypes.PropertiesComment] = typeService.GetClassificationType("ook.");
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }

        /// <summary>
        /// Search the given span for any instances of classified tags
        /// </summary>
        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            foreach (var tagSpan in _aggregator.GetTags(spans))
            {
                var tagSpans = tagSpan.Span.GetSpans(spans[0].Snapshot);
                yield return 
                    new TagSpan<ClassificationTag>(tagSpans[0], 
                                                   new ClassificationTag(_propertiesTypes[tagSpan.Tag.type]));
            }
        }
    }
}
