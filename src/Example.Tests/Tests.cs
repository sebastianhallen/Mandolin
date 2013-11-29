namespace Example.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class Tests0
    {
        [Test] public void Test00(){ Assert.Pass(); }
        [Test] public void Test01(){ Assert.Pass(); }
        [Test] public void Test02(){ Assert.Pass(); }
        [Test] public void Test03(){ Assert.Pass(); }
        [Test] public void Test04(){ Assert.Pass(); }
    }
    
    [TestFixture]
    public class Tests1
    {
        [Test] public void Test10(){ Assert.Pass(); }
        [Test] public void Test11(){ Assert.Pass(); }
        [Test] public void Test12(){ Assert.Pass(); }
        [Test] public void Test13(){ Assert.Pass(); }
        [Test] public void Test14(){ Assert.Pass(); }
    }

    [TestFixture]
    public class Tests2
    {
        [Test] public void Test20(){ Assert.Pass(); }
        [Test] public void Test21(){ Assert.Pass(); }
        [Test] public void Test22(){ Assert.Pass(); }
        [Test] public void Test23(){ Assert.Pass(); }
        [Test] public void Test24(){ Assert.Pass(); }
    }

    [TestFixture]
    public class Tests3
    {
        [Test] public void Test30(){ Assert.Pass(); }
        [Test] public void Test31(){ Assert.Pass(); }
        [Test] public void Test32(){ Assert.Pass(); }
        [Test] public void Test33(){ Assert.Pass(); }
        [Test] public void Test34(){ Assert.Pass(); }
    }

    [TestFixture]
    public class Tests4
    {
        [Test] public void Test40(){ Assert.Pass(); }
        [Test] public void Test41(){ Assert.Pass(); }
        [Test] public void Test42(){ Assert.Pass(); }
        [Test] public void Test43(){ Assert.Pass(); }
        [Test] public void Test44(){ Assert.Pass(); }
    }

    [TestFixture]
    public class GenericTests0
        : GenericTestsBase<string>
    {
        [Test] public void TestInNonGenericInheritingGenericClass00() {}
        [Test] public void TestInNonGenericInheritingGenericClass01() {}
        [Test] public void TestInNonGenericInheritingGenericClass02() {}
        [Test] public void TestInNonGenericInheritingGenericClass03() {}
        [Test] public void TestInNonGenericInheritingGenericClass04() {}
    }

    [TestFixture]
    public class GenericTests1
        : GenericTestsBase<string>
    {
        [Test] public void TestInNonGenericInheritingGenericClass10() {}
        [Test] public void TestInNonGenericInheritingGenericClass11() {}
        [Test] public void TestInNonGenericInheritingGenericClass12() {}
        [Test] public void TestInNonGenericInheritingGenericClass13() {}
        [Test] public void TestInNonGenericInheritingGenericClass14() {}
    }

    [TestFixture]
    public class GenericTests2
        : GenericTestsBase<string>
    {
        [Test] public void TestInNonGenericInheritingGenericClass20() {}
        [Test] public void TestInNonGenericInheritingGenericClass21() {}
        [Test] public void TestInNonGenericInheritingGenericClass22() {}
        [Test] public void TestInNonGenericInheritingGenericClass23() {}
        [Test] public void TestInNonGenericInheritingGenericClass24() {}
    }

    [TestFixture]
    public class GenericTests3
        : GenericTestsBase<string>
    {
        [Test] public void TestInNonGenericInheritingGenericClass30() {}
        [Test] public void TestInNonGenericInheritingGenericClass31() {}
        [Test] public void TestInNonGenericInheritingGenericClass32() {}
        [Test] public void TestInNonGenericInheritingGenericClass33() {}
        [Test] public void TestInNonGenericInheritingGenericClass34() {}
    }

    [TestFixture]
    public class GenericTests4
        : GenericTestsBase<string>
    {
        [Test] public void TestInNonGenericInheritingGenericClass40() {}
        [Test] public void TestInNonGenericInheritingGenericClass41() {}
        [Test] public void TestInNonGenericInheritingGenericClass42() {}
        [Test] public void TestInNonGenericInheritingGenericClass43() {}
        [Test] public void TestInNonGenericInheritingGenericClass44() {}
    }

    public class GenericTestsBase<T>
    {
        [TestCase]
        public void GenericWithInput<TInner>(T classType, TInner methodType, object other)
        {
            Assert.Pass();
        }

        [Test]
        public void StandardInsideGeneric()
        {
            Assert.Pass();
        }
    }
}
