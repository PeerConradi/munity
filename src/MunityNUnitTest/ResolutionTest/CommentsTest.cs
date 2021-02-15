using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Models.Resolution;
using MUNity.Extensions.ResolutionExtensions;
using NUnit.Framework;

namespace MunityNUnitTest.ResolutionTest
{
    public class CommentsTest
    {
        [Test]
        public void TestNewCommentHasId()
        {
            var comment = new Comment();
            Assert.IsFalse(string.IsNullOrEmpty(comment.CommentId));
        }

        [Test]
        public void TestTagShouldBeInit()
        {
            var comment = new Comment();
            Assert.NotNull(comment.Tags);
        }

        [Test]
        public void ReadByShouldBeInit()
        {
            var comment = new Comment();
            Assert.NotNull(comment.ReadBy);
        }

        [Test]
        public void TestCreateCommentOnPreambleParagraph()
        {
            var resolution = new Resolution();
            var paragraph = resolution.CreatePreambleParagraph("Test");
            var comment = new Comment() { Text = "Neuer Text" };
            paragraph.Comments.Add(comment);
            Assert.Contains(comment, paragraph.Comments);
        }

        [Test]
        public void TestCreateCommentOnOperativeParagraph()
        {
            var resolution = new Resolution();
            var paragraph = resolution.OperativeSection.CreateOperativeParagraph("Test");
            var comment = new Comment() { Text = "Neuer Text" };
            paragraph.Comments.Add(comment);
            Assert.Contains(comment, paragraph.Comments);
        }


    }
}
