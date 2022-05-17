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
        public void DoubleConstructorTest() => TestRunner.Test( new DoubleConstructor() { TestString = "DoubleTest"});
        
        [TestMethod]
        public void NoConstructorTest() => TestRunner.TestThrow<NoConstructor, NullReferenceException>( new NoConstructor("ThrowTest"));

        public Tuple<string, string> TestMethod()
        {
            string s1 = "T";
            string s2 = "testest";
            s2 += "T";
            return new Tuple<string, string>(s1, s2);
        }
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

        public ParameterConstructor(string teststring, string teststring2)
        {
            TestString = teststring;
            TestString2 = teststring2;
        }

        public ParameterConstructor(int wrongtype)
        {
            throw new InvalidOperationException();
        }
        public bool Equals(ParameterConstructor other)
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
            return Equals((ParameterConstructor)obj);
        }

        public override int GetHashCode()
        {
            return (TestString != null ? TestString.GetHashCode() : 0);
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
            throw new Exception();
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
