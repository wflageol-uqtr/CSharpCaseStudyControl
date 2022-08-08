# Immutability Features Instructions

As part of the treatment group in the experiment, you will have to use some newer features in C# related to immutability.  Each of these features is presented below, with links to further documentation about the feature.

_Important_:

* If you are part of the treatment group, you _must_ use each of these features _at least once_ within your code. You can use as extensively or as little as you want, but you must use them.
* If you are part of the control group, you _cannot_ use any of these features.

## Record Types
    
Records are a type of object, much like a class or a struct. They use a similar syntax to classes, with the exception of a more concise constructor and properties definition declared at the top of the record:

    record MyRecord(string Field1, int Field2) 
    {
        void SomeMethod(string arg) 
        {
            Field1 = arg;
            return Field2;
        }
    }
    
The main characteristic of Records is structural equality. Two Records are structurally equivalent if their properties (``Field1`` and ``Field2`` in the above example) are equal. This has two main effects: you can use the usual equality operators (``==`` and ``!=``) to compare Records structurally, and you it allows the usage of _Record Updating_ (more on that below).

You can find more information about Record Types here: https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/records

## Record Updating

Called "non-destructive mutation" in C#, Record Updating allows to change the data in a Record in a immutable way. This is achieved by cloning the Record and changing the data of the clone during creation, effectively leaving the original unchanged. You can use Record Updating in C# with the ``with`` keyword:

    var record = new MyRecord("Content", 0);
    var record2 = record with { Field2 = 42 };
    
You can find mre information about Record Updating here: https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/records#non-destructive-mutation

## Pattern Matching

Pattern Matching is a tool used to test whether an expression has a specific structure without having to resort to type casting. The reason type casting values is discouraged in statically typed languages is that it bypasses type safety and can easily lead to type errors (e.g., calling a method on a type that doesn't support it). However, in certain cases, type casting can be used to circumvent some limitations of the language or to write more concise, easier to understand code. Pattern Matching allows you to achieve the same results, without many of the inconvenients.

There are multiple ways to use Pattern Matching in C#. You can use it if ``if`` statements as such:

    if (someVariable is int i) 
    {
        return i + 2;
    }
    else
    {
        return 0;
    }

This code is completely safe, as no manual type casting has taken place. The compiler guarantees that in the scope where i exists, it will always be an integer, so using it in an arithmetic expression is always valid.

You can also use Pattern Matching in switch statements:

    return someVariable switch
    {
        int i => i + 2;
        string s => 1;
        _ => throw new InvalidOperationException();
    }

Note that unlike normal switch expressions, this Pattern Matching switch must handle every possible case (with the ``_`` case being the general case) or the compiler will issue a warning.

Also note that this particular switch syntax can only be used for single-line expressions (each case must return a single expression). It is not possible at the moment to write multiple statements in a single case using this syntax.

If you need to write multiple statements in a switch-like environment, you can use a chain of Pattern Matching if-else, as such:

    if(someVariable is int i)
    {
        // Do stuff with i
    }
    else if(someVariable is string s)
    {
        // Do stuff with s
    }
    else 
    {
        // General case.
    }
    
It is also possible to combine Pattern Matching and Record Types for more advanced techniques. We will not be covering those here and you are not required to use these advanced techniques in the experiment.

You can find more information about Pattern Matching here: https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/pattern-matching

## Multiple Values Return

In some cases, more frequently in functional programming, you may wish to have a single function return multiple values. For instance, you may want to return a newly created element in a hierarchy, as well as the new hierarchy created by adding that element with Record Updating. In C#, you can do this by using the Tuples syntax:

    (int, string) MakeTuple()
    {
        return (42, "Content");
    }
    
    // ...
    
    (int, string) tuple = MakeTuple();
    // Use tuple.Item1 and tuple.Item2 to access the values.
    
    // or alternatively
    
    (int i, string s) = MakeTuple();
    // Use i and s as normal variables.
    
You can find more information on tuple types here: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples
