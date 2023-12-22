namespace TestCalculator;

using Execution.Compiled;
using Execution;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase(".5", .5)]
    [TestCase("5.0", 5)]
    [TestCase("5.0e0", 5)]
    [TestCase("5.0e-0", 5)]
    [TestCase("5.0e+0", 5)]
    [TestCase("50e-1", 5)]
    [TestCase("500E-2", 5)]
    [TestCase("0.500E1", 5)]
    [TestCase("0.500E+1", 5)]
    [TestCase("0.5e+1", 5)]
    [TestCase(".5e+1", 5)]
    [TestCase("2.0+3.0", 5)]
    [TestCase("2+3.0", 5)]
    [TestCase("2.0+3", 5)]
    [TestCase("2.1+2.9", 5)]
    [TestCase("  2.1    +   2.9 ", 5)]
    [TestCase("7.0 - .4 - .6 ", 6)]
    [TestCase("7.0 - 2", 5)]
    [TestCase("7 - 2.0", 5)]
    [TestCase("7.0 - 2.0", 5)]
    [TestCase(" - 2.0 + 7.0", 5)]
    [TestCase(" -2.0 + 7.000", 5)]
    [TestCase(" 1/3.0 * 5.0*3 ", 5)]
    [TestCase(" 5/3.0 * 3.0 ", 5)]
    [TestCase(" 1/3.0 * 5 * 3 ", 5)]
    [TestCase(" 1.0/3 * 5 * 3 ", 5)]
    [TestCase(" (1/3.0) * (5 * 3) ", 5)]
    [TestCase(" -(1/3.0) * (-5 * 3) ", 5)]
    [Test, Category("Positive scenario")]
    public void ComputesDoubleExpression(string expression, double expected)
    {
        TypedValue? actual = Execution.CalcExpression(expression); //Calculator.Compute(expression);
        var actualDouble = actual?.doubleValue;
        //Token actual = Execution.CalcExpression(expression); //Calculator.Compute(expression);
        //var actualDouble = (actual as TokenConstant<double>).value;

        const double tolerance = 1e-100;

        Assert.That(AlmostEquals((actualDouble ?? double.NaN), expected, tolerance));
        // Assert.That(Math.Abs((actualDouble ?? double.NaN) / expected - 1), Is.LessThanOrEqualTo(tolerance));
    }

    [TestCase(" 2 +3 ", 5)]
    [TestCase(" +2 +3 ", 5)]
    [TestCase("7 - 2", 5)]
    [TestCase(" - 2 + 7", 5)]
    [TestCase(" -20 + 25  ", 5)]
    [TestCase(" (5/1) * (3 / 3) ", 5)]
    [TestCase(" (50/10)", 5)]
    [TestCase(" -(50/10)*(-1)", 5)]
    [TestCase(" -(50/10)*(-1)/(-1)", -5)]
    [TestCase(" -(50/10 + 5*2*3)*(-1)/(-1)", -35)]
    [TestCase(" -(50/10 + 5*2*3*0)*(-1)/(-1)", -5)]
    [TestCase(" -(50/10 + 5*2*3*0)*(-1)/(-1) + 2 - 2 + 0*4*5 ", -5)]
    [Test, Category("Positive scenario")]
    public void ComputesIntExpression(string expression, int expected)
    {
        TypedValue? actual = Execution.CalcExpression(expression); //Calculator.Compute(expression);
        var actualInt = actual?.intValue;
        //Token actual = Execution.CalcExpression(expression); //Calculator.Compute(expression);
        //var actualInt = (actual as TokenConstant<int>).value;

        Assert.That(actualInt, Is.EqualTo(expected));
    }

    [TestCase("var x = 1;", 0)]
    [TestCase("return;", 0)]
    [TestCase("return 1;", 1)]
    [TestCase("return 1+1;", 2)]
    [TestCase("return -1+2;", 1)]
    [TestCase("return -1+(2*30*5) +1;", 300)]
    [TestCase("return -(1+2);", -3)]
    [TestCase("return 1 + 10*3 + 9;", 40)]
    [TestCase("return 1 + (10*3 + 9) / 13 ;", 4)]
    [TestCase("""
                if (2==2) { return 1; }
              """, 1)]
    [TestCase("""
                if (2==0) { return 1; }
              """, 0)]
    [TestCase("""
                if (11 > 0) { return 1; }
              """, 1)]
    [TestCase("""
                if (11 > 0) { return 1; }
                else { return 22; }
              """, 1)]
    [TestCase("""
                if (11 < 0) { return 1; }
                else { return 22; }
              """, 22)]
    [TestCase("""
              var x = 1;
              if x == 1 { return 100; }
                else { return 500; }
              """, 100)]
    [TestCase("""
              var x = -1;
              if (x == -1) { return 100; }
                else { return 500; }
              """, 100)]
    [TestCase("""
              var x = 2;
              if (x != 1) { return 100; }
                else { return 500; }
              """, 100)]
    [TestCase("""
              var x = 2;
              if x > 1 { return 100; }
                else { return 500; }
              """, 100)]
    [TestCase("""
              var x = 2;
              if x > 10 { return 100; }
                else if (x > 100) { return 500; }
                else if (x < 5)   { return 15; }
                else  { return 99; }
              """, 15)]
    [TestCase("""
              var i = 2;
              return i; 
              """, 2)]
    [TestCase("""
              var i = 0;
              while i < 10 
              {
                i = i + 1; 
              }
              return i; 
              """, 10)]
    [TestCase("""
              var i = 0;
              var j = 2;
              while i < 10 && j <= 2 
              {
                i = i + 1;
                j = i % 3;
              }
              return i+0; 
              """, 10)]
    [TestCase("""
              var i = 0;
              while (i < 10)
              {
                i = i + 1 - 2*0; 
              }
              return i*100; 
              """, 1000)]
    [TestCase("""
              var i = 0;
              while (i < 100)
              {
                i = i + 1 - 2*0; 
                if i == 20 { 
                    return; 
                }
              }
              return 100; 
              """, 0)]
    [TestCase("""
              var i = 0;
              while (i < 100)
              {
                i = i + 1 - 2*0; 
                if i % 50 > 40 + 1.0 { 
                    return i % 50; 
                }
              }
              return 100; 
              """, 42)]
    public void TestExecInt(string expression, int expected)
    {
        TypedValue? actual = Execution.Exec(expression);
        var actualInt = actual?.intValue;
        //Token? actual = Execution.Exec(expression); //Calculator.Compute(expression);
        //var actualInt = (actual as TokenConstant<int>)?.value;

        Assert.That(actualInt, Is.EqualTo(expected));
    }

    [TestCase("""
              function f()
              {
                return 5; 
              }
              return 100; 
              """, 100)]

    [TestCase("""
              var x = 11, y=22;
              var z = 333;
              function f1()
              {
                x = 1;
                return 5; 
              }
              function f2()
              {
                x = 2;
                return 15; 
              }
              return x + y + z; 
              """, 366)]
    [TestCase("""
              var x = 7;
              function f1()
              {
                return 5; 
              }
              x = f1() + f1();
              return x; 
              """, 10)]
    [TestCase("""
              var x = 7;
              function f1()
              {
                return 5; 
              }
              x = f1() + f1()*f1();
              return x; 
              """, 30)]
    [TestCase("""
              var x = 7;
              function f1()
              {
                return 5; 
              }
              x = f1() * f1() - f1();
              return x; 
              """, 20)]
    [TestCase("""
              var x = 7;
              function f1()
              {
                x = 1;
                return 5; 
              }
              x = f1();
              return x; 
              """, 5)]
    [TestCase("""
              var x = 11;
              function f1()
              {
                x = 1;
                return 5; 
              }
              return x + f1(); // !! 1:f1(x=1) = 5    2:5 + x = 6
              """, 16)]
    [TestCase("""
              var x = 11;
              function f1()
              {
                x = 1;
                return 5; 
              }
              return (x) + f1(); // !! (x)  - x is TokenVar: ref, not value
              """, 16)]
    [TestCase("""
              var x = 11;
              function f1()
              {
                x = 1;
                return 5; 
              }
              return (x+0) + f1(); // !! (x+0)  - now value
              """, 16)]
    [TestCase("""
              var x = 11;
              function f1()
              {
                x = 1;
                return 5; 
              }
              return (x*1) + f1(); // !! 1:(x*1) = 11  2:f1()  3:5 + 11 = 16
              """, 16)]
    [TestCase("""
              var x = -1;
              var step = 0;
              function f2(a)
              {
                //x = f1(a,6) + f1(a, 1);                        //   131 + 26
                return x;                                      //   ret 157
              }
               function f1(a,b)     // a=1 b=2        a=5 b=6     a=5 b=1
              {
                x = a;              // x=1            x=5         x=5
                b = a + b + x;      // b=1+2+1=4      b=5+6+5=16  b=5+1+5=11
                a = a + b;          // a=1+4=5        a=5+16=21   a=5+11=16
                if step < 1 {       //    step = 0
                   step = step + 1; // step=1
                   x = f2(x);       // x= ...                                  x..157
                   b = x;                                              //      b=157
                } else {                              
                   b = b * 10;                   //   b=110         b=10
                }
                return a+b;                      //   ret 21+110    ret 16+10  ret 5+157
              }
              return f1(1,2); // 
              """, 6+162*0)]
    public void TestExecFunc(string expression, int expected)
    {
        TypedValue? actual = Execution.Exec(expression);
        var actualInt = actual?.intValue;

        Assert.That(actualInt, Is.EqualTo(expected));
    }


    [TestCase("""
              function f(x, y)
              {
                return x + y; 
              }
              return f(20, 30);
              """, 50)]
    [TestCase("""
              function f(x, y)
              {
                var z;
                z = x + y;
                return z; 
              }
              return f(20, 30);
              """, 50)]
    [TestCase("""
              var z;
              function f(x, y)
              {
                z = x + y;
                return z; 
              }
              return f(20, 30);
              """, 50)]
    [TestCase("""
              var z;
              function f(x, y)
              {
                var a=1,b,c=3;
                x = x + 100; // 120
                y = y * 10;  // 300
                b = x + y;   // 420
                return b; 
              }
              return f(20, 30);
              """, 420)]
    [TestCase("""
              var z;
              function f2(x)
              {
                return x*10; 
              }
              function f(x, y)
              {
                var z = f2(5); // 50
                z = z + f2(x) + f2(y); // 50 + 200 + 300
                return z; 
              }
              return f(20, 30);
              """, 550)]
    [TestCase("""
              var z = 7;
              function f2(x)
              {
                return x*10; 
              }
              function f3(x, y)
              {
                var a=x;
                var b=y+1;
                var z= 100;
                return a + b + f2(b); 
              }
              function f(x, y)
              {
                y = f3(x,y); // 20 + 31 + 310 = 361
                x = f2(y + y) + f2(y) + 7; // 7220 + 3610 + 7
                return x; 
              }
              return f(20, 30);
              """, 10837)]
    public void TestExecFuncParam(string expression, int expected)
    {
        TypedValue? actual = Execution.Exec(expression);
        var actualInt = actual?.intValue;

        Assert.That(actualInt, Is.EqualTo(expected));
    }

    [TestCase("""
              function factorial(x) {
                if (x <= 1) { return 1; } else { return x * factorial(x - 1); }
              }
              return factorial(4); 
              """, 24)] // 8!=40320 9!=362880 11!=39916800 12!=479001600 13!=6227020800 15!=1307674368000
    [TestCase("""
              var n = 10000, s = 0;
              while n > 0 {
                n = n - 1;
                s = 1; 
              }
              return s;
              """, 1, TestName = "Loop empty 10k")]
    [TestCase("""
              var n = 5000, s = 0;
              while n > 0 {
                n = n - 1;
                s = 1; 
              }
              return s;
              """, 1, TestName = "Loop empty 5k")]
    [TestCase("""
              function factorial(x) {
                if (x <= 1) { return 1; } else { return x * factorial(x - 1); }
              }

              var n = 5000, s = 0;
              while n > 0 {
                n = n - 1;
                s = factorial(12);
              }
              return s;
              """, 479001600, TestName = "Loop 12! 5k")]
    [TestCase("""
              function factorial(x) {
                if (x <= 1) { return 1; } else { return x * factorial(x - 1); }
              }

              var n = 5000, s = 0;
              while n > 0 {
                n = n - 1;
                s = factorial(6);
              }
              return s;
              """, 720, TestName = "Loop 6! 5k")]
    [TestCase("""
              function factorial(x) {
                if (x <= 1) { return 1; } else { return x * factorial(x - 1); }
              }

              var n = 5000, s = 0;
              while n > 0 {
                n = n - 1;
                s = factorial(3);
              }
              return s;
              """, 6, TestName = "Loop 3! 5k")]
    [TestCase("""
              function factorial(x) {
                if (x <= 1) { return 1; } else { return x * factorial(x - 1); }
              }

              var n = 5000, s = 0;
              while n > 0 {
                n = n - 1;
                s = factorial(12);
              }
              return s;
              """, 479001600, TestName = "Loop 12! 5k")]
    [TestCase("""
              function factorial(x) {
                if (x <= 1) { return 1; } else { return x * factorial(x - 1); }
              }

              var n = 5000, s = 0;
              while n > 0 {
                n = n - 1;
                s = factorial(12);
                s = factorial(12);
              }
              return s;
              """, 479001600, TestName = "Loop 12!,12! 5k")]
    [TestCase("""
              function factorial(x) {
                if (x <= 1) { return 1; } else { return x * factorial(x - 1); }
              }

              var n = 10000, s = 0;
              while n > 0 {
                n = n - 1;
                s = factorial(12);
              }
              return s;
              """, 479001600, TestName = "Loop 12! 10k")]
    [TestCase("""
              function factorial(x) {
                if (x <= 1) { return 1; } else { return x * factorial(x - 1); }
              }

              var n = 10000/2, s = 0;
              while n > 0 {
                n = n - 1;
                s = factorial(12);
                s = factorial(12);
              }
              return s;
              """, 479001600, TestName = "Loop 12!,12! 5k")]
    public void TestExecFactorial(string expression, int expected)
    {
        TypedValue? actual = Execution.Exec(expression);
        var actualInt = actual?.intValue;

        Assert.That(actualInt, Is.EqualTo(expected));
    }


    [TestCase("""
              function factorial(x) {
                if (x <= 1) { return 1; } else { return x * factorial(x - 1); }
              }

              factorial(2);
              return 12;
              """, 12, TestName = "FuncAsProc")]
    [TestCase("""
              function f(x) { x = 1; }
              
              f(2);
              return 12;
              """, 12, TestName = "funcNoReturn")]
    [TestCase("""
              function f(x) { x = 1; return; }
              
              f(2);
              return 12;
              """, 12, TestName = "funcNoRetval")]
    [TestCase("""
              function f(x) { x = 1; }
              var x;
              f(2);
              x = f(1) + 7; // 0 + 7
              return x;
              """, 7, TestName = "funcAutoReturn0")]
    [TestCase("""
              function f(x) { x = 1; return; }
              
              var x = 7 + f(1) + 5; // 7 + 0 + 5
              f(2);

              return x;
              """, 12, TestName = "funcAutoRetval0")]
    [TestCase("""
              function f(x) { x = 1; return; }
              function f2(x) { x = 1; }
              function f3(x) { return f(11) + f2(12); }
              
              var x = 100 + f(22) + f2(33) + f3(10); // 100 + 0 + 0 + 0
              return x;
              """, 100, TestName = "funcAutoRet")]
    public void TestExecFun(string expression, int expected)
    {
        TypedValue? actual = Execution.Exec(expression);
        var actualInt = actual?.intValue;

        Assert.That(actualInt, Is.EqualTo(expected));
    }

    [TestCase("""
              var i = 2;
              var j = i + i * 1.0; // 4.0
              var k = i * j;       // 8.0
              k = -k / (-k) * -(0-k);
              return k; // 8
              """, 8)]
    [TestCase("""
              var k = 8.0;
              if k != 8 {
                k = 11;
              }
              if -k != -8 {
                k = 12;
              }
              return k; // 8
              """, 8)]
    public void TestExecDouble(string expression, double expected)
    {
        TypedValue? actual = Execution.Exec(expression); //Calculator.Compute(expression);
        var actualDouble = actual?.doubleValue;

        const double tolerance = 1e-100;

        // both absolute and relative diffs
        Assert.That(AlmostEquals((actualDouble ?? double.NaN), expected, tolerance));

        // only absolute diff
        // Assert.That(Math.Abs((actualDouble ?? double.NaN - expected), Is.LessThanOrEqualTo(tolerance));

        // only relative diff
        // Assert.That(Math.Abs((actualDouble ?? double.NaN) / expected - 1), Is.LessThanOrEqualTo(tolerance));
    }

    [TestCase("""
              var str = 'this is string';
              return str;
              """, "this is string")]
    [TestCase("""
              var str = 'this is string';
              if str != 'this is string' {
                str = 'wtf?';
              }
              return str;
              """, "this is string")]
    [TestCase("""
              var str1 = 'string1';
              var str2 = 'string2';
              var str3;
              if str1 != str2 {
                str3 = 'wtf?';
              }
              return str1 + str2;
              """, "string1string2")]
    [TestCase("""
              var str1 = 'string';
              var str2 = 'string2';
              if str2 == str1 + '2' {
                return 'ok2';
              }
              return str1 + str2;
              """, "ok2")]
    [TestCase("""
              var str1 = 'string';
              var str2 = 'string2';
              if str2 + '34' == str1 + '2' + '34' {
                return 'ok234';
              }
              return str1 + str2;
              """, "ok234")]
    [TestCase("""
              var str1 = 'string1';
              var str2 = 'string2';
              return str1 + ' ' + str2;
              """, "string1 string2")]
    public void TestExecString(string expression, string expected)
    {
        TypedValue? actual = Execution.Exec(expression); //Calculator.Compute(expression);
        var actualString = actual?.stringValue;

        Assert.That(actualString, Is.EqualTo(expected));
    }

    [TestCase("""
              var a[] = [10,20,30];
              return a[1]; 
              """, 20)]
    [TestCase("""
              var b[3] = [100,200,300];
              return b[2]; 
              """, 300)]
    [TestCase("""
              var a[] = [10,20,30];
              var b[3] = [100,200,300];
              a[2] = a[1] + b[2]           // 20 + 300 = 320
              return b[0] * a[2];          // 100 * 320 = 32000
              """, 32000)]
    public void TestExecArrayInt(string expression, int expected)
    {
        TypedValue? actual = Execution.Exec(expression); //Calculator.Compute(expression);
        var actualRes = actual?.intValue;

        Assert.That(actualRes, Is.EqualTo(expected));
    }

    public static bool AlmostEquals(double x, double y, double tolerance)
    {
    // https://roundwide.com/equality-comparison-of-floating-point-numbers-in-csharp/

        var diff = Math.Abs(x - y);
        return diff <= tolerance ||
               diff <= Math.Max(Math.Abs(x), Math.Abs(y)) * tolerance;
    }
    
    //[Test, Category("Positive scenario")]
    //public void ComputesWithPriority()
    //{
    //    string expression = "1+2*3";
    //    double actual = Execution.Exec(expression); //Calculator.Compute(expression);
    //    double expected = 7;

    //    Assert.That(actual, Is.EqualTo(expected));
    //}

    //[Test, Category("Positive scenario")]
    //public void ComputeMultiplicationFirst()
    //{
    //    string expression = "2*4-4";
    //    double actual = Execution.Exec(expression); //Calculator.Compute(expression);
    //    double expected = 4;

    //    Assert.That(actual, Is.EqualTo(expected));
    //}

    //[Test, Category("Positive scenario")]
    //public void ComputesWithSpaces()
    //{
    //    string expression = "2 * 4 - 5";
    //    double actual = Execution.Exec(expression); //Calculator.Compute(expression);
    //    double expected = 3;

    //    Assert.That(actual, Is.EqualTo(expected));
    //}

    //[TestCase("2+3", 5)]
    //[TestCase("1+2*3", 7)]
    //[TestCase("2*4-4", 4)]
    //[TestCase("2 * 4 - 5", 3)]
    //[TestCase("(2 * 4) * 4 - 5 * (4 - 1)", 17)]
    //[TestCase("2 * (2 + 3)", 10)]
    //[TestCase("2 + (1 + 2 * 3)", 9)]
    //[TestCase("2 * (1 + 3)", 8)]
    //[TestCase("4 - (2 + 3 * 5)", -13)]
    //[TestCase("4 - ((2 + 12) / (3 + 4))", 2)]
    //[Category("Positive scenario")]
    //public void Computes(string expression, double expected)
    //{
    //    double actual = Execution.Exec(expression); //Calculator.Compute(expression);

    //    Assert.That(actual, Is.EqualTo(expected));
    //}


    ////[TestCase("2 * (1 + 3) *    ")]
    ////[TestCase("4 - (2 + 3 * 5   ")]
    ////[Test, Category("Negative scenario")]
    ////public void CalculatorReturnNaN(string expression)
    ////{
    ////    double actual = Execution.Exec(expression); //Calculator.Compute(expression);
    ////    Assert.That(actual, Is.EqualTo(double.NaN));
    ////}

    //[TestCase("2 * (1 + 3) *   ")]
    //[TestCase("4 - (2 + 3 * 5  ")]
    //[TestCase("2 & 4 - 5       ")]
    //[TestCase("+ - * /         ")]
    ////[TestCase("1 2 +           ")]
    //[TestCase("1 + ( ) *)      ")]
    //[Test, Category("Negative scenario")]
    //public void ThrowsWithInvalidOperator(string expression)
    //{
    // Assert.Throws<InvalidOperationException>(() =>
    //            Execution.Exec(expression)  //Calculator.Compute(expression)
    //              , "Operators must be +, -, * or / only."); 
    //}

    //[Test, Category("Negative scenario")]
    //public void ThrowsWithDivisionByZero()
    //{
    //    string expression = "2 / 0 - 5";

    //    Assert.Throws<DivideByZeroException>(() =>
    //            Execution.Exec(expression)  //Calculator.Compute(expression)
    //                , "Division by zero is not allowed."); 
    //}
}