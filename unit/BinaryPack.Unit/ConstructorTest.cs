using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryPack.Unit
{
    [TestClass]
    public class ConstructorTest
    {
        [TestMethod]
        public void EmptyConstructorTest() => TestRunner.Test( new EmptyConstructor() { TestString = "Test"});
        
        [TestMethod]
        public void ParameterConstructorTest() => TestRunner.Test( new ParameterConstructor("ParameterTest", "AnotherParameter"));
        
        [TestMethod]
        public void SwappedParameterConstructorTest() => TestRunner.Test( new SwappedParameterConstructor("SwappedParameterTest", "SwappedAnotherParameter"));

        [TestMethod]
        public void DoubleConstructorTest() => TestRunner.Test( new DoubleConstructor() { TestString = "DoubleTest"});
        
        [TestMethod]
        public void NoConstructorTest() => TestRunner.TestThrow<NoConstructor, TypeInitializationException>( new NoConstructor("ThrowTest"));
    }

    public class EmptyConstructor : IEquatable<EmptyConstructor>
    {
        public string TestString;

        public EmptyConstructor()
        {
        }

        public bool Equals(EmptyConstructor other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return TestString == other.TestString;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EmptyConstructor)obj);
        }

        public override int GetHashCode()
        {
            return (TestString != null ? TestString.GetHashCode() : 0);
        }
    }
    
    public class ParameterConstructor : IEquatable<ParameterConstructor>
    {
        public string TestString;
        public string TestString2;

        public ParameterConstructor(int wrongParameterCount)
        {
            throw new InvalidOperationException("Used a constructor with wrong type count");
        }
        
        public ParameterConstructor(int teststring, string teststring2)
        {
            throw new InvalidOperationException("Used a constructor with parameter type");
        }
        
        public ParameterConstructor(string teststring, string teststring2)
        {
            TestString = teststring;
            TestString2 = teststring2;
        }

        public bool Equals(ParameterConstructor other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return TestString == other.TestString && TestString2 == other.TestString2;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ParameterConstructor)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TestString, TestString2);
        }
    }

    public class SwappedParameterConstructor : IEquatable<SwappedParameterConstructor>
    {
        public string TestString;
        public string TestString2;

        public SwappedParameterConstructor(string teststring2, string teststring)
        {
            TestString = teststring;
            TestString2 = teststring2;
        }

        public bool Equals(SwappedParameterConstructor other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return TestString == other.TestString && TestString2 == other.TestString2;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SwappedParameterConstructor)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TestString, TestString2);
        }
    }

    public class DoubleConstructor : IEquatable<DoubleConstructor>
    {
        public string TestString;

        public DoubleConstructor()
        {
        }
        
        public DoubleConstructor(string TestString)
        {
            throw new InvalidOperationException("Used a parameter constructor while a parameterless is available");
        }

        public bool Equals(DoubleConstructor other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return TestString == other.TestString;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DoubleConstructor)obj);
        }

        public override int GetHashCode()
        {
            return (TestString != null ? TestString.GetHashCode() : 0);
        }
    }
    
    public class NoConstructor
    {
        public string TestString;

        public NoConstructor(string TestString2)
        {
        }
    }
}
