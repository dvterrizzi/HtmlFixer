# HtmlFixer

## Introduction
Atempts to fix the message.html files exported by Facebook by replacing the `src` attribute of `<img>' tags.

## Dependencies

- .NET 8
- HtmlAgilityPack 1.11.17

## Build

Use Visual Studio to build a Debug or Release build.

## Run
Use Visual Studio to run the built project, either with or without debugger.

## Files

### Program.cs

Main entry point for the application.
Instantiates a new `MainForm`.

### MainForm.cs

The window with the Browse button.
Asks the user for the master folder and uses the static `Fixer` class to do the fixing.

When double clicked, the Form Designer opens. By right ciicking and choosing `View Code` you can see the code that gets run when the Browse button is clicked.

### Fixer.cs

Static class that can be called anywhere.
Given a starting folder it will attempt to recursively find all files that match `message*.html`. Every file will be sequentially processed by finding any `<img>` tags which have the class `2yuc`. 
The `src` attribute will be updated by scanning the `photos` folder next to the current HTML file to find a matching photo.
After replacing the `src` of all `img` tags it will attempt to overwrite the loaded HTML file.