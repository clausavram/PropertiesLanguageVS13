namespace PropertiesLanguage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Utilities;

    [Export(typeof(ITaggerProvider))]
    [ContentType("ook!")]
    [TagType(typeof(PropertiesTokenTag))]
    internal sealed class PropertiesTokenTagProvider : ITaggerProvider
    {

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            return new PropertiesTokenTagger(buffer) as ITagger<T>;
        }
    }

    public class PropertiesTokenTag : ITag
    {
        public PropertiesTokenTypes type { get; private set; }

        public PropertiesTokenTag(PropertiesTokenTypes type)
        {
            this.type = type;
        }
    }

    internal sealed class PropertiesTokenTagger : ITagger<PropertiesTokenTag>
    {

        ITextBuffer _buffer;
        PropertiesTokenTypes keyType = PropertiesTokenTypes.PropertiesKey;
        PropertiesTokenTypes valueType = PropertiesTokenTypes.PropertiesValue;
        PropertiesTokenTypes commentType = PropertiesTokenTypes.PropertiesComment;

        internal PropertiesTokenTagger(ITextBuffer buffer)
        {
            _buffer = buffer;
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged { add { } remove { } }

        public IEnumerable<ITagSpan<PropertiesTokenTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            foreach (SnapshotSpan curSpan in spans)
            {
                ITextSnapshotLine containingLine = curSpan.Start.GetContainingLine();
                int lineStartLoc = containingLine.Start.Position;
                int curLoc = lineStartLoc;
                string lineText = containingLine.GetText();
                string[] commentSplitTokens = lineText.ToLower().Split(new char[] { '#' }, 2);
                string[] keyValueSplitToken = commentSplitTokens[0].Split(new char[] { '=' }, 2);

                string key = keyValueSplitToken[0];
                string value = keyValueSplitToken.Length == 1 ? null : keyValueSplitToken[1];
                string comment = commentSplitTokens.Length == 1 ? null : commentSplitTokens[1];

                if (!string.IsNullOrEmpty(key))
                {
                    var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc, key.Length));
                    if (tokenSpan.IntersectsWith(curSpan))
                        yield return new TagSpan<PropertiesTokenTag>(tokenSpan, new PropertiesTokenTag(keyType));
                    curLoc += key.Length + 1;
                }

                if (!string.IsNullOrEmpty(value))
                {
                    var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc, value.Length));
                    if (tokenSpan.IntersectsWith(curSpan))
                        yield return new TagSpan<PropertiesTokenTag>(tokenSpan, new PropertiesTokenTag(valueType));
                    curLoc += value.Length;
                }

                if (lineText.Contains("#"))
                {
                    int commentStartLoc = lineStartLoc + lineText.IndexOf('#');
                    int commentLength = lineStartLoc + lineText.Length - commentStartLoc;
                    var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(commentStartLoc, commentLength));
                    if (tokenSpan.IntersectsWith(curSpan))
                        yield return new TagSpan<PropertiesTokenTag>(tokenSpan, new PropertiesTokenTag(commentType));
                    curLoc += comment.Length + 1;
                }

            }
        }
    }
}
