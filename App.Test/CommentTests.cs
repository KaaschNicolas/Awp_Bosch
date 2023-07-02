using App.Core.Validator;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Assert = NUnit.Framework.Assert;

namespace App.Core.Models.Tests
{
    [TestFixture]
    public class CommentTests
    {
        [Test]
        public void Content_Required_Successfully()
        {
            // Arrange
            Comment comment = new Comment();
            CommentValidator commentValidator = new CommentValidator(comment);


            // Act
            List<ValidationResult> results = new List<ValidationResult>();
            bool isValid = commentValidator.IsValid();

            // Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("The Content field is required.", results[0].ErrorMessage);
        }

        [Test]
        public void Content_MaxLength_Successfully()
        {
            // Arrange
            Comment comment = new Comment();
            comment.Content = new string('a', 650); // Set Content to a string of 650 characters
            CommentValidator commentValidator = new CommentValidator(comment);

            // Act
            List<ValidationResult> results = new List<ValidationResult>();
            bool isValid = commentValidator.IsValid();

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void NotedBy_Required_Successfully()
        {
            // Arrange
            Comment comment = new Comment();
            CommentValidator commentValidator = new CommentValidator(comment);

            // Act
            List<ValidationResult> results = new List<ValidationResult>();
            bool isValid = commentValidator.IsValid();

            // Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("The NotedBy field is required.", results[0].ErrorMessage);
        }

        [Test]
        public void NotedById_SetAndGet_Successfully()
        {
            // Arrange
            int notedById = 1;
            Comment comment = new Comment();

            // Act
            comment.NotedById = notedById;
            int result = comment.NotedById;

            // Assert
            Assert.AreEqual(notedById, result);
        }

        [Test]
        public void Pcb_SetAndGet_Successfully()
        {
            // Arrange
            Pcb pcb = new Pcb();
            Comment comment = new Comment();

            // Act
            comment.Pcb = pcb;
            Pcb result = comment.Pcb;

            // Assert
            Assert.AreEqual(pcb, result);
        }
    }
}
