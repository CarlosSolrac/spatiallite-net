﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using SpatialLite.Osm;

namespace Tests.SpatialLite.Osm {
	public class TagTests {
		#region Constructors tests

		[Fact]
		public void Constructor_TagValue_SetsKeyAndValue() {
			string key = "test-key";
			string value = "test-value";

			Tag target = new Tag(key, value);

			Assert.Equal(key, target.Key);
			Assert.Equal(value, target.Value);
		}

		[Fact]
		public void Constructor_TagValue_ThrowsExceptionIfKeyIsNull() {
			string key = null;
			string value = "test-value";

			Assert.Throws<ArgumentNullException>(delegate { new Tag(key, value); });
		}

		[Fact]
		public void Constructor_TagValue_ThrowsExceptionIfKeyIsEmpty() {
			string key = string.Empty;
			string value = "test-value";

			Assert.Throws<ArgumentException>(delegate { new Tag(key, value); });
		}

		[Fact]
		public void Constructor_TagValue_ThrowsExceptionIfValueIsNull() {
			string key = "test-key";
			string value = null;

			Assert.Throws<ArgumentNullException>(delegate { new Tag(key, value); });
		}

		#endregion

		#region Equals tests

		[Fact]
		public void Equals_ReturnTrueForSameKeyAndValue() {
			Tag target = new Tag("test-key", "test-value");
			Tag other = new Tag("test-key", "test-value");

			Assert.True(target.Equals(other));
		}

		[Fact]
		public void Equals_ReturnTrueForSameKeyAndValueAsObject() {
			Tag target = new Tag("test-key", "test-value");
			Tag other = new Tag("test-key", "test-value");

			Assert.True(target.Equals((object)other));
		}

		[Fact]
		public void Equals_ReturnFalseForDifferentKeys() {
			Tag target = new Tag("test-key", "test-value");
			Tag other = new Tag("different-key", "test-value");

			Assert.False(target.Equals(other));
		}

		[Fact]
		public void Equals_ReturnFalseForDifferentValues() {
			Tag target = new Tag("test-key", "test-value");
			Tag other = new Tag("test-key", "different-value");

			Assert.False(target.Equals(other));
		}

		[Fact]
		public void Equals_ReturnFalseForOtherObject() {
			Tag target = new Tag("test-key", "test-value");
			string other = "Test";

			Assert.False(target.Equals(other));
		}

		[Fact]
		public void Equals_ReturnFalseForNull() {
			Tag target = new Tag("test-key", "test-value");
			object other = null;

			Assert.False(target.Equals(other));
		}

		#endregion
	}
}
