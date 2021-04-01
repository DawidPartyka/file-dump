using NUnit.Framework;
using System;
using NUnit.Samples.Cash;
using Moq;

namespace unitTest
{
    public class Tests
    {
        private Cash f14CHF;

        [SetUp]
        public void Setup()
        {
            f14CHF = new Cash(14, "CHF");

            Console.WriteLine("This is SetUp");
        }

        [Test]
        [Category("Smoke")]
        public void xxx()
        {
            var x = 1;
            var y = 2;
            Assert.AreEqual(x, y);
        }

        [Test]
        [Category("Smoke")]
        public void xx()
        {
            var x = 3;
            var y = 3;
            Assert.AreEqual(x, y);
        }

        /// <summary>
        /// Assert that multiplying currency in Cash happens correctly
        /// </summary>
        [Test]
        public void SimpleMultiply()
        {
            // [14 CHF] *2 == [28 CHF]
            Cash expected = new Cash(28, "CHF");
            Assert.AreEqual(expected, f14CHF.Multiply(2));
        }

        [Test]
        public void SimpleAdd()
        {
            // [14 CHF] + [14 CHF] - [14 CHF] == [14 CHF]
            Cash expected = new Cash(14, "CHF");
            f14CHF.Add(expected);
            f14CHF.Subtract(expected);

            Assert.AreEqual(expected, f14CHF);
        }

        [Test]
        public void AddMoneyTest()
        {
            Cash add = new Cash(5, "USD");
            CashBag reference = new CashBag(add, f14CHF);

            Assert.AreEqual(reference, f14CHF.AddMoney(add));
        }

        [Test]
        public void EqualsMethodWithNotHandledObject()
        {
            CashBag notHandledObject = new CashBag(new Cash(5, "USD"), f14CHF);

            Assert.AreEqual(false, f14CHF.Equals(notHandledObject));
        }

        [Test]
        public void isZeroAndNegateCheck()
        {
            Cash expected = new Cash(0, "CHF");
            Assert.AreEqual(expected, f14CHF.Add(f14CHF.Negate()));
        }

        [Test]
        public void MoneyBagTest()
        {
            Cash start = new Cash(6, "CHF");
            Cash add = new Cash(5, "PLN");
            CashBag test = new CashBag(start, add);
            
            Assert.AreEqual(start.AddMoneyBag(test), test.Multiply(2).Subtract(add));
        }

        /// <summary>
        /// Test set Currency , Data-Driven Testing
        /// </summary>
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void SetCurrency_ChangeCurrencyToCHF_TwoObjectCurrenciesAreEqual(int amount)
        {
            // arrange
            Cash c2 = new Cash(amount, "CHF");
            Cash c = new Cash(amount, "PLN");

            // act
            c.SetCurrency("CHF");

            // assert
            Assert.AreEqual(c2.Currency, c.Currency);
        }

        /// <summary>
        ///  TDD Cash_class.ToString() test
        /// </summary>

        [TestCase(14, "CHF")]
        [TestCase(15, "PLN")]
        [TestCase(0, "CAD")]
        public void ToString_GetStringOfClassCash_ReceivedStringIsCorrect(int amount, string currency)
        {
            // arrange
            Cash test = new Cash(amount, currency);

            // act
            string toTest = test.ToString();

            // assert
            Assert.AreEqual($"[{amount} {currency}]", toTest);
        }

        [Test]
        public void Hashes()
        {
            Cash chf = new Cash(6, "CHF");
            Cash chf2 = new Cash(3, "CHF");

            Assert.AreEqual(chf.GetHashCode(), chf2.Multiply(2).GetHashCode());
        }

        /// <summary>
        /// Test CashBag method Equals with Cash object expecting output = false
        /// </summary>
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void CashBagWithCashObjectEqualsTests(int cur)
        {
            // arrange
            Cash dummy = new Cash(cur, "CAD");

            // act
            CashBag test = new CashBag(dummy, new Cash(3, "USD"));
            bool dummyTest = test.Equals(dummy);

            // assert
            Assert.AreEqual(false, dummyTest);
        }

        /// <summary>
        /// Test CashBag method Equals with CashBag passed as parameter
        /// </summary>
        [TestCase(1, "CHF", 5, "USD", 5, true)]
        [TestCase(104, "CAD", 44, "CKR", 44, true)]
        [TestCase(25, "PLN", 3, "USD", 3, true)]
        [TestCase(1, "CHF", 2, "CAD", 3, false)]
        public void CashBagEqualsTest(int c1, string c1n, int c2, string c2n, int c3, bool expected)
        {
            // arrange
            Cash cash1 = new Cash(c1, c1n);
            Cash cash2 = new Cash(c2, c2n);
            Cash cash3 = new Cash(c3, c2n);

            // act
            CashBag testEquals = new CashBag(cash1, cash2);
            CashBag equalsTo = new CashBag(cash1, cash3);

            // assert
            Assert.AreEqual(expected, testEquals.Equals(equalsTo));
        }

        /// <summary>
        /// Test CashBag Equals method with CashBag having different amount of
        /// currencies stored passed as parameter expecting output = false
        /// </summary>
        [TestCase(1, 2, 3, "USD", "CAD", "CHF")]
        [TestCase(5, 7, 8, "PLN", "CAD", "USD")]
        [TestCase(2, 3, 5, "CKR", "USD", "CHF")]
        public void CashBagEqualsDifferentCurrenciesLengthTest(int a, int b, int c, string c1, string c2, string c3)
        {
            // arrange
            Cash cash1 = new Cash(a, c1);
            Cash cash2 = new Cash(b, c2);
            Cash cash3 = new Cash(c, c3);

            // act
            CashBag test = new CashBag(cash1, cash2);
            CashBag breakTest = new CashBag(cash1, cash2);
            breakTest = (CashBag)breakTest.AddMoney(cash3);

            // assert
            Assert.AreEqual(false, test.Equals(breakTest));
        }

        [TearDown]
        public void Teardown() {
            Console.WriteLine("This is TearDown");
        }

        [Test]
        [Category("Unit")]
        public void TestMock()
        {
            //assert
            var Cash = new Cash(2, "PLN");
            var mockBag = new Mock<ICash>();
            mockBag.Setup(x => x.AddMoney(It.IsAny<Cash>())).Returns(new Cash(1, "CHF"));

            //act
            Cash.AddMoneyBag(mockBag.Object);

            //assert
            Assert.IsTrue(true);
            mockBag.Verify(mock => mock.AddMoney(It.IsAny<Cash>()), Times.Once());
        }

    }
}