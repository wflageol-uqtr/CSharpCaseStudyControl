# Case Study on Immutable Features in C#

This document contains the instructions for participation in a case study on C# immutability features. These instructions are for the treatment group of the experiment.

Following is the specifications for the program to be completed.

## The Program

As part of this study, you will read and add functionality to a simple, but non-trivial, file system simulator. The project is available as part of this repository. It contains the basic functionality of the file system to which you'll have to add some new features.

To verify that your implementation is sound, a series of integration tests were also included in the project. At the start of the experiment, only 6 of the 13 unit tests will pass. Your goal is to make all the tests pass without error. *Be mindful that the data used in the test might be subject to change, so you should not tailor your code to that particular data set but write a complete implementation.*

## General Specifications

The file system implementation supports creating a hierarchy of folders and files. A folder may include files and other folders. The system supports a handful of simple operations, such as renaming or deleting a component. The exhaustive list of features is covered by the integration tests, which are described further down.

As an additional constraint, and because it is the subject of the study, your file system implementation is immutable and should remain so. Any operation done to change the structure of the file system should not actually modify any object within the system. For example, when a file is renamed, a new file is created, duplicating the original file, but with a new name. This is also covered by the integration tests.

You are allowed to have mutable variables in your methods, but the objects composing your file system should be fully immutable (attributes should be read-only and only set within constructors). You should use collections within the System.Collections.Immutable namespace to make things easier.

Note that the FileSystemFacade class has a single mutable attribute to represent the current state of the file system (you may want to change this to some kind of collection to represent previous states as well, as that would help implementing the undo operation!).

You are allowed to add interfaces, classes and methods as you see fit to complete the experiment. *You are not allowed to modify the unit tests in the FileWalkerTests project.*

## Integration Tests Documentation

What follows is an overview of the implemented unit tests for the system. The file system has a facade, called FileSystemFacade. This facade is used by the integration tests to interact with the file system.

### TestGetFileByPath & TestGetFileByPathNotFound

This tests that we can obtain a component from the file system. The facade contains a GetFileByPath method which should return a component of the file system given a root folder and the names of the components to traverse to reach the target component. If nothing is found at the given path, null should be returned. This functionality is already implemented.

### TestRename

As mentionned earlier, the rename feature does not actualy modify the name of the given component, but creates a new component with the new name and replaces it in the file system. This test makes sure that a new component is created and that the original component was not modified. This functinoality is already implemented.

### TestDelete

Deleting a component is just removing it from the file system. No change needs to be done on the component itself. This functionality is already implemented.

### TestRenameNotify & TestDeleteNotify

A notification system is included in the file system. When NotifyOnChange is called on a component, changes to that component (i.e. renaming and deleting) are logged in the NotificationLog property of the facade. This way, the integration tests can access the log information. Deleting a folder also calls delete notifications for all observed children. This functionality is already implemented.

### TestCollect*

The file system should support an operation to walk through it, collecting files and folders as needed. The Collect method is this operation. Collect can be called with a root folder and a few flags on whether to collect files, folders, and if the operation should be recursive (collect in child folders too). The method should return an enumeration of every component collected this way. 

The various integration tests cover each use case of the operation. *You need to implement this functinality.*

### TestUndoRename & TestUndoDelete

The file system should support an Undo operation which is used to walk back any operation that was done previously. *You need to implement this functinality.*

### TestDuplicate

The file system should support duplication. Given a root component, the Duplicate method should create a complete duplicate of the child hierarchy of that component (including the component itself). Each new component created this way should be a copy of the original components, but should be distinct objects. *You need to implement this functinality.*

## Language Features

The objective of this study is to measure the impact of a specific set of language features on immutable object-oriented software. As such, you should read the [immutability features instructions](https://github.com/wflageol-uqtr/CSharpCaseStudy/blob/main/immutability-features.md).

The instructions describe the set of features which you should use to help complete your implementation of the new functionality. The existing code already makes use of these features.


