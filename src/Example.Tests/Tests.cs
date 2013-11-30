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
        [Test, Category("DontRun")]     public void TestInNonGenericInheritingGenericClass00() { Assert.Fail(); }
        [Test, Category("MustRun")]     public void TestInNonGenericInheritingGenericClass01() { Assert.Pass(); }
        [Test, Category("NeverRun")]    public void TestInNonGenericInheritingGenericClass02() { Assert.Fail(); }
        [Test, Category("ShouldRun")]   public void TestInNonGenericInheritingGenericClass03() { Assert.Pass(); }
        [Test, Category("Other")]       public void TestInNonGenericInheritingGenericClass04() { Assert.Pass(); }
    }

    [TestFixture]
    public class GenericTests1
        : GenericTestsBase<string>
    {
        [Test, Category("DontRun")]     public void TestInNonGenericInheritingGenericClass10() { Assert.Fail(); }
        [Test, Category("MustRun")]     public void TestInNonGenericInheritingGenericClass11() { Assert.Pass(); }
        [Test, Category("NeverRun")]    public void TestInNonGenericInheritingGenericClass12() { Assert.Fail(); }
        [Test, Category("ShouldRun")]   public void TestInNonGenericInheritingGenericClass13() { Assert.Pass(); }
        [Test, Category("Other")]       public void TestInNonGenericInheritingGenericClass14() { Assert.Pass(); }
    }

    [TestFixture]
    public class GenericTests2
        : GenericTestsBase<string>
    {
        [Test, Category("DontRun")]     public void TestInNonGenericInheritingGenericClass20() { Assert.Fail(); }
        [Test, Category("MustRun")]     public void TestInNonGenericInheritingGenericClass21() { Assert.Pass(); }
        [Test, Category("NeverRun")]    public void TestInNonGenericInheritingGenericClass22() { Assert.Fail(); }
        [Test, Category("ShouldRun")]   public void TestInNonGenericInheritingGenericClass23() { Assert.Pass(); }
        [Test, Category("Other")]       public void TestInNonGenericInheritingGenericClass24() { Assert.Pass(); }
    }

    [TestFixture]
    public class GenericTests3
        : GenericTestsBase<string>
    {
        [Test, Category("DontRun")]     public void TestInNonGenericInheritingGenericClass30() { Assert.Fail(); }
        [Test, Category("MustRun")]     public void TestInNonGenericInheritingGenericClass31() { Assert.Pass(); }
        [Test, Category("NeverRun")]    public void TestInNonGenericInheritingGenericClass32() { Assert.Fail(); }
        [Test, Category("ShouldRun")]   public void TestInNonGenericInheritingGenericClass33() { Assert.Pass(); }
        [Test, Category("Other")]       public void TestInNonGenericInheritingGenericClass34() { Assert.Pass(); }
    }

    [TestFixture]
    public class GenericTests4
        : GenericTestsBase<string>
    {
        [Test, Category("DontRun")]     public void TestInNonGenericInheritingGenericClass40() { Assert.Fail(); }
        [Test, Category("MustRun")]     public void TestInNonGenericInheritingGenericClass41() { Assert.Pass(); }
        [Test, Category("NeverRun")]    public void TestInNonGenericInheritingGenericClass42() { Assert.Fail(); }
        [Test, Category("ShouldRun")]   public void TestInNonGenericInheritingGenericClass43() { Assert.Pass(); }
        [Test, Category("Other")]       public void TestInNonGenericInheritingGenericClass44() { Assert.Pass(); }
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
