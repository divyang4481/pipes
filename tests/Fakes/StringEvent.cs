using System;

namespace hq.pipes.tests.Fakes
{
    public class StringEvent : IEquatable<StringEvent>
    {
        public string Text { get; set; }

        public StringEvent() { }

        public StringEvent(string text)
        {
            Text = text;
        }

        public bool Equals(StringEvent other)
        {
            if (other == null) return false;
            return Text != null && other.Text.Equals(Text);
        }

        public override string ToString()
        {
            return Text;
        }
    }
}