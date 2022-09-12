# Case Study on Immutable Features in C#

This document contains the instructions for participation in a case study on C# immutability features. Following is the specifications for the program to be completed.

If you are part of the treatment (new features) group, please also read the [immutability features instructions](https://github.com/wflageol-uqtr/CSharpCaseStudy/blob/main/immutability-features.md).

## The Program

As part of this study, you will implement a simple, but non-trivial, file system simulator. The skeleton of the project is available as part of this repository. It contains some basic interfaces and a facade to complete.

To verify that your implementation is sound, a series of integration tests were also included in the project. Your goal is to make these tests pass without error. *Be mindful that the data used in the test might be subject to change, so you should not tailor your code to that particular data set but write a complete implementation.*

## General Specifications

The file system implementation should support creating a hierarchy of folders and files. A folder may include files and other folders. The system should support a handful of simple operations, such as renaming or deleting a component. The exhaustive list of features is covered by the integration tests, which are described further down.

As an additional constraint, and because it is the subject of the study, your file system implementation should be immutable. Any operation done to change the structure of the file system should not actually modify any object within the system. For example, if you rename a file, you should be creating a new file, duplicating the original file, but with a new name. This is also covered by the integration tests.

You are allowed to have mutable variables in your methods, but the objects composing your file system should be fully immutable (attributes should be read-only and only set within constructors). You should use collections within the System.Collections.Immutable namespace to make things easier.

Note that the FileSystemFacade class is allowed to have a single mutable attribute to represent the current state of the file system (ideally some kind of collection to represent previous states as well, as that would help implementing the undo operation!).

## Integration Tests Documentation

What follows is an explanation for what is expected and the reasoning behind each integration test. Your file system has a facade, called FileSystemFacade. This facade will be used by the integration tests to interact with your file system.

### TestGetFileByPath & TestGetFileByPathNotFound

There should be a way to obtain a component from your file system. Your facade contains a GetFileByPath method which should return a component of your file system given a root folder and the names of the components to traverse to reach the target component. If nothing is found at the given path, null should be returned.

### TestRename

As mentionned earlier, the rename feature should not actualy modify the name of the given component, but create a new component with the new name and replace it in the file system. This test will make sure that a new component is created and that the original component was not modified.

### TestDelete

Deleting a component is just removing it from the file system. No change needs to be done on the component itself.

### TestRenameNotify & TestDeleteNotify

A notification system should be included in your file system. When NotifyOnChange is called on a component, changes to that component (i.e. renaming and deleting) should be logged in the NotificationLog property of the facade. This way, the integration tests can access the log information. Here are the exact messages that should be logged:

* On rename: "{Original Name} was renamed to {New Name}."
* On delete: "{Name} was deleted."

Deleting a folder should also call delete notifications for all observed children.

### TestCollect*

Your file system should support an operation to walk through it, collecting files and folders as needed. The Collect method is this operation. Collect can be called with a root folder and a few flags on whether to collect files, folders, and if the operation should be recursive (collect in child folders too). The method should return an enumeration of every component collected this way. 

The various integration tests will cover each use case of the operation.

### TestUndoRename & TestUndoDelete

Your file system should support an Undo operation which is used to walk back any operation that was done previously. Since the system is immutable, it should be a simple matter of keeping a collection of previous states and reverting to a previous state whenever undo is called.

### TestDuplicate

The final features your system should support is duplication. Given a root component, the Duplicate method should create a complete duplication of the child hierarchy of that component (including the component itself). Each new component created this way should be a copy of the original components, but should be distinct objects.

## Additional Notes

Here are some notes and features that may be useful for developping the project.

### Multiple Values Return

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
