using NUnit.Framework;

namespace RTW.Tests
{
    public class Tests
    {
        // =================================================
        // Tests
        // =================================================

        /// <summary>
        /// This provides the RTW with text that has no tags. This means that
        /// the size of the parsed string array should be the same.
        /// </summary>
        [Test]
        public void RTW_WithTaglessText_ShouldHaveArrayLengthEqualToInputLength()
		{
            // input to test
            string testInput = "Hello world!";

            // rich text writer to be tested
            RichTextWriter writer = new RichTextWriter();

            // provide RTW with input and parse text
            writer.ReceiveText(testInput);
            writer.ParseText();

            // verify that testInput's length is the same as the # of elements in parsedString
            Assert.AreEqual(testInput.Length, writer.parsedString.Count);
		}

        /// <summary>
        /// Tests for a single pair of tags
        /// </summary>
        [Test]
        public void RTW_WithTextAndOnePairOfTags_ShouldHaveCorrectParsedString()
		{
            // input to test
            string testInput = "Hello <b>world</b>!";

            // RTW to be tested
            RichTextWriter writer = new RichTextWriter();

            // Provide RTW with input and parse text
            writer.ReceiveText(testInput);
            writer.ParseText();

            // verify that the parsed string has 12 elements (12 characters not including tags)
            Assert.AreEqual(writer.parsedString.Count, 12);

            // then do the long and horrible task of individually checking the parsed string's elements
            Assert.AreEqual("H", writer.parsedString[0]);
            Assert.AreEqual("He", writer.parsedString[1]);
            Assert.AreEqual("Hel", writer.parsedString[2]);
            Assert.AreEqual("Hell", writer.parsedString[3]);
            Assert.AreEqual("Hello", writer.parsedString[4]);
            Assert.AreEqual("Hello ", writer.parsedString[5]);
            Assert.AreEqual("Hello <b>w</b>", writer.parsedString[6]);
            Assert.AreEqual("Hello <b>wo</b>", writer.parsedString[7]);
            Assert.AreEqual("Hello <b>wor</b>", writer.parsedString[8]);
            Assert.AreEqual("Hello <b>worl</b>", writer.parsedString[9]);
            Assert.AreEqual("Hello <b>world</b>", writer.parsedString[10]);
            Assert.AreEqual("Hello <b>world</b>!", writer.parsedString[11]);
        }

        /// <summary>
        /// This tests a string having multiple tags in the same input
        /// </summary>
        [Test]
        public void RTW_WithTwoPairsOfTags_ShouldHaveCorrectParsedString()
		{
            // input to test
            string testInput = "<b>Hello</b> <i>World</i>!";

            // RTW to test
            RichTextWriter writer = new RichTextWriter();

            // provide RTW with input and parse text
            writer.ReceiveText(testInput);
            writer.ParseText();

            // check to see if the parsed string is accurate
            string[] expectedParsedString = new string[]
            {
                WrapWithTags("H", "b"),
                WrapWithTags("He", "b"),
                WrapWithTags("Hel", "b"),
                WrapWithTags("Hell", "b"),
                WrapWithTags("Hello", "b"),
                $"{WrapWithTags("Hello", "b")} ",
                $"{WrapWithTags("Hello", "b")} {WrapWithTags("W", "i")}",
                $"{WrapWithTags("Hello", "b")} {WrapWithTags("Wo", "i")}",
                $"{WrapWithTags("Hello", "b")} {WrapWithTags("Wor", "i")}",
                $"{WrapWithTags("Hello", "b")} {WrapWithTags("Worl", "i")}",
                $"{WrapWithTags("Hello", "b")} {WrapWithTags("World", "i")}",
                $"{WrapWithTags("Hello", "b")} {WrapWithTags("World", "i")}!",
            };
		}

        // =================================================
        // Helper functions
        // =================================================

        /// <summary>
        /// Wraps the content with the corresponding tags. Note that the
        /// tag must not include the angle brackets.
        /// </summary>
        /// <param name="content">The text to wrap with the tag</param>
        /// <param name="tag">The tag used to wrap the content</param>
        /// <returns>A string that is wrapped with the requested tags</returns>
        private string WrapWithTags(string content, string tag)
		{
            // open tag (taken as-is from the parameter list)
            string openTag = $"<{tag}>";

            // close tag (if a tag contains an '=' sign with a value, only the main part needs to be kept)
            string closeTag = $"</{tag.Split('=')[0]}>";

            // return the wrapped content
            return $"{openTag}{content}{closeTag}";
		}
    }
}