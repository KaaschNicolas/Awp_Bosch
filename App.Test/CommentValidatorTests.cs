using App.Core.Models;
using NUnit.Framework;
using System.Collections.Generic;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Validator.Tests
{
    [TestFixture]
    public class CommentValidatorTests
    {
        [Test]
        public void Content_Required_Successfully()
        {
            // Arrange
            Comment comment = new Comment();
            CommentValidator commentValidator = new CommentValidator(comment);

            // Act
            bool isValid = commentValidator.IsValid();

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void Content_MaxLength_Successfully()
        {
            // Arrange
            Comment comment = new Comment();
            comment.Content = new string('a', 650); // Set Content to a string of 650 characters
            CommentValidator commentValidator = new CommentValidator(comment);

            // Act
            bool isValid = commentValidator.IsValid();

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void NotedBy_Required_Successfully()
        {
            // Arrange
            Comment comment = new Comment();
            CommentValidator commentValidator = new CommentValidator(comment);

            // Act
            bool isValid = commentValidator.IsValid();

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void GetErrors_ReturnsValidationErrors_Successfully()
        {
            // Arrange
            Comment comment = new Comment();
            comment.Content = null; // Set Content to null
            CommentValidator commentValidator = new CommentValidator(comment);

            // Act
            bool isValid = commentValidator.IsValid();
            List<string> errors = commentValidator.GetErrors();

            // Assert
            Assert.AreEqual(2, errors.Count);
            Assert.AreEqual("Name darf nicht null sein oder 650 Zeichen überschreiten.", errors[0]);
        }
    }
}
